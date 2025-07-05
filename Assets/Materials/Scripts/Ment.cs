using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Ment: Entity
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private int health;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private bool isGrounded = false, isAttacking = false, isRecharged = true, isDead = false, isSwimming = false;

    private Camera mainCamera;
    private Vector2 screenBounds;
    private float objectWidth;
    private Animator anim;

    public Transform attackPos;
    public float attackRange;
    public LayerMask enemy;

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite alive, dead;

    public Transform shotPos;
    public GameObject bullet;

    public static Ment Instance {  get;  set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim= GetComponent<Animator>();
        Instance = this;
        isRecharged = true;
        lives = 6;
        health = lives;
        if (isSwimming)
        {
            rb.gravityScale = 0;
            rb.linearDamping = 5;
        }

    }
    private void Start()
    {
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        objectWidth = sprite.bounds.extents.x;
    }
    private void Update()
    {
        if (isDead) return;
        if (isGrounded && !isAttacking) State = States.idle;
        if (!isAttacking && isRecharged && Input.GetMouseButtonDown(0) && !isSwimming) Attack();
        if (Input.GetMouseButtonDown(1) && !isSwimming) Shoot();
        if (isGrounded && (Input.GetButtonDown("Jump")) && !isSwimming) Jump();
        if (Input.GetButton("Horizontal") && !isSwimming) Run();
        else if ((Input.GetButton("Horizontal")) && isSwimming) SwimX();
        else if ((Input.GetButton("Vertical")) && isSwimming) SwimY();
        if (health > lives) health = lives;
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i<health) hearts[i].sprite = alive;
            else hearts[i].sprite = dead;
            if (i<lives) hearts[i].enabled = true;
            else hearts[i].enabled = false;
        } 
    }
    private void FixedUpdate()
    {
        IsGrounded();
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log(collision.name);
    //    Destroy(collision.gameObject);
    //}

    private void Run()
    {
        if (isGrounded && !isAttacking) State = States.run;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed*Time.deltaTime);
        sprite.flipX = dir.x < 0;
        UpdateShotPosition();
    }
    private void SwimX()
    {
        //if (isGrounded && !isAttacking) State = States.run;
        Vector3 dirx = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dirx, speed*Time.deltaTime);
        sprite.flipX = dirx.x < 0;
    }
    private void SwimY()
    {
        ////if (isGrounded && !isAttacking) State = States.run;
        Vector3 dirx = transform.right * Input.GetAxis("Horizontal");
        Vector3 diry = transform.up * Input.GetAxis("Vertical");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + diry, speed * Time.deltaTime);
        sprite.flipX = dirx.x < 0;
    }
    private void UpdateShotPosition()
    {
        float offsetX = sprite.flipX ? -Mathf.Abs(shotPos.localPosition.x) : Mathf.Abs(shotPos.localPosition.x);
        shotPos.localPosition = new Vector3(offsetX, shotPos.localPosition.y, shotPos.localPosition.z);
    }
    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
    private void IsGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = colliders.Length > 1;
        if (!isGrounded && !isAttacking & !isSwimming) State = States.jump;
    }
    public override void GetDamage()
    {
        if (isDead) return;
        health--;
        if (health == 0)
        {
            foreach (var h in hearts) h.sprite = dead;
            Die();
        }
        Debug.Log($"{health} lives left(Ment)");
    }
    private void Attack()
    {
        if (!isRecharged || isAttacking) return;
        if (isRecharged) 
        {
            State = States.attack;
            isAttacking = true;
            isRecharged = false;
            StartCoroutine(AttackAnimation());
            StartCoroutine(AttackCoolDown());
            Debug.Log("Attack");
        }
    }
    private void Shoot()
    {
        GameObject newBullet = Instantiate(bullet, shotPos.transform.position, transform.rotation);
        float direction = sprite.flipX ? -1f : 1f;
        newBullet.GetComponent<Bullet>().SetDirection(direction);
    }
    public enum States
    {
        idle,
        run,
        jump,
        attack,
        none
    }
    private States State
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }
    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.8f);
        isAttacking = false;
    }
    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(0.3f);
        isRecharged = true;
    }

    private void OnAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);

        for (int i = 0; i < colliders.Length; i++)
            colliders[i].GetComponent<Entity>().GetDamage();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    public override void Die()
    {
        State = States.none;
        anim.SetTrigger("death");
        isDead = true;
    }
}
