using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

// �������� ������ ���������� ��� 2 �������
public class CharacterSelectionManager : MonoBehaviour
{
    [Header("��������� ����������")]
    public GameObject[] availableCharacters; // ��������� ��� ������ ���������

    [Header("������ �� CharacterSwitcher")]
    public CharacterSwitcher player1Switcher;
    public CharacterSwitcher player2Switcher;

    [Header("������ �� UI")]
    public GameObject selectionPanel; // ������ ������ ���������
    public Transform characterButtonParent; // �������� ��� ������ ����������
    public GameObject characterButtonPrefab; // ������ ������ ������ ���������
    public Button startGameButton; // ������ ������� ����

    [Header("������ ��������� ����������")]
    public Transform player1SelectedPanel;
    public Transform player2SelectedPanel;

    [Header("����� ������")]
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;

    private List<Button> characterButtons = new List<Button>(); // ������ ������ ������ ����������
    private float originalTimeScale;

    // ������ ��������� ���������� ������ �������
    private List<GameObject> selectedCharactersPlayer1 = new List<GameObject>();
    private List<GameObject> selectedCharactersPlayer2 = new List<GameObject>();

    void Start()
    {
        originalTimeScale = Time.timeScale;
        Time.timeScale = 0f; // ������������� ����, ���� ���� �����

        CreateCharacterButtons(); // ���������� UI ������ ��� ������
        UpdateStartGameButtonState(); // ���������, ����� �� ������������ ������ ������
        startGameButton.gameObject.SetActive(false); // ������ ������ ������ �� ������ 2� ����������
    }

    // �������� UI-������ ��� ������ ����������
    void CreateCharacterButtons()
    {
        characterButtons = new List<Button>();

        for (int i = 0; i < availableCharacters.Length; i++)
        {
            GameObject buttonGO = Instantiate(characterButtonPrefab, characterButtonParent);
            Button button = buttonGO.GetComponent<Button>();
            Image buttonImage = buttonGO.GetComponent<Image>();
            buttonImage.color = Color.white;
            TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();

            HeroImage heroImage = availableCharacters[i].GetComponent<HeroImage>();
            if (heroImage != null)
            {
                buttonImage.sprite = heroImage.icon; // ������ ���������
                if (buttonText != null)
                {
                    buttonText.text = heroImage.characterName; // ��� ���������
                }
            }
            else
            {
                Debug.LogError("HeroImage script not found on " + availableCharacters[i].name);
            }

            // ��������� ������ �� ������ � ������
            CharacterButtonInfo info = buttonGO.AddComponent<CharacterButtonInfo>();
            info.characterPrefab = availableCharacters[i];

            characterButtons.Add(button);

            // ��������� ������ ��� �������������� ������
            if (buttonGO.GetComponent<DraggableCharacter>() == null)
            {
                buttonGO.AddComponent<DraggableCharacter>();
            }
        }
    }

    // ��������, ������� �� �� 2 ���������, � ���������� ���������� ������ ������
    public void UpdateStartGameButtonState()
    {
        Debug.Log("Player 1 selected count: " + player1SelectedPanel.childCount);
        Debug.Log("Player 2 selected count: " + player2SelectedPanel.childCount);

        if (player1SelectedPanel.childCount == 2 && player2SelectedPanel.childCount == 2)
        {
            startGameButton.gameObject.SetActive(true);
        }
        else
        {
            startGameButton.gameObject.SetActive(false);
        }
    }

