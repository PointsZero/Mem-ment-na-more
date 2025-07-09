using UnityEngine;
using TMPro; // Добавляем пространство имён для Text Mesh Pro
using System.Collections;

public class Dialog : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text TextDialog; // Заменяем Text на TMP_Text
    public string[] message;
    public bool StartDialog = false;
    private int currentMessageIndex = 0;

    void Start()
    {
        // Инициализация массива сообщений
        message = new string[2];
        message[0] = "-Угх… Еще 5 минут… Стоп";
        message[1] = "Это… море? Московское?";
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
                // Диалог закончен
                panel.SetActive(false);
                StartDialog = false;
            }
        }
    }
}