using UnityEngine;
using UnityEngine.U2D;

public class Entity : MonoBehaviour
{
    protected int lives;
    public virtual void GetDamage()
    {
        lives--;
        if (lives == 0) Die();
    }
    public virtual void Die()
    {
        Debug.Log("Entity died");
        Destroy(this.gameObject);
    }


}
