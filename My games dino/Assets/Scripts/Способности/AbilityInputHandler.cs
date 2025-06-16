using UnityEngine;
using System.Collections;

public class AbilityInputHandler : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private MonoBehaviour abilityScript; // �� ��� ����� ���������, �� ����� ������
    private IAbility ability;

    [SerializeField] public string abilityId;
    [SerializeField] private float cooldownTime = 3f;

    private KeyCode activationKey;
    private AbilityCooldownUI cooldownUI;
    private CharacterSwitcher ownerSwitcher;

    public void SetActivationKey(KeyCode key)
    {
        activationKey = key;
    }

    public void SetCooldownUI(AbilityCooldownUI ui)
    {
        cooldownUI = ui;
    }

    public void SetOwner(CharacterSwitcher switcher)
    {
        ownerSwitcher = switcher;
    }

    // ������������� ���������� � �������� ���������� �����������
    IEnumerator Start()
    {
        animator = GetComponent<Animator>();
        ability = abilityScript as IAbility ?? GetComponent<IAbility>();

        if (ability == null)
        {
            Debug.LogError("Assigned script does not implement IAbility interface.");
        }

        // �������� abilityId ����� HeroImage, ���� ��� �� ������
        if (string.IsNullOrEmpty(abilityId))
        {
            HeroImage heroImage = GetComponent<HeroImage>();
            if (heroImage != null)
            {
                abilityId = heroImage.abilityId;
            }
        }

        yield return null;
    }

    void Update()
    {
        if (GameStateManager.Instance != null && GameStateManager.Instance.IsGameFrozen)
            return;

        if (Input.GetKeyDown(activationKey))
        {
            ExecuteAbility();
        }
    }

    public void ExecuteAbility()
    {
        if (ownerSwitcher == null) return;

        GameObject currentCharacter = ownerSwitcher.CurrentCharacter;
        if (currentCharacter == null) return;

        IAbility ability = currentCharacter.GetComponent<IAbility>();
        string baseAbilityId = currentCharacter.GetComponent<HeroImage>()?.abilityId;

        // ��������� ����������� ID ����������� � ������ ������
        string uniqueId = $"P{ownerSwitcher.playerNumber}_{baseAbilityId}";

        Debug.Log($"Attempting to execute ability: {abilityId} with uniqueId: {uniqueId}");

        // ���������, �� �� �������� �� �����������
        if (ability != null &&
            !AbilityCooldownTracker.Instance.IsOnCooldown(uniqueId) &&
            !AbilityCooldownTracker.Instance.IsActive(uniqueId))
        {
            // ��������� �������� ������������� �����������
            Animator characterAnimator = currentCharacter.GetComponent<Animator>();
            if (characterAnimator != null)
            {
                characterAnimator.SetTrigger("UseAbility");
            }
            else
            {
                Debug.LogError("Animator component is missing on character.");
            }

            // ���������� �����������
            ability.Activate();

            float usedActiveTime = (ability is IAbilityWithDuration durationAbility)
                ? durationAbility.Duration
                : 0f;

            // �������� ������� �����������
            AbilityCooldownTracker.Instance.StartAbility(uniqueId, usedActiveTime, cooldownTime);

            if (cooldownUI != null)
            {
                cooldownUI.SetAbility(uniqueId, null);
            }

            Debug.Log($"Ability {abilityId} executed, starting cooldown.");
        }
        else
        {
            Debug.Log($"Ability {abilityId} cannot be executed because it is either on cooldown or active.");
        }
    }
}
