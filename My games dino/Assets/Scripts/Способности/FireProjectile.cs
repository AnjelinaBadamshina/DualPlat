using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D rb;
    private float direction = 1f;

    public void SetDirection(float dir)
    {
        direction = dir;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = new Vector2(speed * direction, 0);
            Debug.Log("Projectile velocity set to: " + rb.velocity);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2") || other.CompareTag("Crystal"))
            return;

        
        if (other.CompareTag("FireMechanism"))
        {
            ElementalMechanism mech = other.GetComponent<ElementalMechanism>();
            if (mech != null)
                mech.Activate(DamageType.Fire);
        }

        if (other.CompareTag("Enemy"))
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (enemy != null)
                enemy.TakeDamage(1, DamageType.Fire);
        }

        Destroy(gameObject); // Если снаряд сразу уничтожается, убедись, что его траектория и условия для уничтожения правильные
    }
}

