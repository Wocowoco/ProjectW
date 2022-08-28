using UnityEngine;
using static CombatEventManager;

public class ButtonControlScript : MonoBehaviour
{
    public int Amount;
    public DefenceRow DefenceRow;
    public EntityType Target;

    public void DealDamage()
    {
        CombatEventManager.DealDamage(Target, DefenceRow, Amount);
    }

    public void AddDefence()
    {
        CombatEventManager.AddDefence(Target, DefenceRow, Amount);
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
        CombatEventManager.DealDamage(Target, defenceRow, damage);
        Debug.Log($"Dealing {damage} melee damage to {defenceRow}.");
    }
}
