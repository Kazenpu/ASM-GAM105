using UnityEngine;

public class NPC : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    public GameObject chatbox;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        chatbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Interact"))
        {
            Debug.Log("Nhan vat tuong tac");
            chatbox.SetActive(true);
            Invoke("DeactiveChatBox", 3f);
        }
    }
    void DeactiveChatBox()
    {
        chatbox.SetActive(false);
    }
}
