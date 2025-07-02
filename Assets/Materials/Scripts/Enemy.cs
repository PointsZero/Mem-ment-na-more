//using TMPro;
//using UnityEngine;

//public class Enemy : Entity
//{
//    [SerializeField] private float speed = 3f;
//    [SerializeField] private Transform pointA;
//    [SerializeField] private Transform pointB;
//    private Vector3 dir, targetPosition;
//    private SpriteRenderer sprite;
//    private bool movingToB = true;
//    private void Start()
//    {
//        dir = transform.right;
//        lives = 5;
//        targetPosition = pointB.position;
//        if (pointA == null || pointB == null)
//        {
//            Debug.LogError("Не назначены точки пути!");
//            enabled = false;
//        }
//    }
//    private void Update()
//    {
//        Move();
//    }
//    private void OnCollisionEnter2D(Collision2D collision)
//    {

//        if (collision.gameObject == Ment.Instance.gameObject)
//        {
//            Ment.Instance.GetDamage();
//            Debug.Log($"{lives} lives left(Enemy)");
//            lives--;
//            if (lives == 0) Die();

//        }
//    }
//    private void Move()
//    {
//        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

//        // Движение к текущей цели
//        transform.position = Vector3.MoveTowards(
//            transform.position,
//            targetPosition,
//            speed * Time.deltaTime
//        );
//        sprite.flipX = dir.x < 0;
//        // Проверка достижения точки
//        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
//        {
//            // Меняем цель
//            targetPosition = movingToB ? pointA.position : pointB.position;
//            movingToB = !movingToB;
//        }
//    }
//}
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private SpriteRenderer sprite; // Добавлено SerializeField

    private Vector3 targetPosition;
    private bool movingToB = true;
    private float arrivalThreshold = 0.1f;

    private void Start()
    {
        lives = 5;

        sprite = GetComponentInChildren<SpriteRenderer>();
        if (pointA == null || pointB == null || sprite == null)
        {
            Debug.LogError("Не назначены обязательные параметры!");
            enabled = false;
            return;
        }

        targetPosition = pointB.position;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        // Движение к текущей цели
        transform.position = Vector3.MoveTowards(transform.position,targetPosition,speed * Time.deltaTime);

        // Обновляем направление взгляда
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        if (moveDirection != Vector3.zero)
        {
            sprite.flipX = moveDirection.x < 0;
        }

        // Проверка достижения точки
        if (Vector3.Distance(transform.position, targetPosition) < arrivalThreshold)
        {
            movingToB = !movingToB;
            targetPosition = movingToB ? pointB.position : pointA.position;
        }
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

}