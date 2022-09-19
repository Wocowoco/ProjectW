using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CombatEventManager;

public class DamageIntent : MonoBehaviour
{
    public TextMeshProUGUI DamageText;
    public RawImage Background;

    public void Initialize(int damage, DamageType damageType)
    {
        DamageText.text = damage.ToString();
        switch (damageType)
        {
            case DamageType.Melee:
                Background.color = new Color32(164, 35, 35, 255);
                break;
            case DamageType.Ranged:
                Background.color = new Color32(72, 118, 53, 255);
                break;
            case DamageType.Magic:
                Background.color = new Color32(56, 67, 144, 255);
                break;
        }
    }
}
