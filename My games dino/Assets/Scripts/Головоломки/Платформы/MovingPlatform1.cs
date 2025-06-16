using UnityEngine;

public class MovingPlatform1 : MonoBehaviour
{
    // Вызывается, когда коллайдер персонажа входит в коллайдер платформы
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверяем, является ли объект Player1 или Player2
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            // Проверяем нормаль контакта, чтобы персонаж был сверху платформы
            ContactPoint2D contact = collision.GetContact(0);
            if (contact.normal.y > 0.5f) // Нормаль вверх (персонаж стоит на платформе)
            {
                // Устанавливаем платформу как родителя для персонажа
                collision.transform.SetParent(transform);
            }
        }
    }

    // Вызывается, когда коллайдер персонажа покидает коллайдер платформы
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Проверяем, является ли объект Player1 или Player2
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            // Снимаем родительство, чтобы персонаж больше не двигался с платформой
            collision.transform.SetParent(null);
        }
    }
}