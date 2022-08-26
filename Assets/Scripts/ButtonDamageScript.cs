using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LifeNodeManager;

public class ButtonDamageScript : MonoBehaviour
{
    public int Damage;
    public DefenceRow DefenceRow;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DealDamage()
    {
        CombatEventManager.DealDamage(DefenceRow, Damage);
    }

    public void DealRandomDamage()
    {
        //Deal 1-3 damage on a random row
        int damage = Random.Range(1, 4);
        int row = Random.Range(1, 4);
        DefenceRow defenceRow = DefenceRow.Top;

        switch (row)
        {
            case 1:
                defenceRow = DefenceRow.Top;
                break;
            case 2:
                defenceRow = DefenceRow.Middle;
                break;
            case 3:
                defenceRow = DefenceRow.Bottom;
                break;
            default:
                break;
        }
        CombatEventManager.DealDamage(defenceRow, damage);
        Debug.Log($"Dealing {damage} damage to {defenceRow}.");
    }
}
