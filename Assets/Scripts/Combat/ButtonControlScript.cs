using UnityEngine;
using UnityEngine.UI;
using static CombatEventManager;

public class ButtonControlScript : MonoBehaviour
{
    public int Amount;
    public DefenceRow DefenceRow;
    public EntityType Target;


    private void OnEnable()
    {
        Debug.Log($"{this.transform.name} loaded.");
        CombatEventManager.StartTurnEvent += PlayerTurnStart;
        CombatEventManager.EndTurnEvent += PlayerTurnEnd;
    }

    private void OnDisable()
    {
        CombatEventManager.StartTurnEvent -= PlayerTurnStart;
        CombatEventManager.EndTurnEvent -= PlayerTurnEnd;
    }

    private void PlayerTurnStart(EntityType entity)
    {
        if (entity == EntityType.Player)
        {
            this.transform.GetComponent<Button>().interactable = true;
        }
    }
    private void PlayerTurnEnd(EntityType entity)
    {
        if (entity == EntityType.Player)
        {
            this.transform.GetComponent<Button>().interactable = false;
        }
    }
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
    }

    public void DealDamageRandomRow()
    {
        DefenceRow defenceRow = DefenceRow.Top;
        int row = Random.Range(1, 4);
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
        CombatEventManager.DealDamage(Target, defenceRow, Amount);
    }
    public void EndTurn()
    {
        CombatEventManager.EndTurn(Target);
    }
}
