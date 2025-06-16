using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController1p : MonoBehaviour
{
    [SerializeField] public Transform Player1;
    [SerializeField] private float speedX;
    [SerializeField] private float speedY;
    //[SerializeField] private float sizespeedX; // ������, �.�. �� ����������
    [SerializeField] private float distanceX;
    [SerializeField] private float distanceY;
    //[SerializeField] private float minOrthographicSize = 3f; // ������, �.�. ������� �������������

    private Camera camera;
    private Vector3 startposition;
    private float initialOrthographicSize;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        startposition = transform.position;
        initialOrthographicSize = camera.orthographicSize; // ���������� ��������� �������
    }

    void Start()
    {
        CharacterSwitcher.OnCharacterSwitched += OnCharacterSwitched;

        if (Player1 == null)
        {
            CharacterSwitcher characterSwitcher = FindObjectOfType<CharacterSwitcher>();
            if (characterSwitcher != null && characterSwitcher.CurrentCharacter != null)
            {
                Player1 = characterSwitcher.CurrentCharacter.transform;
            }

            if (Player1 == null)
            {
                Debug.LogError("Player1 �� ��������! ������ �� ����� ��������.");
            }
        }
    }

    void OnDestroy()
    {
        CharacterSwitcher.OnCharacterSwitched -= OnCharacterSwitched;
    }

    void OnCharacterSwitched(Transform newCharacter)
    {
        Player1 = newCharacter;
    }

    void Update()
    {
        if (Player1 == null) return;

        float lerpSpeed = 3.0f;
        Vector3 targetPosition = new Vector3(
            Mathf.Lerp(transform.position.x, Player1.position.x, Time.deltaTime * lerpSpeed),
            Mathf.Lerp(transform.position.y, Player1.position.y, Time.deltaTime * lerpSpeed),
            startposition.z // ��� ����� ������, ����� ������ �� ������������ �� Z
        );
        transform.position = targetPosition;

        // **������ ��� ��������� ��������**
        // float distanceXFromCenter = Mathf.Abs(Player1.position.x - transform.position.x);
        // float distanceYFromCenter = Mathf.Abs(Player1.position.y - transform.position.y);

        // if (distanceXFromCenter > camera.orthographicSize * distanceX ||
        //     distanceYFromCenter > camera.orthographicSize * distanceY)
        // {
        //     camera.orthographicSize += Time.deltaTime * sizespeedX;
        // }
        // else if (camera.orthographicSize > minOrthographicSize)
        // {
        //     camera.orthographicSize -= Time.deltaTime * sizespeedX;
        //     camera.orthographicSize = Mathf.Max(camera.orthographicSize, minOrthographicSize);
        // }
    }

    public void Reset()
    {
        camera.orthographicSize = initialOrthographicSize;
        // transform.position = startposition; //� ��� �����, ����� ������� ������ � ��������� ������� �� ��������� � ������
    }
}