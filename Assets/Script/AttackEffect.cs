using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 0.2f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject, 0.05f);

            Destroy(gameObject, 0.05f);
        }
    }
}