using UnityEngine;
using System.Collections;

public class FireWallAnimatorTrap : MonoBehaviour
{
    public Animator animator;
    public float activeDuration = 2f;
    public float inactiveDuration = 2f;

    private BoxCollider2D col;
    private Vector2 baseSize;
    private Vector2 baseOffset;

    void Awake()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        baseSize = col.size;
        baseOffset = col.offset;

        // Начинаем в выключенном состоянии
        col.enabled = false;
        animator.Play("IdleOff", 0, 0);
    }

    void Start()
    {
        StartCoroutine(CycleRoutine());
    }

    private IEnumerator CycleRoutine()
    {
        while (true)
        {
            // Активация
            animator.Play("Appear");
            yield return new WaitForSeconds(0.2f); // начало появления
            col.enabled = true;

            float activeTime = activeDuration;
            while (activeTime > 0f)
            {
                UpdateColliderToMatchAnimator();
                activeTime -= Time.deltaTime;
                yield return null;
            }

            // Деактивация
            col.enabled = false;
            animator.Play("Disappear");
            yield return new WaitForSeconds(inactiveDuration);
        }
    }

    private void UpdateColliderToMatchAnimator()
    {
        // Предположим, что высота объекта меняется от 0 до 1
        float currentYScale = transform.localScale.y; // если в анимации scaleY анимируется
        col.size = new Vector2(baseSize.x, baseSize.y * currentYScale);
        col.offset = new Vector2(baseOffset.x, baseOffset.y * currentYScale);
    }
}
