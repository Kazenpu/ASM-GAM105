using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interact"))
        {
            animator.SetTrigger("Open");
            StartCoroutine(Stop());
            Destroy();
        }
    }
    IEnumerator Stop()
    {
        yield return new WaitForSeconds(1f);
        animator.enabled = false;
    }
    void Destroy()
    {
        Destroy(gameObject, 1.5f);
    }
}
