using UnityEngine;

public class LevelEndTrigger2p : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LevelController2P.instance != null) // �������� �� null, ����� �������� ������
        {
            if (collision.CompareTag("Player1"))
            {
                LevelController2P.instance.CharacterFinished(1); // Player1 ������� �������
            }
            else if (collision.CompareTag("Player2"))
            {
                LevelController2P.instance.CharacterFinished(2); // Player2 ������� �������
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (LevelController2P.instance != null) // �������� �� null, ����� �������� ������
        {
            if (collision.CompareTag("Player1"))
            {
                LevelController2P.instance.CharacterLeft(1); // Player1 ������� �������
            }
            else if (collision.CompareTag("Player2"))
            {
                LevelController2P.instance.CharacterLeft(2); // Player2 ������� �������
            }
        }
    }
}
