using UnityEngine;
using UnityEngine.Video;

public class Video : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject videoScreen;

    void Start()
    {
        videoPlayer.loopPointReached += EndReached; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            videoPlayer.Stop();
            EndReached(videoPlayer);
            Ment.Instance.isCatScene = false;

        }
    }

    void EndReached(VideoPlayer vp)
    {
        videoScreen.SetActive(false);
    }
}