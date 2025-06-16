using UnityEngine;

public class Damage2p : MonoBehaviour
{
    [Header("Ссылки на CharacterSwitcher")]
    [SerializeField] public CharacterSwitcher Player1Switcher;
    [SerializeField] public CharacterSwitcher Player2Switcher;

    [Header("Точки возрождения")]
    public Transform respawnPointPlayer1;
    public Transform respawnPointPlayer2;

    public int collisionDamage = 1;

    private void OnCollisionEnter2D(Collision2D coll)
    {
        var healthManager = coll.gameObject.GetComponent<HealthHero2p>();
        if (healthManager == null) return;

        var player1 = Player1Switcher?.CurrentCharacter?.transform;
        var player2 = Player2Switcher?.CurrentCharacter?.transform;

        if (player1 == null || player2 == null) return;

        HealthHero2p otherHealth = null;
        Transform otherTransform = null;
        Transform thisRespawn = null;
        Transform otherRespawn = null;

        if (coll.transform == player1)
        {
            otherTransform = player2;
            otherHealth = otherTransform.GetComponent<HealthHero2p>();
            thisRespawn = respawnPointPlayer1;
            otherRespawn = respawnPointPlayer2;
        }
        else if (coll.transform == player2)
        {
            otherTransform = player1;
            otherHealth = otherTransform.GetComponent<HealthHero2p>();
            thisRespawn = respawnPointPlayer2;
            otherRespawn = respawnPointPlayer1;
        }
        else return;

        if (healthManager.isInvincible || (otherHealth != null && otherHealth.isInvincible))
        {
            Debug.Log("🛡 Один из игроков неуязвим. Урон не нанесён.");
            return;
        }

        // Применяем урон
        healthManager.SetHealth(-collisionDamage);
        if (otherHealth != null)
            otherHealth.SetHealth(-collisionDamage);

        Debug.Log($"💥 Урон игроку {coll.gameObject.name} и второму игроку: -{collisionDamage}");

        // Респаун обоих
        healthManager.transform.position = thisRespawn.position;
        if (otherHealth != null)
            otherHealth.transform.position = otherRespawn.position;

        Debug.Log($"↩ Оба игрока перемещены на точки респауна");

        // Сохраняем здоровье
        SaveLoadManager2p.SavePlayerHealth(healthManager.TotalHealth);
        if (otherHealth != null)
            SaveLoadManager2p.SavePlayerHealth(otherHealth.TotalHealth);

        Debug.Log("💾 Здоровье обоих игроков сохранено.");
    }
}
