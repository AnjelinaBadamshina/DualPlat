using UnityEngine;
using UnityEngine.UI;

public class CrystalCounter : MonoBehaviour
{
    public int crystal = 0;         // ѕеременна€ дл€ хранени€ количества подобранных кристаллов
    public Text crystalText;        // —сылка на текстовое поле, отображающее количество кристаллов


    // ћетод дл€ увеличени€ количества кристаллов и обновлени€ текстового пол€
    public void IncreaseCrystalCount()
    {
        if (crystal <= 3)
        {
            crystal = crystal+1;                      // ”величиваем счетчик кристаллов
        }

        crystalText.text = crystal.ToString();   // ќбновл€ем текстовое поле, отображающее количество кристаллов
    }
}


