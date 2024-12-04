using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject,2.5f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Ban trung muc tieu");
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
    void Update()
    {
        
    }
}
