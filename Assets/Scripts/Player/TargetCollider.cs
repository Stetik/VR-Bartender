using UnityEngine;

public class TargetCollider : MonoBehaviour
{
    public Target target;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            target?.Hit();
        }
    }
}