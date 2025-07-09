using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Entity
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] public SpriteRenderer sprite;
    private float dropBackForce = 3f;

    private Animator anim;
    private Collider2D col;

    private Vector3 targetPosition;
    private bool movingToB = true, isDamaged = false, isCry = false, isShooting = false;
    private float arrivalThreshold = 0.1f;

    private float detectionDistanceX = 5f;
    private float detectionDistanceY = 1f;
    private bool isTargetDetected;

    public Transform shotPos;
    public GameObject bullet;

    private bool canShoot = true;
    public static Enemy Instance { get; set; }

    private States State
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }

    private void Start()
    {
        lives = 5;
        sprite = GetComponentInChildren<SpriteRenderer>();
        targetPosition = pointB.position;
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

    }

    private void Update()
    {
        if (!isDamaged && !isCry)
        {
            if(!isShooting) State = States.run;
            Move();
            DetectMent();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (lives > 0 && collision.gameObject == Ment.Instance.gameObject)
        {
            Vector3 hitDirection = Ment.Instance.transform.position - transform.position;
            Ment.Instance.DropBack(hitDirection);
            Ment.Instance.GetDamage();
            lives--;
        }
        else if (lives == 0) Die();
    }
    private void DetectMent()
    {
        if (Ment.Instance == null) return;
        float directionX = Ment.Instance.transform.position.x - transform.position.x;
        float directionY = Ment.Instance.transform.position.y - transform.position.y;
        isTargetDetected = (Mathf.Abs(directionX) <= detectionDistanceX) && (Mathf.Abs(directionY) <= detectionDistanceY);

        if (isTargetDetected && canShoot)
        {
            State = States.attack;
            isShooting = true;
            Shoot();
            StartCoroutine(ShootTime());
            StartCoroutine(RechargeTime());
            Debug.Log("detedct");
        }
    }
    private void Shoot()
    {
        GameObject newBullet = Instantiate(bullet, shotPos.transform.position, transform.rotation);
        float direction = sprite.flipX ? -1f : 1f;
        newBullet.GetComponent<Bullet>().SetDirection(direction);
        canShoot = false;
    }
    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        if (moveDirection != Vector3.zero)
        {
            sprite.flipX = moveDirection.x < 0;
        }
        if (Vector3.Distance(transform.position, targetPosition) < arrivalThreshold)
        {
            movingToB = !movingToB;
            targetPosition = movingToB ? pointB.position : pointA.position;
        }
        UpdateShotPosition();
    }
    private void UpdateShotPosition()
    {
        float offsetX = sprite.flipX ? -Mathf.Abs(shotPos.localPosition.x) : Mathf.Abs(shotPos.localPosition.x);
        shotPos.localPosition = new Vector3(offsetX, shotPos.localPosition.y, shotPos.localPosition.z);
    }
    public override void GetDamage()
    {
        if (isCry) return;
        lives--;
        if (lives == 0)
        {
            Die();
        }
        else
        {
            State = States.damage;
            isDamaged = true;
            StartCoroutine(DamageCoolDown());
        }
        Debug.Log($"{lives} lives left(Enemy)");
    }
    public void DropBack(Vector2 direction)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Debug.Log(dropBackForce);
            rb.AddForce(direction.normalized * dropBackForce, ForceMode2D.Impulse);
        }
    }
    private IEnumerator DamageCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        isDamaged = false;
    }
    private IEnumerator RechargeTime()
    {
        yield return new WaitForSeconds(2.5f);
        canShoot = true;
    }
    private IEnumerator ShootTime()
    {
        yield return new WaitForSeconds(0.8f);
        isShooting = false;
    }

    public enum States
    {
        idle,
        run,
        attack,
        damage,
        none
    }
    public override void Die()
    {
        State = States.none;
        col.isTrigger = true;
        anim.SetTrigger("cry");
        isCry = true;
        Debug.Log($"Boy cryes");
    }
}