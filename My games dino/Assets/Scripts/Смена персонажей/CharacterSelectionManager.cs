using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

// Менеджер выбора персонажей для 2 игроков
public class CharacterSelectionManager : MonoBehaviour
{
    [Header("Настройки персонажей")]
    public GameObject[] availableCharacters; // Доступные для выбора персонажи

    [Header("Ссылки на CharacterSwitcher")]
    public CharacterSwitcher player1Switcher;
    public CharacterSwitcher player2Switcher;

    [Header("Ссылки на UI")]
    public GameObject selectionPanel; // Панель выбора персонажа
    public Transform characterButtonParent; // Родитель для кнопок персонажей
    public GameObject characterButtonPrefab; // Префаб кнопки выбора персонажа
    public Button startGameButton; // Кнопка запуска игры

    [Header("Панели выбранных персонажей")]
    public Transform player1SelectedPanel;
    public Transform player2SelectedPanel;

    [Header("Точки спавна")]
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;

    private List<Button> characterButtons = new List<Button>(); // Список кнопок выбора персонажей
    private float originalTimeScale;

    // Списки выбранных персонажей каждым игроком
    private List<GameObject> selectedCharactersPlayer1 = new List<GameObject>();
    private List<GameObject> selectedCharactersPlayer2 = new List<GameObject>();

    void Start()
    {
        originalTimeScale = Time.timeScale;
        Time.timeScale = 0f; // Останавливаем игру, пока идет выбор

        CreateCharacterButtons(); // Генерируем UI кнопки для выбора
        UpdateStartGameButtonState(); // Проверяем, можно ли активировать кнопку старта
        startGameButton.gameObject.SetActive(false); // Прячем кнопку старта до выбора 2х персонажей
    }

    // Создание UI-кнопок для выбора персонажей
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
                buttonImage.sprite = heroImage.icon; // Иконка персонажа
                if (buttonText != null)
                {
                    buttonText.text = heroImage.characterName; // Имя персонажа
                }
            }
            else
            {
                Debug.LogError("HeroImage script not found on " + availableCharacters[i].name);
            }

            // Сохраняем ссылку на префаб в кнопке
            CharacterButtonInfo info = buttonGO.AddComponent<CharacterButtonInfo>();
            info.characterPrefab = availableCharacters[i];

            characterButtons.Add(button);

            // Добавляем скрипт для перетаскивания кнопки
            if (buttonGO.GetComponent<DraggableCharacter>() == null)
            {
                buttonGO.AddComponent<DraggableCharacter>();
            }
        }
    }

    // Проверка, выбрано ли по 2 персонажа, и обновление активности кнопки старта
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

    // Запуск игры после выбора персонажей
    public void StartGame()
    {
        Debug.Log("StartGame() called!");

        // Проверка наличия переключателей
        if (player1Switcher == null || player2Switcher == null)
        {
            Debug.LogError("CharacterSwitcher не назначен!");
            return;
        }

        // Проверка, что выбрано по 2 персонажа
        if (player1SelectedPanel.childCount != 2 || player2SelectedPanel.childCount != 2)
        {
            Debug.LogError("Не выбраны 2 персонажа для каждого игрока!");
            return;
        }

        // Подготовка массивов персонажей
        player1Switcher.characterPrefabs = new GameObject[2];
        player2Switcher.characterPrefabs = new GameObject[2];

        // Загрузка выбранных персонажей для игрока 1
        for (int i = 0; i < 2; i++)
        {
            CharacterButtonInfo info1 = player1SelectedPanel.GetChild(i).GetComponent<CharacterButtonInfo>();
            if (info1 != null)
            {
                GameObject player1Character = Instantiate(info1.characterPrefab);
                player1Switcher.characterPrefabs[i] = player1Character;
                Debug.Log("Player 1 selected: " + info1.characterPrefab.name);
                player1Character.SetActive(true); // Активируем персонажа
            }
            else
            {
                Debug.LogError("CharacterButtonInfo для Player 1 не найден");
            }
        }

        // Загрузка выбранных персонажей для игрока 2
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
                Debug.LogError("CharacterButtonInfo для Player 2 не найден");
            }
        }

        // ОБЯЗАТЕЛЬНО: Устанавливаем данные в CharacterSwitcher до спавна
        player1Switcher.spawnPoint = player1SpawnPoint;
        player1Switcher.playerNumber = 1;

        player2Switcher.spawnPoint = player2SpawnPoint;
        player2Switcher.playerNumber = 2;

        // Генерируем UI и спавним стартового персонажа
        player1Switcher.SetupUI();
        player1Switcher.SpawnCharacter(0);
        player2Switcher.SetupUI();
        player2Switcher.SpawnCharacter(0);

        // Включаем управление камерой
        CameraController2p cameraController = GetComponent<CameraController2p>();
        if (cameraController != null)
        {
            cameraController.enabled = true;
        }

        // Прячем меню выбора и запускаем игру
        selectionPanel.SetActive(false);
        gameObject.SetActive(false);
        Time.timeScale = originalTimeScale; // Возобновляем игру

        Debug.Log("Game starting!");
    }

    // Устаревший метод ручного спавна (не используется)
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

    // Логика обработки перетаскивания персонажей на панели
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

        UpdateStartGameButtonState(); // Проверяем, можно ли запускать игру
    }

    // Удаление персонажа из панелей выбора при возврате
    public void RemoveCharacterFromSelection(GameObject character)
    {
        if (selectedCharactersPlayer1.Contains(character))
        {
            selectedCharactersPlayer1.Remove(character);
            Debug.Log("Удален из Player 1: " + character.name);
        }
        else if (selectedCharactersPlayer2.Contains(character))
        {
            selectedCharactersPlayer2.Remove(character);
            Debug.Log("Удален из Player 2: " + character.name);
        }

        UpdateStartGameButtonState();
    }

}
