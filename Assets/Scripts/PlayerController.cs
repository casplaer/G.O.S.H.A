using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    //Attack 
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    Vector2 movement;
    bool wasMovingLeft = false;
    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
                nextAttackTime = Time.time + 1f/attackRate;
            }
        }

        movement.y = Input.GetAxisRaw("Vertical");
        movement.x = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("Speed", movement.sqrMagnitude);

        bool flipped = movement.x < 0;
        
        if(flipped && wasMovingLeft)
        {
            FlipCharacter(true);
        }
        else if(movement.x > 0) FlipCharacter(false);

        wasMovingLeft = flipped;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void FlipCharacter(bool facingLeft)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0f, facingLeft ? 180f : 0f, 0f));
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        animator.SetInteger("AttackType", Random.Range(1,4));

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(var enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(25);
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
