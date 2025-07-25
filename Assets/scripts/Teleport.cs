using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [SerializeField] private bool isEndLevelTeleport = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Ment player = other.GetComponent<Ment>();

            // ������ ��������� ���������� ��� �������� ����� ��������
            PlayerStats.SaveStats(player);

            if (isEndLevelTeleport && !Ment.Instance.isNacked)
            {
                LoadNextLevel();
            }
            // �������������� ������ ��� ������ ����� ����������
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
            // ���� ��� ��������� �������, ������������ � ���� ��� ������ �������
            SceneManager.LoadScene(0);
            // ����� ���������� ��� ����������� � ����
            PlayerStats.Health = 6;
            PlayerStats.Coins = 0;
            PlayerStats.IsInitialized = false;
        }
    }
}