using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject menu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(!menu.activeSelf);

            Time.timeScale = menu.activeSelf ? 0f : 1f;
        }
    }

    public void ResumeGame()
    {
        menu.SetActive(false);
        Time.timeScale = 1f;
    }
}