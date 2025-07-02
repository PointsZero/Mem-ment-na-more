using UnityEngine;

public class Hedgehog : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Ment.Instance.gameObject)
        {
            Ment.Instance.GetDamage();
        }
    }
}
