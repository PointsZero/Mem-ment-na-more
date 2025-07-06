using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dumping = 1.2f;
    public Vector2 offset = new Vector2(3f,4.5f);
    [SerializeField, Range(0f, 1f)] float strengthY = 0.1f;
    public bool isLeft;
    private Transform player;
    private int lastX;

    private void Start()
    {
        offset = new Vector2(Mathf.Abs(offset.x), offset.y);
        FindPlayer(isLeft);
    }

    public void FindPlayer(bool playerIsLeft)
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player").transform;
        lastX = Mathf.RoundToInt(player.position.x);
        if (playerIsLeft)
            transform.position = new Vector3(player.position.x - offset.x, (player.position.y + offset.y) * strengthY, transform.position.z);
        else
            transform.position = new Vector3(player.position.x + offset.x, (player.position.y + offset.y) * strengthY, transform.position.z);
    }

    private void Update()
    {
        if (player)
        {
            int curX = Mathf.RoundToInt(player.position.x);
            if(curX > lastX) isLeft = false;
            else if(curX < lastX) isLeft = true;
            lastX = Mathf.RoundToInt(player.position.x);
            Vector3 target;
            if (isLeft)
                target = new Vector3(player.position.x - offset.x, (player.position.y + offset.y) * strengthY, transform.position.z);
            else
                target = new Vector3(player.position.x + offset.x, (player.position.y + offset.y) * strengthY, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, target, dumping * Time.deltaTime);
        }
    }


}
