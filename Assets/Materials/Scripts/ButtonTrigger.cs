using UnityEngine;
using UnityEngine.UI;

public class ButtonTrigger : MonoBehaviour
{
    public float activationRadius = 3f; // Радиус активации
    public GameObject buttonToShow; // Ссылка на кнопку, которую нужно показать/скрыть

    private Transform playerTransform;
    private bool isPlayerInRange = false;

    void Start()
    {
        // Находим игрока по тегу
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure you have an object with 'Player' tag.");
        }

        // Скрываем кнопку при старте
        if (buttonToShow != null)
        {
            buttonToShow.SetActive(false);
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // Проверяем расстояние до игрока
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        bool shouldBeActive = distance <= activationRadius;

        // Если состояние изменилось
        if (shouldBeActive != isPlayerInRange)
        {
            isPlayerInRange = shouldBeActive;

            // Показываем или скрываем кнопку
            if (buttonToShow != null)
            {
                buttonToShow.SetActive(isPlayerInRange);
            }
        }
    }

    // Опционально: визуализация радиуса в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, activationRadius);
    }
}