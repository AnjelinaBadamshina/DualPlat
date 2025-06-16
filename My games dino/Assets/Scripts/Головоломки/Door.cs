using System.Linq;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Vector3 openPosition;
    [SerializeField] private Vector3 closedPosition;
    [SerializeField] private float speed;
    [SerializeField] private ButtonPlate[] buttons;

    private void Update()
    {
        if (buttons.Any(button => button.Pressed)) // Через Linq проверяем, есть ли в массиве хоть одна нажатая кнопка
            transform.position = Vector3.MoveTowards(transform.position, openPosition, speed * Time.deltaTime); // Двигаем дверь в сторону openPosition
        else
            transform.position = Vector3.MoveTowards(transform.position, closedPosition, speed * Time.deltaTime); // Двигаем дверь в сторону closedPosition
    }
}