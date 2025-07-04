using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������, ������� ������� ������� ���������� Camera
[RequireComponent(typeof(Camera))]
public class CameraController2p : MonoBehaviour
{
    [Header("������ �� CharacterSwitcher")]
    [SerializeField] public CharacterSwitcher Player1Switcher; // ������ �� CharacterSwitcher ������� ������
    [SerializeField] public CharacterSwitcher Player2Switcher; // ������ �� CharacterSwitcher ������� ������

    private Transform Player1 { get { return Player1Switcher != null && Player1Switcher.CurrentCharacter != null ? Player1Switcher.CurrentCharacter.transform : null; } }
    private Transform Player2 { get { return Player2Switcher != null && Player2Switcher.CurrentCharacter != null ? Player2Switcher.CurrentCharacter.transform : null; } }

    private Camera camera; // ������ �� ��������� Camera
    private Vector3 startposition; // ��������� ��������� ������

    [SerializeField] private float speedX; // �������� �������� �� ��� X (�� ������������)
    [SerializeField] private float speedY; // �������� �������� �� ��� Y (�� ������������)
    [SerializeField] private float sizespeedX; // �������� ��������� �������� ������
    [SerializeField] private float distanceX; // ����� ���������� �� ��� X
    [SerializeField] private float distanceY; // ����� ���������� �� ��� Y
    [SerializeField] private float minOrthographicSize = 7f; // ����������� ��������������� ������ (����� ��������)

    private void Awake()
    {
        camera = GetComponent<Camera>(); // �������� ��������� Camera
        startposition = transform.position; // ��������� ��������� ���������
        camera.orthographicSize = minOrthographicSize; // ������������� ��������� ������ ������
    }

    void Start()
    {
        if (Player1Switcher == null || Player2Switcher == null)
        {
            Debug.LogError("CameraController2p: Player1Switcher ��� Player2Switcher �� ���������. ���������� �������.");
            enabled = false;
        }
    }

    void Update()
    {
        if (Player1 == null || Player2 == null)
        {
            return; // �������, ���� ������ �� �������
        }

        // ����������� �������� ������
        float lerpSpeed = 3.0f;
        Vector3 targetPosition = new Vector3(
            Mathf.Lerp(transform.position.x, (Player1.position.x + Player2.position.x) / 2, Time.deltaTime * lerpSpeed),
            Mathf.Lerp(transform.position.y, (Player1.position.y + Player2.position.y) / 2, Time.deltaTime * lerpSpeed),
            startposition.z
        );

        transform.position = targetPosition;

        // ��������� �������� ������
        float distanceXBetweenPlayers = Mathf.Abs(Player1.position.x - Player2.position.x);
        float distanceYBetweenPlayers = Mathf.Abs(Player1.position.y - Player2.position.y);

        if (distanceXBetweenPlayers > camera.orthographicSize * distanceX ||
            distanceYBetweenPlayers > camera.orthographicSize * distanceY)
        {
            camera.orthographicSize += Time.deltaTime * sizespeedX; // �������� ������
        }
        else if (camera.orthographicSize > minOrthographicSize)
        {
            camera.orthographicSize -= Time.deltaTime * sizespeedX; // ���������� ������
        }
    }

    public void Reset()
    {
        camera.orthographicSize = minOrthographicSize; // ���������� � ��������� ��������
        transform.position = startposition;
    }
}