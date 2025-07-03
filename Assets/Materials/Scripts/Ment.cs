using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Ment: Entity
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private int health;
    private Rigidbody2D rb;
    private SpriteRenderer sprite, spriteB;
    private bool isGrounded = false, isAttacking = false, isRecharged = true;
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

    }
    private void Start()
    {
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        objectWidth = sprite.bounds.extents.x;
    }
    private void Update()
    {
        if (isGrounded) State = States.idle;
        if (Input.GetButton("Horizontal")) Run();
        if (isGrounded && Input.GetButtonDown("Jump")) Jump();
        if (Input.GetButtonDown("Fire1")) Attack();
        if (Input.GetMouseButtonDown(1)) Shoot();
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
        //Vector3 viewPos = transform.position;
        //viewPos.x = Mathf.Clamp(viewPos.x, -screenBounds.x + objectWidth, screenBounds.x - objectWidth);
        //transform.position = viewPos;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name + " ment");
        Destroy(collision.gameObject);
    }
    private void Run()
    {
        if (isGrounded) State = States.run;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed*Time.deltaTime);
        sprite.flipX = dir.x < 0;
        UpdateShotPosition();
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
        if (!isGrounded) State = States.jump;
    }
    public override void GetDamage()
    {
        health--;
        if (health == 0)
        {
            foreach (var h in hearts) h.sprite = dead;
            Die();
        }
        Debug.Log($"{lives} lives left(Ment)");
    }
    private void Attack()
    {
        if (isGrounded && isRecharged) 
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
        attack
    }
    private States State
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }
    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
    }
    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        isRecharged = true;
    }
}
