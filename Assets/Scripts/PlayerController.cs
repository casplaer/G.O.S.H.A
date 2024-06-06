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
    public LayerMask destructableObjectsLayers;
    public HealthBar healthBar;
    //Attack 
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    public GameObject deathMenu;

    public float maxHealth = 100f;
    public float currentHealth;

    Vector2 movement;
    bool wasMovingLeft = false;
    // Update is called once per frame

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

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

    public void Attack()
    {
        animator.SetTrigger("Attack");
        animator.SetInteger("AttackType", Random.Range(1,4));

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRate, destructableObjectsLayers);

        foreach(var enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(25);
        }

        foreach(var  hitObject in hitObjects)
        {
            hitObject.GetComponent<DesctructableObject>().TakeDamage(50);
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            DeathMenu.isDead = true;

        }
    }
}
