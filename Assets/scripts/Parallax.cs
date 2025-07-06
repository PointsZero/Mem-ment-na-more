using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] Transform followingTarget;
    [SerializeField, Range(0f, 1f)] float strengthX = 0.1f;
    [SerializeField, Range(0f, 1f)] float strengthY = 0.1f;
    [SerializeField] bool disableVerticalPar;
    Vector3 targetPrevPos;

    void Start()
    {
        if (!followingTarget)
            followingTarget = Camera.main.transform;
        targetPrevPos = followingTarget.position;
    }

    void Update()
    {
        var delta = followingTarget.position - targetPrevPos;
        if (disableVerticalPar)
            delta.y = 0;
        targetPrevPos = followingTarget.position;
        transform.position += new Vector3(delta.x * strengthX, delta.y * strengthY, delta.z);
    }
}
