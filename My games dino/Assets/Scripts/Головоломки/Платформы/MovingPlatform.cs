using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3[] positions; // Точки, между которыми движется платформа
    public float speed = 2f;   // Скорость движения платформы

    private int currentIndex = 0;
    private int direction = 1;  // 1 = вперёд, -1 = назад
    private Rigidbody2D rb;    // Ссылка на Rigidbody2D

    void Start()
    {
        // Получаем Rigidbody2D и настраиваем как Kinematic
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.useFullKinematicContacts = true;
        }
        else
        {
            Debug.LogWarning("Rigidbody2D not found on MovingPlatform!");
        }
    }

    void FixedUpdate()
    {
        if (positions.Length == 0 || Time.timeScale == 0) return;

        Vector3 target = positions[currentIndex];
        rb.MovePosition(Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime));

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentIndex += direction;
            if (currentIndex >= positions.Length || currentIndex < 0)
            {
                direction *= -1;
                currentIndex += direction * 2;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            ContactPoint2D contact = collision.GetContact(0);
            Debug.Log($"Collision with {collision.gameObject.name}, Normal: {contact.normal}, Contact Point: {contact.point}, Platform Pos: {transform.position}");
            Debug.DrawRay(contact.point, contact.normal * 2, Color.red, 2f); // Визуальная отладка
            if (true) // Временно игнорируем нормаль для теста
            {
                Debug.Log($"{collision.gameObject.name} is now parented to {gameObject.name}");
                collision.transform.SetParent(transform);
            }
            else
            {
                Debug.Log($"Not parented: Normal.y {contact.normal.y} is too low");
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            Debug.Log($"{collision.gameObject.name} is no longer parented");
            collision.transform.SetParent(null);
        }
    }
}