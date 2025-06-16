using UnityEngine;

public class MovingPlatform1 : MonoBehaviour
{
    // ����������, ����� ��������� ��������� ������ � ��������� ���������
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���������, �������� �� ������ Player1 ��� Player2
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            // ��������� ������� ��������, ����� �������� ��� ������ ���������
            ContactPoint2D contact = collision.GetContact(0);
            if (contact.normal.y > 0.5f) // ������� ����� (�������� ����� �� ���������)
            {
                // ������������� ��������� ��� �������� ��� ���������
                collision.transform.SetParent(transform);
            }
        }
    }

    // ����������, ����� ��������� ��������� �������� ��������� ���������
    private void OnCollisionExit2D(Collision2D collision)
    {
        // ���������, �������� �� ������ Player1 ��� Player2
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            // ������� ������������, ����� �������� ������ �� �������� � ����������
            collision.transform.SetParent(null);
        }
    }
}