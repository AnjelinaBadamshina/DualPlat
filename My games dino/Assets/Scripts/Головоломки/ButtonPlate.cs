using UnityEngine;

public class ButtonPlate : MonoBehaviour
{
    private int pressed = 0; // Количество столкновений с игроками или ящиками. Нужно, чтобы корректно обрабатывалась ситуация, когда два игрока одновременно встали на кнопку
    public bool Pressed => pressed > 0; // Свойство, которое преобразует внутреннюю информацию во внешнюю - нажата кнопка или нет

    [SerializeField] private Sprite releasedImage;
    [SerializeField] private Sprite pressedImage;

    private SpriteRenderer img;

    private void Awake()
    {
        img = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision) // Триггер, чтобы на кнопку не приходилось каждый раз запрыгивать сверху
    {
        if (collision.GetComponent<MovementHero2p>() == null && !collision.gameObject.name.Contains("Ящик")) // Если столкновение не с динозавром или ящиком, то выходим из функции
            return;
        
        img.sprite = pressedImage; // Меняем спрайт кнопки
        pressed++; // Увеличиваем количество столкновений
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<MovementHero2p>() == null && !collision.gameObject.name.Contains("Ящик"))
            return;

        img.sprite = releasedImage;
        pressed--;

    }
}