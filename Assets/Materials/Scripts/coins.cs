
using UnityEngine;
using UnityEngine.U2D;

public class coins : MonoBehaviour
{

    public GameObject[] things;

    private void Awake()
    {
        Instantiate(things[0], new Vector3(0, -0.42f, 0), Quaternion.identity);
        Instantiate(things[1], new Vector3(3.97f, 0.81f, 0), Quaternion.identity);
        Instantiate(things[2], new Vector3(6.63f, -0.42f, 0), Quaternion.identity);
        Instantiate(things[3], new Vector3(9.91f, 1.35f, 0), Quaternion.identity);
        Instantiate(things[4], new Vector3(14.51f, -0.42f, 0), Quaternion.identity);
        for (int i = 0; i < things.Length; i++)
        {
            BoxCollider2D collider = things[i].GetComponent<BoxCollider2D>();
            if (collider == null)
            {
                collider = things[i].AddComponent<BoxCollider2D>();
            }
            collider.isTrigger = true;
            collider.size = new Vector2(0.5552368f, 0.6700923f);
            collider.offset = new Vector2(-7.2f, -3.7f);

        }
    }


}
