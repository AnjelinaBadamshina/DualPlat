using UnityEngine;

public class Damage1p : MonoBehaviour
{
    public Transform respawnPointPlayer1;      // Точка возрождения первого игрока
    public int collisionDamage = 1;            // Урон от ловушки
    public string tagPlayer1 = "Player";       // Тег игрока

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (!coll.gameObject.CompareTag(tagPlayer1)) return;

        var healthManager = coll.gameObject.GetComponent<HealthHero1p>();
        if (healthManager == null)
        {
            Debug.LogError("❌ Не найден компонент HealthHero1p на объекте: " + coll.gameObject.name);
            return;
        }

        if (healthManager.isInvincible)
        {
            Debug.Log("🛡 Игрок неуязвим. Урон не нанесён: " + coll.gameObject.name);
            return;
        }

        // Применяем урон
        healthManager.SetHealth(-collisionDamage);
        Debug.Log($"💥 Урон по {coll.gameObject.name}: -{collisionDamage}. Текущее HP: {healthManager.TotalHealth}");

        // Респаун игрока
        healthManager.transform.position = respawnPointPlayer1.position;
        Debug.Log($"↩ Игрок {coll.gameObject.name} перемещён на точку респауна: {respawnPointPlayer1.position}");

        // Сохраняем здоровье
        SaveLoadManager2p.SavePlayerHealth(healthManager.TotalHealth);
        Debug.Log("💾 Здоровье игрока сохранено.");
    }
}
