using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableCharacter : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector3 originalScale;

    private CharacterSelectionManager selectionManager;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        selectionManager = FindObjectOfType<CharacterSelectionManager>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(canvas.transform, true);
        canvasGroup.blocksRaycasts = false;
        rectTransform.localScale = originalScale * 1.2f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        rectTransform.localScale = originalScale;

        // Если всё ещё на канвасе, значит не было попадания в Drop-зону
        if (transform.parent == canvas.transform)
        {
            // Если вернули на стартовую панель — удаляем из выбранных
            if (eventData.pointerEnter != null &&
                eventData.pointerEnter.transform == selectionManager.characterButtonParent)
            {
                transform.SetParent(selectionManager.characterButtonParent);
                transform.localScale = Vector3.one;
                selectionManager.RemoveCharacterFromSelection(gameObject);
            }
            else
            {
                // Просто вернём в исходную панель
                transform.SetParent(originalParent);
                rectTransform.anchoredPosition = Vector2.zero;
            }
        }
    }
}
