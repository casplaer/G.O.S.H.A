using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayer;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    public float maxHealth = 100f;
    public float currentHealth;

    private bool isAttacking = false;

    public float moveSpeed = 2f; // —корость передвижени€ врага
    private Transform player;

    private bool wasMovingLeft = false;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform; // ѕредполагаетс€, что у игрока тег "Player"
    }

    private void FixedUpdate()
    {
        if (player != null && !isAttacking)
        {
            ChasePlayer();
        }
        CheckForPlayerInRange();
    }

    public void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(player.position, transform.position);

        if (distance > attackRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            animator.SetBool("isRunning", true);

            bool movingLeft = direction.x < 0;
            if (movingLeft && !wasMovingLeft)
            {
                FlipCharacter(false);
            }
            else if (direction.x > 0 && wasMovingLeft)
            {
                FlipCharacter(true);
            }
            wasMovingLeft = movingLeft;

        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    void FlipCharacter(bool facingLeft)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0f, facingLeft ? 180f : 0f, 0f));
    }

    public void CheckForPlayerInRange()
    {
        if (isAttacking) return;

        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        if (hitPlayers.Length > 0 && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;

        this.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
    }

    // Ётот метод должен быть вызван в конце анимации атаки с использованием анимационного событи€
    void OnAttackEnd()
    {
        isAttacking = false;

        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (var player in hitPlayers)
        {
            player.GetComponent<PlayerController>().TakeDamage(25);
        }
    }
}
