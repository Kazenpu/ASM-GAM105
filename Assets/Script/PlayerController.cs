using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using Unity.Entities;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    public GameObject sword;
    public Transform swordPosition;
    public Transform arrowPosition;

    public float delay;
    public int numberArrows;
    public GameObject prefabArrow;
    public float speedArrow;

    private Rigidbody2D rb;
    private Vector2 move;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sword.SetActive(false);
    }
    void Update()
    {
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");

        move.Normalize();

        if (move.x > 0)
        {
            Flip(false);
        }
        else if (move.x < 0)
        {
            Flip(true);
        }
        UpdateAnimation();

        if (Input.GetKeyDown(KeyCode.J))
        {
            animator.SetTrigger("Swing 1");
            ActiveSword();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Swing 2");
            ActiveSword();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(PrefabArrows()); //Coroutine de su dung WaitForSeconds
        }
    }
    void FixedUpdate() //Ham xu ly va cham vat ly
    {
        rb.linearVelocity = move * speed;
        sword.transform.position = swordPosition.position;
    }
    void Flip(bool facingLeft)
    {
        Vector2 newScale = transform.localScale;
        newScale.x = facingLeft ? -1 : 1; //toan tu 3 ngoi
        transform.localScale = newScale;
    }
    void UpdateAnimation()
    {
        animator.SetBool("Is Running", move != Vector2.zero);
    }
    public void ActiveSword()
    {
        sword.SetActive(true);
        Invoke("DeactiveSword", 0.5f);
    }
    public void DeactiveSword()
    {
        sword.SetActive(false);
    }
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        // Kiểm tra va chạm với Border
        if (collision.gameObject.CompareTag("Border"))
        {
            rb.linearVelocity = Vector2.zero;//Dung chuyen dong
            Debug.Log("Cham den gioi han map");
        }
    }
    IEnumerator PrefabArrows()
    {
        Vector3 direction = transform.localScale.x > 0 ? transform.right : -transform.right;
        // Xet theo truc voi transform.right
        // Toan tu 3 ngoi voi transform.localScale.x > 0 voi transform.localScale.x la float chu khong phai bool

        for (int i = 0; i < numberArrows; i++)
        {
            Vector3 spawnPos = arrowPosition.position + direction * 1 * i;
            GameObject createdArrow = Instantiate(prefabArrow, spawnPos, Quaternion.identity);

            if (transform.localScale.x > 0) //Nhan vat quay sang phai
            {
                createdArrow.transform.localScale = new Vector3(1, 1, 1);
            }
            else //Nhan vat quay sang trai >>> Flip
            {
                createdArrow.transform.localScale = new Vector3(-1, 1, 1);
            }

            Rigidbody2D rbArrow = createdArrow.GetComponent<Rigidbody2D>();
            if (rbArrow != null)
            {
                rbArrow.linearVelocity = direction * speedArrow;
            }
            yield return new WaitForSeconds(delay); //gia tri tra ve cua IEnumerator
        }
    }
}
