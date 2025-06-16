using UnityEngine;

public class MovementHero2p : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool FacingRight = true; // Переменная для отслеживания направления персонажа
    private float lastMoveInput = 0f; // Запоминаем последнее ненулевое значение moveInput

    [Header("Настройки перемещения игрока")]
    [Range(0, 10f)] public float speed = 1f; // Скорость перемещения персонажа
    [Range(0, 15f)] public float jumpForce = 8f; // Сила прыжка персонажа
    public float flipShift;

    [Header("Настройки анимации игрока")]
    [SerializeField] private Animator animator; // Ссылка на компонент аниматора персонажа

    [Space]
    [Header("Настройки заземления игрока")]
    public bool isGrounded = false; // Переменная для определения, находится ли персонаж на земле
    public float checkGroundOffsetY = -0.7f; // Смещение точки проверки заземления по оси Y
    public float checkGroundRadius = 0.3f; // Радиус проверки заземления

    public AudioSource Audio;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Получаем доступ к Rigidbody2D компоненту персонажа
    }

    void Update()
    {
        float moveInput = 0f; // Инициализируем переменную для ввода перемещения

        // Проверяем, какой игрок управляет персонажем и получаем ввод перемещения
        if (gameObject.CompareTag("Player1"))
        {
            moveInput = Input.GetAxisRaw("Horizontal"); // Получаем ввод перемещения для первого игрока
            if (Input.GetKeyDown(KeyCode.W) && isGrounded) // Проверяем, была ли нажата кнопка прыжка и находится ли персонаж на земле
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Применяем силу прыжка к персонажу
                                                                     // Воспроизводим звук
                Audio.Play();

            }
        }
        else if (gameObject.CompareTag("Player2"))
        {
            moveInput = Input.GetAxisRaw("Horizontal2"); // Получаем ввод перемещения для второго игрока
            if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded) // Проверяем, была ли нажата кнопка прыжка и находится ли персонаж на земле
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Применяем силу прыжка к персонажу

                Audio.Play();
            }
        }

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y); // Применяем ввод перемещения к скорости персонажа

        animator.SetFloat("HorizontalMove", Mathf.Abs(moveInput)); // Устанавливаем параметр анимации для перемещения

        // Проверяем направление перемещения и поворачиваем персонажа соответственно
        if (moveInput != 0) // Запоминаем только ненулевые значения
        {
            lastMoveInput = moveInput;
        }

        if (lastMoveInput > 0 && !FacingRight)
        {
            Flip(); // Поворачиваем персонажа вправо
        }
        else if (lastMoveInput < 0 && FacingRight)
        {
            Flip(); // Поворачиваем персонажа влево
        }
    }

    // Метод для поворота персонажа
    public void Flip()
    {
        FacingRight = !FacingRight; // Меняем направление персонажа
        Vector3 theScale = transform.localScale; // Получаем масштаб персонажа
        theScale.x *= -1; // Инвертируем масштаб по оси X

        // Перемещаем персонажа в сторону поворота, так как коллайдеры ассиметричные
        if (FacingRight)
            transform.position += flipShift * Vector3.right;
        else
            transform.position += flipShift * Vector3.left;

        transform.localScale = theScale; // Применяем измененный масштаб
    }



    // Метод, вызываемый на каждом кадре физики для проверки заземления
    private void FixedUpdate()
    {
        CheckGround(); // Вызываем метод проверки заземления
    }

    // Метод для проверки заземления
    private void CheckGround()
    {
        // Проверяем, есть ли коллайдеры в указанной области
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y + checkGroundOffsetY), checkGroundRadius);

        if (Physics2D.OverlapPointAll(new Vector2(transform.position.x, transform.position.y + checkGroundOffsetY), LayerMask.GetMask("Dinosaur")).Length == 1 // Проверяем, что на уровне ног только один динозавр (мы) - не будет срабатывать, когда двое просто проходят сквозь друг друга
            && Physics2D.OverlapPointAll(new Vector2(transform.position.x, transform.position.y + checkGroundOffsetY - 0.2f), LayerMask.GetMask("Dinosaur")).Length > 0) // Проверяем, что ниже динозавра есть еще динозавр - мы стоим сверху.
        {
            rb.includeLayers = LayerMask.GetMask("Dinosaur"); // В RigidBody2D указываем, что игнорируя настройки физики, будем сталкиваться со слоем Dinosaur
        }
        else
        {
            rb.includeLayers = LayerMask.GetMask("Nothing"); // Убираем слой "Dinosaur" из исключений.
        }

        // Если количество коллайдеров больше 1, то персонаж находится на земле
        if (colliders.Length > 1)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false; // Иначе персонаж в воздухе
        }
    }

    // Отрисовка для метода CheckGround()
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * checkGroundOffsetY);
        Gizmos.DrawWireSphere(transform.position + Vector3.up * checkGroundOffsetY, checkGroundRadius);
    }
}
