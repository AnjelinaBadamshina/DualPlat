using UnityEngine;

public static class SaveLoadManager2p
{
    public static void SavePlayerHealth(float health)
    {
        PlayerPrefs.SetFloat("PlayerHealth", health);
        PlayerPrefs.Save();
    }

    public static float LoadPlayerHealth()
    {
        return PlayerPrefs.GetFloat("PlayerHealth", 3f); // Возвращает здоровье по умолчанию (например, 100), если данные не были найдены
    }
}