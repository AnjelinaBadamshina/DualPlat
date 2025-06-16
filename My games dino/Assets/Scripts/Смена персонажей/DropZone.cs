using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop triggered"); // Проверяем, срабатывает ли OnDrop

        if (eventData.pointerDrag != null)
        {
            Debug.Log("Pointer Drag is not null");

            DraggableCharacter draggable = eventData.pointerDrag.GetComponent<DraggableCharacter>();
            if (draggable == null)
            {
                Debug.Log("No DraggableCharacter component found!");
                return;
            }

            Debug.Log("Draggable character found");

            RectTransform dropped = eventData.pointerDrag.GetComponent<RectTransform>();

            if (transform.childCount < 2) // Ограничиваем максимум 2 персонажа
            {
                Debug.Log("Dropping character on the panel");

                dropped.SetParent(transform, false); // false сохраняет локальные координаты
                dropped.anchoredPosition = Vector2.zero;

                // Обновляем кнопку старта
                CharacterSelectionManager manager = FindObjectOfType<CharacterSelectionManager>();
                if (manager != null)
                    manager.UpdateStartGameButtonState();
            }
            else
            {
                Debug.Log("Panel already has 2 characters.");
            }
        }
        else
        {
            Debug.Log("Pointer Drag is null");
        }
    }
}
