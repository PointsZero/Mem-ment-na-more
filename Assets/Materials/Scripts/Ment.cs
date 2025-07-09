using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ment : Entity
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private bool isNacked = false;
    private int health;
    private float dropBackForce = 10f;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private bool isGrounded = false, isAttacking = false, isRecharged = true, canShoot = true, isDead = false, isDamaged = false;
    [SerializeField] private bool isSwimming = false;
    private int countCoins = 0;
    private Camera mainCamera;
    private Vector2 screenBounds;
    private float objectWidth;
    private Animator anim;
    [SerializeField] private TMP_Text coinsText;
    public GameObject DeathScreen;
    public Transform attackPos;
    public float attackRange;
    public LayerMask enemy;

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite alive, dead;

    public Transform shotPos;
    public GameObject bullet;
    public int GetHealth() => health;
    public int GetCoins() => countCoins;
    //[SerializeField] private TMP_Text displayText;

    public static Ment Instance { get; set; }
    public enum States
    {
        idle,
        run,
        jump,
        attack,
        damage,
        none
    }

    private States State
    {
        get => (States)anim.GetInteger("state");
        set => anim.SetInteger("state", (int)value);
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Instance = this;
        isRecharged = true;
        anim.SetBool("swim", isSwimming);
        anim.SetBool("nacked", isNacked);
        if (PlayerStats.IsInitialized)
        {
            lives = 6; 
            health = PlayerStats.Health;
            countCoins = PlayerStats.Coins;

        }
        else
        {
            lives = 6;
            health = lives;
        }

        if (isSwimming)
        {
            rb.gravityScale = 0;
            rb.linearDamping = 5;
        }
        UpdateCoinsText();
    }
    private void Start()
    {
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        objectWidth = sprite.bounds.extents.x;
        UpdateCoinsText();
    }
    private void Update()
    {
        if (isDead) return;
        if (isGrounded && !isAttacking && !isDamaged) State = States.idle;
        //if (!isAttacking && isRecharged && Input.GetMouseButtonDown(0) && !isSwimming) Attack();
        if (Input.GetMouseButtonDown(0) && !isSwimming && canShoot && !isNacked)
        {
            Shoot();
            StartCoroutine(ShootTime());
        }
        if (isGrounded && (Input.GetButtonDown("Jump")) && !isSwimming && !isDamaged) Jump();
        if (Input.GetButton("Horizontal") && !isSwimming && !isDamaged) Run();
        else if ((Input.GetButton("Horizontal")) && isSwimming) SwimX();
        else if ((Input.GetButton("Vertical")) && isSwimming) SwimY();
        if (health > lives) health = lives;
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health) hearts[i].sprite = alive;
            else hearts[i].sprite = dead;
            if (i < lives) hearts[i].enabled = true;
            else hearts[i].enabled = false;
        }
        //displayText.text = $"Coins: {countCoins}";
    }
    private void FixedUpdate()
    {
        IsGrounded();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            countCoins++;
            UpdateCoinsText(); 
            Debug.Log($"countCoins = {countCoins}");
        }
        else if (collision.gameObject.CompareTag("Heal"))
        {
            if (health < 6) health++;
            Debug.Log($"lives ment = {health}");
        }
        Debug.Log(collision.name);
        Destroy(collision.gameObject);
    }
    private void UpdateCoinsText()
    {
        if (coinsText != null)
        {
            coinsText.text = $"{countCoins}";
            coinsText.gameObject.SetActive(true);
        }
    }
    private void Run()
    {
        if (isGrounded && !isAttacking) State = States.run;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        sprite.flipX = dir.x < 0;
        UpdateShotPosition();
    }
    private void SwimX()
    {
        Vector3 dirx = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dirx, speed * Time.deltaTime);
        sprite.flipX = dirx.x < 0;
    }
    private void SwimY()
    {
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
        if (!isGrounded && !isAttacking && !isSwimming && !isDead) State = States.jump;
    }
    public override void GetDamage()
    {
        if (isDead) return;

        health--;
        if (health == 0)
        {
            if (!DeathScreen.activeSelf)
            {
                DeathScreen.SetActive(true);
            }

            foreach (var h in hearts) h.sprite = dead;
            Die();
        }
        else
        {
            State = States.damage;
            isDamaged = true;
            StartCoroutine(DamageCoolDown());
        }
        Debug.Log($"{health} lives left(Ment)");
    }

    //private void Attack()
    //{
    //    if (!isRecharged || isAttacking) return;
    //    if (isRecharged)
    //    {
    //        State = States.attack;
    //        isAttacking = true;
    //        isRecharged = false;
    //        StartCoroutine(AttackAnimation());
    //        StartCoroutine(AttackCoolDown());
    //        Debug.Log("Attack");
    //    }
    //}

    private void Shoot()
    {
        State = States.none;
        anim.SetTrigger("shoot");
        isAttacking = true;
        StartCoroutine(AttackAnimation());
        Debug.Log("Shoot");
        GameObject newBullet = Instantiate(bullet, shotPos.transform.position, transform.rotation);
        float direction = sprite.flipX ? -1f : 1f;
        newBullet.GetComponent<Bullet>().SetDirection(direction);
        canShoot = false;
    }

    public void DropBack(Vector2 direction)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(direction.normalized * dropBackForce, ForceMode2D.Impulse);
        }
    }
    public override void Die()
    {
        State = States.none;
        anim.SetTrigger("death");
        isDead = true;
    }
   
    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.2f);
        isAttacking = false;
    }
    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(0.3f);
        isRecharged = true;
        Debug.Log("Recharged");
    }

    private IEnumerator DamageCoolDown()
    {
        yield return new WaitForSeconds(0.8f);
        isDamaged = false;
    }
    private IEnumerator ShootTime()
    {
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
    }

    //private void OnAttack()
    //{
    //    Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);

    //    for (int i = 0; i < colliders.Length; i++)
    //        colliders[i].GetComponent<Entity>().GetDamage();
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(attackPos.position, attackRange);
    //}



}
