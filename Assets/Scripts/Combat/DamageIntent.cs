using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageIntent : MonoBehaviour
{
    public TextMeshProUGUI DamageText;

    public void Initialize(int damage)
    {
        DamageText.text = damage.ToString();
    }
}
