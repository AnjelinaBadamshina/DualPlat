using UnityEngine;
using UnityEngine.UI; // ����������� ���������� UI

public class WaterRiseController : MonoBehaviour
{
    public float initialRiseSpeed = 0.5f; // ��������� �������� ������� ����
    public float riseAcceleration = 0.1f; // ��������� ������� ����
    public float riseInterval = 20.0f; // �������� ������� � �������� ����� ��������� ����
    private float nextRiseTime; // �����, ����� ���� ���������� � ��������� ���
    private float currentRiseSpeed; // ������� �������� ������� ����

    public Text countdownText; // ������ �� ��������� ������� UI

    private bool isPaused = false; // ���� �����
    private bool isGameOver = false; // ���� ����� ����

    private Vector3 initialPosition; // ��������� ������� ����

    void Start()
    {
        // ������������� ��������� �������� � ����� ���������� ������� ����
        currentRiseSpeed = initialRiseSpeed;
        nextRiseTime = Time.time + riseInterval;
        initialPosition = transform.position; // ��������� ��������� ������� ����
    }

    void Update()
    {
        if (isPaused || isGameOver)
        {
            return; // �� ��������� ������ � ����, ���� ���� �� ����� ��� ���������
        }

        // ��������� �������� ������ �� ������
        UpdateCountdown();

        // ���������, ���� �� ��������� ����
        if (Time.time >= nextRiseTime)
        {
            // ��������� ����
            RiseWater();
            // ��������� ����� ���������� ������� ����
            nextRiseTime = Time.time + riseInterval;
            // ����������� �������� �������
            currentRiseSpeed += riseAcceleration;
        }
    }

    void RiseWater()
    {
        // ��������� ���� �� ��� Y
        transform.position += Vector3.up * currentRiseSpeed;
    }

    void UpdateCountdown()
    {
        // ��������� ���������� ����� �� ���������� ������� ����
        float timeLeft = nextRiseTime - Time.time;

        // ��������� ��������� ������� ��������� �������
        countdownText.text = "�� ��������� �����: " + Mathf.Ceil(timeLeft).ToString() + "�";

        // ��������� ����� ������ � ����������� �� ����������� �������
        if (timeLeft <= 1.0f)
        {
            countdownText.color = Color.red; // ������ ���� ������ �� �������
        }
        else
        {
            countdownText.color = Color.white; // ������ ���� ������ �� �����
        }
    }

    public void StopAndHideTimer()
    {
        isPaused = true;

    }

    public void ResumeAndShowTimer()
    {
        isPaused = false;
        nextRiseTime = Time.time + riseInterval; // ��������� ����� ���������� ������� ����
    }

    // ��������� ����� ��� ������ ������ ����
    public void ResetWaterLevel()
    {
        transform.position = initialPosition; // ���������� ������� ����
        currentRiseSpeed = initialRiseSpeed; // ���������� �������� �������
        nextRiseTime = Time.time + riseInterval; // ���������� ����� �� ���������� �������
    }
}
