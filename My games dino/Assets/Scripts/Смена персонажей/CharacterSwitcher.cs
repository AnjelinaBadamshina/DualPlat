using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSwitcher : MonoBehaviour
{
    [Header("Режим игры: 1 - false, 2 - true")]
    public bool IsTwoPlayerMode = false;

    [Header("Настройки игрока")]
    public int playerNumber = 1; // 1 или 2

    [Header("Настройки персонажей")]
    public GameObject[] characterPrefabs;
    public KeyCode[] switchKeys;
    public Transform spawnPoint;

    [Header("Настройки UI")]
    public GameObject characterButtonPrefab;
    public Transform characterButtonParent;
    public float buttonScaleFactor = 1.2f;
    public Color selectedColor = Color.yellow;
    public Color unselectedColor = Color.white;

    [Header("UI кулдауна способности")]
    public AbilityCooldownUI cooldownUI;

    [Header("Клавиши активации способности по персонажу")]
    public KeyCode[] abilityKeys;

    private GameObject currentCharacter;
    private int currentCharacterIndex = 0;
    [SerializeField] private Button[] characterButtons;

    public GameObject CurrentCharacter { get { return currentCharacter; } }

    private bool lastFacingRight = true;
    private float lastHealth = 3f;

    public delegate void CharacterSwitched(Transform newCharacter);
    public static event CharacterSwitched OnCharacterSwitched;

    void Start()
    {
        SetupUI();
        SpawnCharacter(0);
    }

    void Update()
    {
        if (GameStateManager.Instance != null && GameStateManager.Instance.IsGameFrozen)
            return;

        for (int i = 0; i < switchKeys.Length; i++)
        {
            if (Input.GetKeyDown(switchKeys[i]))
            {
                SwitchCharacter(i);
            }
        }
    }

    public void SwitchCharacter(int index)
    {
        if (index == currentCharacterIndex) return;

        MovementHero2p movementScript = currentCharacter.GetComponent<MovementHero2p>();
        if (movementScript != null)
        {
            lastFacingRight = movementScript.FacingRight;
        }

        if (IsTwoPlayerMode)
        {
            HealthHero2p healthScript = currentCharacter.GetComponent<HealthHero2p>();
            if (healthScript != null)
            {
                lastHealth = healthScript.TotalHealth;
            }
        }
        else
        {
            HealthHero1p healthScript = currentCharacter.GetComponent<HealthHero1p>();
            if (healthScript != null)
            {
                lastHealth = healthScript.TotalHealth;
            }
        }

        Vector3 currentPosition = currentCharacter.transform.position;
        Quaternion currentRotation = currentCharacter.transform.rotation;

        
        Destroy(currentCharacter);
        SpawnCharacter(index, currentPosition, currentRotation, lastFacingRight, lastHealth);

        
    }

    public void SpawnCharacter(int index)
    {
        SpawnCharacter(index, spawnPoint.position, spawnPoint.rotation, true, 3f);
    }

    public void SpawnCharacter(int index, Vector3 position, Quaternion rotation, bool facingRight, float health)
    {
        if (index < 0 || index >= characterPrefabs.Length) return;

        currentCharacterIndex = index;
        currentCharacter = Instantiate(characterPrefabs[index], position, rotation);

        currentCharacter.tag = playerNumber == 1 ? "Player1" : "Player2";

        // Flip
        MovementHero2p movementScript = currentCharacter.GetComponent<MovementHero2p>();
        if (movementScript != null)
        {
            movementScript.FacingRight = facingRight;

            Vector3 scale = currentCharacter.transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (facingRight ? 1 : -1);
            currentCharacter.transform.localScale = scale;
        }

        // Health
        if (IsTwoPlayerMode)
        {
            var healthScript = currentCharacter.GetComponent<HealthHero2p>();
            if (healthScript != null)
            {
                healthScript.maxHealth = 3;
                healthScript.SetHealth(health - healthScript.TotalHealth);
                currentCharacter.GetComponent<Animator>()?.Rebind();
            }
        }
        else
        {
            var healthScript = currentCharacter.GetComponent<HealthHero1p>();
            if (healthScript != null)
            {
                healthScript.maxHealth = 3;
                healthScript.SetHealth(health - healthScript.TotalHealth);
                currentCharacter.GetComponent<Animator>()?.Rebind();
            }
        }

        // Назначаем AbilityInputHandler параметры: UI и клавишу
        AbilityInputHandler inputHandler = currentCharacter.GetComponent<AbilityInputHandler>();
        if (inputHandler != null)
        {
            inputHandler.SetOwner(this); // <-- ОБЯЗАТЕЛЬНО

            if (abilityKeys.Length > currentCharacterIndex)
                inputHandler.SetActivationKey(abilityKeys[currentCharacterIndex]);

            if (cooldownUI != null)
                inputHandler.SetCooldownUI(cooldownUI);
        }

        HeroImage heroImage = currentCharacter.GetComponent<HeroImage>();
        if (heroImage != null && cooldownUI != null)
        {
            string fullAbilityId = $"P{playerNumber}_{heroImage.abilityId}";

            // 🔧 Убедимся, что AbilityCooldownTracker знает об этой способности
            AbilityCooldownTracker.Instance.EnsureAbilityRegistered(fullAbilityId);

            // ✅ Установим в UI
            cooldownUI.SetAbility(fullAbilityId, heroImage.abilityIcon);
        }

        OnCharacterSwitched?.Invoke(currentCharacter.transform);

        UpdateUI();
    }


    private void FlipCharacter(GameObject character)
    {
        Vector3 scale = character.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * -1;
        character.transform.localScale = scale;
    }

    public void SetupUI()
    {
        characterButtons = new Button[characterPrefabs.Length];

        for (int i = 0; i < characterPrefabs.Length; i++)
        {
            GameObject buttonGO = Instantiate(characterButtonPrefab, characterButtonParent);
            Button button = buttonGO.GetComponent<Button>();
            Image buttonImage = buttonGO.GetComponent<Image>();
            TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();

            HeroImage heroImage = characterPrefabs[i].GetComponent<HeroImage>();
            if (heroImage != null)
            {
                buttonImage.sprite = heroImage.icon;
                if (buttonText != null)
                {
                    buttonText.text = heroImage.characterName;
                }
                else
                {
                    Debug.LogError("TextMeshProUGUI not found in prefab button");
                }
            }
            else
            {
                Debug.LogError("HeroImage not found in character prefab: " + characterPrefabs[i].name);
            }

            characterButtons[i] = button;
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < characterButtons.Length; i++)
        {
            RectTransform buttonRect = characterButtons[i].GetComponent<RectTransform>();
            Image buttonImage = characterButtons[i].GetComponent<Image>();

            if (i == currentCharacterIndex)
            {
                buttonRect.localScale = Vector3.one * buttonScaleFactor;
                buttonImage.color = selectedColor;
            }
            else
            {
                buttonRect.localScale = Vector3.one;
                buttonImage.color = unselectedColor;
            }
        }
    }
}
