using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform pointA;
    public Transform pointB;

    private Vector3 target;
    private bool movingToB = true;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        target = pointB.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        FaceDirection();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            movingToB = !movingToB;
            target = movingToB ? pointB.position : pointA.position;
            FaceDirection();
        }
    }

    void FaceDirection()
    {
        if (spriteRenderer == null) return;

        if (target.x < transform.position.x)
            spriteRenderer.flipX = false;  // Лицо влево
        else
            spriteRenderer.flipX = true; // Лицо вправо
    }
}