using UnityEngine;

public class FlyingEnemyMovement : MonoBehaviour
{
    public Transform leftPoint;
    public Transform rightPoint;
    public float moveSpeed = 2f;
    public float amplitude = 1f;
    public float frequency = 1f;

    private Vector3 startPos;
    private bool movingRight = true;
    private float baseY;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        startPos = transform.position;
        baseY = startPos.y;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        Vector3 pos = transform.position;

        if (movingRight)
        {
            pos.x += moveSpeed * Time.deltaTime;
            if (pos.x >= rightPoint.position.x)
            {
                pos.x = rightPoint.position.x;
                movingRight = false;
                Flip();
            }
        }
        else
        {
            pos.x -= moveSpeed * Time.deltaTime;
            if (pos.x <= leftPoint.position.x)
            {
                pos.x = leftPoint.position.x;
                movingRight = true;
                Flip();
            }
        }

        pos.y = baseY + yOffset;
        transform.position = pos;
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = movingRight ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
}
