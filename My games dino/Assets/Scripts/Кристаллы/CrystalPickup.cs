using UnityEngine;

public class CrystalPickup : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 1.5f, 0);
    public float followSpeed = 5f;

    private bool pickedUp = false;
    private CharacterSwitcher owner;

    public static bool mainCrystalPickedUp = false; // 🔥 Новый флаг

    private void OnEnable()
    {
        CharacterSwitcher.OnCharacterSwitched += OnCharacterSwitched;
    }

    private void OnDisable()
    {
        CharacterSwitcher.OnCharacterSwitched -= OnCharacterSwitched;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (pickedUp) return;

        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            // Найти всех переключателей
            CharacterSwitcher[] allSwitchers = FindObjectsOfType<CharacterSwitcher>();
            foreach (var switcher in allSwitchers)
            {
                if (switcher.CurrentCharacter == other.gameObject)
                {
                    owner = switcher;
                    pickedUp = true;

                    // 🔥 Отмечаем, если это главный кристалл
                    if (CompareTag("MainCrystal"))
                    {
                        mainCrystalPickedUp = true;
                        Debug.Log("Главный кристалл собран!");
                    }

                    GetComponent<Collider2D>().enabled = false;

                    Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                        rb.simulated = false;
                    }

                    break;
                }
            }
        }
    }

    private void Update()
    {
        if (pickedUp && owner != null && owner.CurrentCharacter != null)
        {
            Vector3 targetPosition = owner.CurrentCharacter.transform.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }

    private void OnCharacterSwitched(Transform newCharacter)
    {
        // Ничего не нужно — объект сам следует за текущим персонажем
    }
}
