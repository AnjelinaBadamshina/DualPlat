using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HeavyObjectController : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject authorizedMover;
    private float moveTimer;

    public float blockedMoveSpeed = 0.01f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        // ������ ��������� �������� �� ��� Z
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void FixedUpdate()
    {
        if (authorizedMover == null || moveTimer <= 0f)
        {
            // ����� �� ��������: ��������� X � ��������� ���������� Z
            if (rb.constraints != (RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation))
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
        }
        else
        {
            // ����� ��������: ������������ X, ��������� ���������� Z
            if (rb.constraints != RigidbodyConstraints2D.FreezeRotation)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }

            moveTimer -= Time.fixedDeltaTime;
        }
    }

    public void AllowMovement(GameObject mover, float duration)
    {
        authorizedMover = mover;
        moveTimer = duration;
    }

    public bool CanBeMovedBy(GameObject mover)
    {
        return mover == authorizedMover && moveTimer > 0f;
    }
}