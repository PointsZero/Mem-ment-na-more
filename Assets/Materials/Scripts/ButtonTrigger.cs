using UnityEngine;
using UnityEngine.UI;

public class ButtonTrigger : MonoBehaviour
{
    public float activationRadius = 3f; // ������ ���������
    public GameObject buttonToShow; // ������ �� ������, ������� ����� ��������/������

    private Transform playerTransform;
    private bool isPlayerInRange = false;

    void Start()
    {
        // ������� ������ �� ����
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure you have an object with 'Player' tag.");
        }

        // �������� ������ ��� ������
        if (buttonToShow != null)
        {
            buttonToShow.SetActive(false);
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // ��������� ���������� �� ������
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        bool shouldBeActive = distance <= activationRadius;

        // ���� ��������� ����������
        if (shouldBeActive != isPlayerInRange)
        {
            isPlayerInRange = shouldBeActive;

            // ���������� ��� �������� ������
            if (buttonToShow != null)
            {
                buttonToShow.SetActive(isPlayerInRange);
            }
        }
    }

    // �����������: ������������ ������� � ���������
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, activationRadius);
    }
}