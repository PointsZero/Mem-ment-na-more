using UnityEngine;
using TMPro; // ��������� ������������ ��� ��� Text Mesh Pro
using System.Collections;

public class Dialog : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text TextDialog; // �������� Text �� TMP_Text
    public string[] message;
    public bool StartDialog = false;
    private int currentMessageIndex = 0;

    void Start()
    {
        // ������������� ������� ���������
        message = new string[2];
        message[0] = "-���� ��� 5 ����� ����";
        message[1] = "��� ����? ����������?";
        panel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            panel.SetActive(true);
            currentMessageIndex = 0;
            TextDialog.text = message[currentMessageIndex];
            StartDialog = true;
        }
    }

    void Update()
    {
        if (StartDialog && Input.GetKeyDown(KeyCode.E))
        {
            currentMessageIndex++;
            if (currentMessageIndex < message.Length)
            {
                TextDialog.text = message[currentMessageIndex];
            }
            else
            {
                // ������ ��������
                panel.SetActive(false);
                StartDialog = false;
            }
        }
    }
}