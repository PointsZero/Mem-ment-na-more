using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : Entity
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private SpriteRenderer sprite; 

    private Vector3 targetPosition;
    private bool movingToB = true;
    private float arrivalThreshold = 0.1f;

    public float detectionDistance = 10f;
    public bool isTargetDetected;

    public Transform shotPos;
    public GameObject bullet;

    private bool canShoot = true;
    public static Enemy Instance { get; set; }
    private void Start()
    {
        lives = 5;
        sprite = GetComponentInChildren<SpriteRenderer>();
        targetPosition = pointB.position;
    }

    private void Update()
    {
        Move();
        DetectMent();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Ment.Instance.gameObject)
        {
            Ment.Instance.GetDamage();
            Debug.Log($"{lives} lives left(Enemy)");
            lives--;
            if (lives <= 0) Die();
        }
    }
    private void DetectMent()
    {
        if (Ment.Instance == null) return;

        float distance = Vector3.Distance(transform.position, Ment.Instance.transform.position);
        isTargetDetected = distance <= detectionDistance;

        if (isTargetDetected && canShoot)
        {
            //Debug.Log("DetectMent");
            Shoot();
            StartCoroutine(ShootTime());
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
        transform.position = Vector3.MoveTowards(transform.position,targetPosition,speed * Time.deltaTime);
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
        lives--;
        if (lives == 0)
        {
            Die();
        }
        Debug.Log($"{lives} lives left(Enemy)");
    }
    private IEnumerator ShootTime()
    {
        yield return new WaitForSeconds(0.8f);
        canShoot = true;

    }

}