    // ������ ���� ����� ������ ����������
    public void StartGame()
    {
        Debug.Log("StartGame() called!");

        // �������� ������� ��������������
        if (player1Switcher == null || player2Switcher == null)
        {
            Debug.LogError("CharacterSwitcher �� ��������!");
            return;
        }

        // ��������, ��� ������� �� 2 ���������
        if (player1SelectedPanel.childCount != 2 || player2SelectedPanel.childCount != 2)
        {
            Debug.LogError("�� ������� 2 ��������� ��� ������� ������!");
            return;
        }

        // ���������� �������� ����������
        player1Switcher.characterPrefabs = new GameObject[2];
        player2Switcher.characterPrefabs = new GameObject[2];

        // �������� ��������� ���������� ��� ������ 1
        for (int i = 0; i < 2; i++)
        {
            CharacterButtonInfo info1 = player1SelectedPanel.GetChild(i).GetComponent<CharacterButtonInfo>();
            if (info1 != null)
            {
                GameObject player1Character = Instantiate(info1.characterPrefab);
                player1Switcher.characterPrefabs[i] = player1Character;
                Debug.Log("Player 1 selected: " + info1.characterPrefab.name);
                player1Character.SetActive(true); // ���������� ���������
            }
            else
            {
                Debug.LogError("CharacterButtonInfo ��� Player 1 �� ������");
            }
        }

        // �������� ��������� ���������� ��� ������ 2
        for (int i = 0; i < 2; i++)
        {
            CharacterButtonInfo info2 = player2SelectedPanel.GetChild(i).GetComponent<CharacterButtonInfo>();
            if (info2 != null)
            {
                GameObject player2Character = Instantiate(info2.characterPrefab);
                player2Switcher.characterPrefabs[i] = player2Character;
                Debug.Log("Player 2 selected: " + info2.characterPrefab.name);
                player2Character.SetActive(true);
            }
            else
            {
                Debug.LogError("CharacterButtonInfo ��� Player 2 �� ������");
            }
        }

        // �����������: ������������� ������ � CharacterSwitcher �� ������
        player1Switcher.spawnPoint = player1SpawnPoint;
        player1Switcher.playerNumber = 1;

        player2Switcher.spawnPoint = player2SpawnPoint;
        player2Switcher.playerNumber = 2;

        // ���������� UI � ������� ���������� ���������
        player1Switcher.SetupUI();
        player1Switcher.SpawnCharacter(0);
        player2Switcher.SetupUI();
        player2Switcher.SpawnCharacter(0);

        // �������� ���������� �������
        CameraController2p cameraController = GetComponent<CameraController2p>();
        if (cameraController != null)
        {
            cameraController.enabled = true;
        }

        // ������ ���� ������ � ��������� ����
        selectionPanel.SetActive(false);
        gameObject.SetActive(false);
        Time.timeScale = originalTimeScale; // ������������ ����

        Debug.Log("Game starting!");
    }

    // ���������� ����� ������� ������ (�� ������������)
    private void SpawnCharacters()
    {
        foreach (var characterPrefab in player1Switcher.characterPrefabs)
        {
            Instantiate(characterPrefab, player1SpawnPoint.position, Quaternion.identity);
        }

        foreach (var characterPrefab in player2Switcher.characterPrefabs)
        {
            Instantiate(characterPrefab, player2SpawnPoint.position, Quaternion.identity);
        }
    }

    // ������ ��������� �������������� ���������� �� ������
    public void DraggedCharacterToPanel(GameObject draggedCharacter, Transform targetPanel)
    {
        if (targetPanel == player1SelectedPanel && selectedCharactersPlayer1.Count < 2)
        {
            selectedCharactersPlayer1.Add(draggedCharacter);
            draggedCharacter.transform.SetParent(player1SelectedPanel);
            draggedCharacter.transform.localScale = Vector3.one;
            draggedCharacter.SetActive(true);
            Debug.Log("Player 1 added: " + draggedCharacter.name);
            Debug.Log("Player 1 selected characters count: " + selectedCharactersPlayer1.Count);
        }
        else if (targetPanel == player2SelectedPanel && selectedCharactersPlayer2.Count < 2)
        {
            selectedCharactersPlayer2.Add(draggedCharacter);
            draggedCharacter.transform.SetParent(player2SelectedPanel);
            draggedCharacter.transform.localScale = Vector3.one;
            draggedCharacter.SetActive(true);
            Debug.Log("Player 2 added: " + draggedCharacter.name);
            Debug.Log("Player 2 selected characters count: " + selectedCharactersPlayer2.Count);
        }

        UpdateStartGameButtonState(); // ���������, ����� �� ��������� ����
    }

    // �������� ��������� �� ������� ������ ��� ��������
    public void RemoveCharacterFromSelection(GameObject character)
    {
        if (selectedCharactersPlayer1.Contains(character))
        {
            selectedCharactersPlayer1.Remove(character);
            Debug.Log("������ �� Player 1: " + character.name);
        }
        else if (selectedCharactersPlayer2.Contains(character))
        {
            selectedCharactersPlayer2.Remove(character);
            Debug.Log("������ �� Player 2: " + character.name);
        }

        UpdateStartGameButtonState();
    }

}
