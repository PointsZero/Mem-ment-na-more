using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
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
                enemy.GetDamage();
                Debug.Log("shot hit in enemy");
                Destroy(gameObject);
            }
        }
        else if (collision.CompareTag("Player"))
        {
            Debug.Log("shot hit in ment");
            Destroy(gameObject);
            Ment.Instance.GetDamage();
        }
    }

}
