using UnityEngine;

public class Medusa : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] public SpriteRenderer sprite;

    private Animator anim;

    private bool movingToB = true;
    private float arrivalThreshold = 0.1f;
    private Vector3 targetPosition;

    void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        targetPosition = pointB.position;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Ment.Instance.gameObject)
        {
            Ment.Instance.GetDamage();
        }
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        if (moveDirection != Vector3.zero)
        {
            sprite.flipY = moveDirection.y < 0;
        }
        if (Vector3.Distance(transform.position, targetPosition) < arrivalThreshold)
        {
            movingToB = !movingToB;
            targetPosition = movingToB ? pointB.position : pointA.position;
        }
    }
}
