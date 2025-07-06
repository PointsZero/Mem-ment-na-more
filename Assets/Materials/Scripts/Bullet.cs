using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]  public float speed = 10f;
    private Rigidbody2D rb;
    private float direction = 1f;
    private float maxDistance = 10f;
    private Vector2 startPosition;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        rb.linearVelocity = transform.right * speed * direction;

        if (direction < 0)
        {
            SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
            GetComponent<SpriteRenderer>().sortingLayerName = "Bullet";
            GetComponent<SpriteRenderer>().sortingOrder = 1;
            if (sprite != null)
            {
                sprite.flipX = true;
            }
        }
    }
    void Update()
    {
        ShootRange();
    }
    public void SetDirection(float newDirection)
    {
        direction = newDirection;
    }
    public void ShootRange()
    {
        if (Mathf.Abs(transform.position.x - startPosition.x) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy != null)
            {
                Vector3 hitDirection = enemy.transform.position - transform.position;
                enemy.GetDamage();
                enemy.DropBack(hitDirection);
                Destroy(gameObject);

            }
        }
        else if (collision.CompareTag("Player"))
        {
            Vector3 hitDirection = Ment.Instance.transform.position - transform.position;
            Ment.Instance.GetDamage();
            Ment.Instance.DropBack(hitDirection);
            Destroy(gameObject);

        }
        else if (collision.CompareTag("Block_up"))
        {
            Destroy(gameObject);
        }
    }
}
