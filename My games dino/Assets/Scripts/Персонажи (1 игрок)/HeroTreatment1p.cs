using UnityEngine;

public class HeroTreatment1p : MonoBehaviour
{
    public int collisionHeal = 1; // ���������� ������ ��������, ����������� ��� ������������
    public string tagPlayer1 = "Player"; // �� ��������� ��� ������ - Player

    public AudioSource Audio;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���������, ����� �� ������ � �������, ������� ��������� ���
        if (other.CompareTag(tagPlayer1))
        {
            // �������� ��������� �������� ���������
            HealthHero1p health = other.GetComponent<HealthHero1p>();
            if (health != null)
            {
                if (Audio != null) Audio.Play();
                // �������� ����� ������� � ���������� �������� � ��������� ���������� �������
                health.SetHealth(collisionHeal);

                // ���������� ������ ������� ����� �������������
                Destroy(gameObject);
            }
        }
    }
}