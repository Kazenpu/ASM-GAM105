using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELiteEnemy : MonoBehaviour
{
    public float speed = 3f;

    public Transform A;
    public Transform B;
    public Transform C;

    private Vector2 targetPosition;
    private int targetIndex = 0;
    private Transform[] waypoints;
    private Vector2 move;
    private bool facingRight = true;

    private Animator animator;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        waypoints = new Transform[] { A, B, C };
        targetPosition = waypoints[targetIndex].position;
    }

    void Update()
    {
        UpdateAnimation();

        move = (targetPosition - rb.position).normalized;
        rb.linearVelocity = move * speed;

        // Kiểm tra nếu đã đến vị trí mục tiêu
        if (Vector2.Distance(rb.position, targetPosition) < 0.1f)
        {
            // Cập nhật điểm đến tiếp theo theo thứ tự
            targetIndex = (targetIndex + 1) % waypoints.Length;
            targetPosition = waypoints[targetIndex].position;
        }

        // Flip sprite nếu cần
        if (move.x < 0 && facingRight)
        {
            Flip();
        }
        else if (move.x > 0 && !facingRight)
        {
            Flip();
        }
    }
    void UpdateAnimation()
    {
        animator.SetBool("Is Running", move != Vector2.zero);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            // Enemy ngừng di chuyển
            speed = 0f;
            rb.linearVelocity = Vector2.zero;

            // Kích hoạt animation và tự hủy
            animator.SetTrigger("Is Death");
            Destroy(gameObject, 1.1f);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
