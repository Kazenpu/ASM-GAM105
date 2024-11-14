using UnityEngine;

public class Sword : MonoBehaviour
{
    void Start()
    {
        //Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.CompareTag("Enemy"))
        {
            Debug.Log("Chem trung muc tieu");
            Destroy(collision.gameObject);
        }
    }
}
