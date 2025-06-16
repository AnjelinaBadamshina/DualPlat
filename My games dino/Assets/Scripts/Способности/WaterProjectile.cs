using UnityEngine;

public class WaterProjectile : MonoBehaviour
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
            Debug.Log("WaterProjectile velocity: " + rb.velocity);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2") || other.CompareTag("Crystal"))
            return;

        else if (other.CompareTag("Fire"))
        {
            FireSurface fire = other.GetComponent<FireSurface>();
            if (fire != null)
                fire.Extinguish();
        }


        if (other.CompareTag("WaterMechanism"))
        {
            ElementalMechanism mech = other.GetComponent<ElementalMechanism>();
            if (mech != null)
                mech.Activate(DamageType.Water);
        }


        if (other.CompareTag("Enemy"))
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (enemy != null)
                enemy.TakeDamage(1, DamageType.Water);
        }

        Destroy(gameObject);
    }
}
