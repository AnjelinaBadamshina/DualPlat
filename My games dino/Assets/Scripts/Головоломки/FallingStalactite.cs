using UnityEngine;

public class FallingStalactiteWithShake : MonoBehaviour
{
    public Collider2D triggerZone; // Назначается в инспекторе (Is Trigger должен быть включён)
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f; // амплитуда дрожания по позиции
    public float shakeRotation = 5f;    // амплитуда дрожания по углу (в градусах)
    public float destroyDelay = 2f;

    public string[] playerTags = { "Player1", "Player2" };

    private Rigidbody2D rb;
    private bool isTriggered = false;
    private bool isFalling = false;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;

        if (triggerZone == null)
        {
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered || isFalling) return;

        foreach (string tag in playerTags)
        {
            if (other.CompareTag(tag))
            {
               
                StartCoroutine(ShakeAndFall());
                isTriggered = true;
                break;
            }
        }
    }

    private System.Collections.IEnumerator ShakeAndFall()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-shakeMagnitude, shakeMagnitude);
            float y = Random.Range(-shakeMagnitude, shakeMagnitude);
            float angle = Random.Range(-shakeRotation, shakeRotation);

            transform.localPosition = initialPosition + new Vector3(x, y, 0f);
            transform.localRotation = Quaternion.Euler(0f, 0f, angle);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Восстановим позицию и ротацию
        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;

        // Начинаем падать
        rb.bodyType = RigidbodyType2D.Dynamic;
        isFalling = true;

       
        // Удалим объект через destroyDelay, но раньше, если столкнёмся
        Destroy(gameObject, destroyDelay);
    }

    // При столкновении — сразу уничтожаем сталактит
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        Destroy(gameObject);
    }
}
