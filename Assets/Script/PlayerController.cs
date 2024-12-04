using NUnit.Framework.Internal.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 move;
    private Animator animator;

    public GameObject gameOver;

    public Slider healthSlider;
    public int characterHealth;

    public GameObject sword;
    public Transform swordPosition;
    public Transform arrowPosition;

    public GameObject interact;

    public float delay;
    public int numberArrows;
    public GameObject prefabArrow;
    public float speedArrow;

    private Vector2 targetPosition;
    public int numberUltimate;
    public GameObject prefabUltimate;

    public AudioClip attackSound1;
    public AudioClip attackSound2;
    public AudioClip arrowSound;
    public AudioClip pressSound;
    private AudioSource audioSource;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();

        sword.SetActive(false);
        interact.SetActive(false);
        gameOver.SetActive(false);
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
            audioSource.PlayOneShot(attackSound1);
            ActiveSword();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Swing 2");
            audioSource.PlayOneShot(attackSound2);
            ActiveSword();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(PrefabArrows()); //Coroutine de su dung WaitForSeconds
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine (Ultimate());
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            audioSource.PlayOneShot(pressSound);
            ActiveInteract();
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
    public void ActiveInteract()
    {
        interact.SetActive(true);
        Invoke("DeactiveInteract", 0.5f);
    }
    public void DeactiveInteract()
    {
        interact.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyAttack"))
        {   

            animator.SetTrigger("Getting hit");

            characterHealth -= 1;
            healthSlider.value = characterHealth;

            if (characterHealth <= 0)
            {
                animator.SetTrigger("Is Dead");
                gameOver.SetActive(true);
            }
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

            audioSource.PlayOneShot(arrowSound);

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
    IEnumerator Ultimate()
    {
        for (int i = 0; i < numberUltimate; i++)
        {
            float randomX = UnityEngine.Random.Range(0f, 1f);
            Vector3 randomPos = Camera.main.ViewportToWorldPoint(new Vector3(randomX, 1f, 0)); // Trên màn hình, cao hơn một chút
            randomPos.z = 0; // Đặt z về 0 để đảm bảo nó ở mặt phẳng 2D

            Quaternion rotation = Quaternion.Euler(0, 0, -90);

            GameObject createdUltimate = Instantiate(prefabUltimate, randomPos, rotation);

            Rigidbody2D rbUltimate = createdUltimate.GetComponent<Rigidbody2D>();
            if (rbUltimate == null)
            {
                rbUltimate = createdUltimate.AddComponent<Rigidbody2D>();
            }
            yield return new WaitForSeconds(0.5f);
        }

        
    }
}
