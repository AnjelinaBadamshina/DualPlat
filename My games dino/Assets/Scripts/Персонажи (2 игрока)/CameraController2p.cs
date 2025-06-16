using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Атрибут, который требует наличия компонента Camera
[RequireComponent(typeof(Camera))]
public class CameraController2p : MonoBehaviour
{
    [Header("Ссылки на CharacterSwitcher")]
    [SerializeField] public CharacterSwitcher Player1Switcher; // Ссылка на CharacterSwitcher первого игрока
    [SerializeField] public CharacterSwitcher Player2Switcher; // Ссылка на CharacterSwitcher второго игрока

    private Transform Player1 { get { return Player1Switcher != null && Player1Switcher.CurrentCharacter != null ? Player1Switcher.CurrentCharacter.transform : null; } }
    private Transform Player2 { get { return Player2Switcher != null && Player2Switcher.CurrentCharacter != null ? Player2Switcher.CurrentCharacter.transform : null; } }

    private Camera camera; // Ссылка на компонент Camera
    private Vector3 startposition; // Начальное положение камеры

    [SerializeField] private float speedX; // Скорость движения по оси X (не используется)
    [SerializeField] private float speedY; // Скорость движения по оси Y (не используется)
    [SerializeField] private float sizespeedX; // Скорость изменения масштаба камеры
    [SerializeField] private float distanceX; // Порог расстояния по оси X
    [SerializeField] private float distanceY; // Порог расстояния по оси Y
    [SerializeField] private float minOrthographicSize = 7f; // Минимальный ортографический размер (новый параметр)

    private void Awake()
    {
        camera = GetComponent<Camera>(); // Получаем компонент Camera
        startposition = transform.position; // Сохраняем начальное положение
        camera.orthographicSize = minOrthographicSize; // Устанавливаем начальный размер камеры
    }

    void Start()
    {
        if (Player1Switcher == null || Player2Switcher == null)
        {
            Debug.LogError("CameraController2p: Player1Switcher или Player2Switcher не назначены. Отключение скрипта.");
            enabled = false;
        }
    }

    void Update()
    {
        if (Player1 == null || Player2 == null)
        {
            return; // Выходим, если игроки не найдены
        }

        // Сглаживание движения камеры
        float lerpSpeed = 3.0f;
        Vector3 targetPosition = new Vector3(
            Mathf.Lerp(transform.position.x, (Player1.position.x + Player2.position.x) / 2, Time.deltaTime * lerpSpeed),
            Mathf.Lerp(transform.position.y, (Player1.position.y + Player2.position.y) / 2, Time.deltaTime * lerpSpeed),
            startposition.z
        );

        transform.position = targetPosition;

        // Изменение масштаба камеры
        float distanceXBetweenPlayers = Mathf.Abs(Player1.position.x - Player2.position.x);
        float distanceYBetweenPlayers = Mathf.Abs(Player1.position.y - Player2.position.y);

        if (distanceXBetweenPlayers > camera.orthographicSize * distanceX ||
            distanceYBetweenPlayers > camera.orthographicSize * distanceY)
        {
            camera.orthographicSize += Time.deltaTime * sizespeedX; // Отдаляем камеру
        }
        else if (camera.orthographicSize > minOrthographicSize)
        {
            camera.orthographicSize -= Time.deltaTime * sizespeedX; // Приближаем камеру
        }
    }

    public void Reset()
    {
        camera.orthographicSize = minOrthographicSize; // Сбрасываем к заданному значению
        transform.position = startposition;
    }
}