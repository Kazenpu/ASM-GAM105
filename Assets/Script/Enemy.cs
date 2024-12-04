using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;

    //public float speed = 3f;
    public float moveInterval = 2f;
    private Vector2 moveRange = new Vector2(5f, 5f);
    private Vector2 direction;

    private Vector2 targetPosition;
    private Rigidbody2D rb;
    private Animator animator;

    private Transform player;
    private bool facingRight = true;

    private float originalSpeed;
    public float detectionRange = 4f;

    public GameObject enemyPrefabAttack;
    private bool isAttacking = false;
    private Coroutine attackCoroutine;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        SetRandomTargetPosition();
        InvokeRepeating("SetRandomTargetPosition", moveInterval, moveInterval);

        originalSpeed = enemyData.moveSpeed;
    }

    void Update()
    {
        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float distanceToPlayer = Vector2.Distance(player.position, rb.position);

        if (distanceToPlayer <= detectionRange)
        {
            enemyData.moveSpeed = originalSpeed * 1.5f;
            direction = ((Vector2)player.position - rb.position).normalized;
            rb.linearVelocity = direction * enemyData.moveSpeed;
        }
        else
        {
            enemyData.moveSpeed = originalSpeed;
            direction = (targetPosition - rb.position).normalized;

            if (Vector2.Distance(rb.position, targetPosition) < 0.1f)
            {
                SetRandomTargetPosition();
            }
            rb.linearVelocity = direction * enemyData.moveSpeed;
        }

        UpdateAnimation();

        if (!isAttacking)
        {
            if (direction.x < 0 && facingRight)
            {
                Flip();
            }
            else if (direction.x > 0 && !facingRight)
            {
                Flip();
            }
        }
    }

    void UpdateAnimation()
    {
        animator.SetBool("Is Running", !isAttacking && direction != Vector2.zero);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {   
        if (collision.CompareTag("Weapon"))
        {
            rb.linearVelocity = Vector2.zero;

            animator.SetBool("Is Running", false);
            animator.SetTrigger("Is Death");

            SelfDestroy();
        }
        else if (collision.CompareTag("Player"))
        {
            if (!isAttacking)
            {
                isAttacking = true;
                animator.SetBool("Is Running", false);

                attackCoroutine = StartCoroutine(ContinuousAttack());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isAttacking)
        {
            isAttacking = false;

            // Dừng tấn công liên tục
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }

    IEnumerator ContinuousAttack()
    {
        while (isAttacking)
        {
            animator.SetTrigger("Is Attacking");

            yield return new WaitForSeconds(1f);

            Vector3 spawnEnemyAttack = transform.position;
            GameObject createdEnemyAttack = Instantiate(enemyPrefabAttack, spawnEnemyAttack, Quaternion.identity);
            Destroy(createdEnemyAttack, 0.1f);
        }
    }
    void SetRandomTargetPosition()
    {
        float randomX = Random.Range(-moveRange.x / 2, moveRange.x / 2) + transform.position.x;
        float randomY = Random.Range(-moveRange.y / 2, moveRange.y / 2) + transform.position.y;
        targetPosition = new Vector2(randomX, randomY);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    public void SelfDestroy()
    {
        Destroy(gameObject,1.5f);
    }
}
