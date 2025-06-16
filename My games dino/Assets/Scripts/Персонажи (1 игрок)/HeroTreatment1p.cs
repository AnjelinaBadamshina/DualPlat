using UnityEngine;

public class HeroTreatment1p : MonoBehaviour
{
    public int collisionHeal = 1; // Количество единиц здоровья, добавляемых при столкновении
    public string tagPlayer1 = "Player"; // По умолчанию тэг игрока - Player

    public AudioSource Audio;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, вошел ли объект в триггер, имеющий указанный тег
        if (other.CompareTag(tagPlayer1))
        {
            // Получаем компонент здоровья персонажа
            HealthHero1p health = other.GetComponent<HealthHero1p>();
            if (health != null)
            {
                if (Audio != null) Audio.Play();
                // Вызываем метод лечения у компонента здоровья с передачей количества лечения
                health.SetHealth(collisionHeal);

                // Уничтожаем объект лечения после использования
                Destroy(gameObject);
            }
        }
    }
}