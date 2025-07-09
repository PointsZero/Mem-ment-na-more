using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [SerializeField] private bool savePosition = false;
    [SerializeField] private bool isEndLevelTeleport = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Ment player = other.GetComponent<Ment>();

            // Всегда сохраняем статистику при переходе между уровнями
            PlayerStats.SaveStats(player);

            if (isEndLevelTeleport)
            {
                LoadNextLevel();
            }
            // Дополнительная логика для других типов телепортов
        }
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // Если это последний уровень, возвращаемся в меню или первый уровень
            SceneManager.LoadScene(0);
            // Сброс статистики при возвращении в меню
            PlayerStats.Health = 6;
            PlayerStats.Coins = 0;
            PlayerStats.IsInitialized = false;
        }
    }
}