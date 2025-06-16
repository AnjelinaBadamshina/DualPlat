using UnityEngine;

public class HeroTreatment2p : MonoBehaviour
{
    public int collisionHeal = 1; // Количество единиц здоровья, добавляемых при столкновении
    public string tagPlayer1;
    public string tagPlayer2; // Тег объекта, с которым взаимодействует этот объект лечения

    public AudioSource Audio;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, вошел ли объект в триггер, имеющий указанный тег
        if (other.CompareTag(tagPlayer1) || other.CompareTag(tagPlayer2))
        {
            // Получаем компонент здоровья персонажа
            HealthHero2p health = other.GetComponent<HealthHero2p>();
            if (health != null)
            {
                Audio.Play();
                // Вызываем метод лечения у компонента здоровья с передачей количества лечения
                health.SetHealth(collisionHeal);
            }

            // Уничтожаем объект лечения после использования
            Destroy(gameObject);
        }
    }
}
