using UnityEngine;
using UnityEngine.UI;
using static CombatEventManager;

public class ButtonControlScript : MonoBehaviour
{
    public int Amount;
    public int EnergyCost;
    public DefenceRow DefenceRow;
    public EntityType Target;


    private void OnEnable()
    {
        CombatEventManager.StartTurnEvent += PlayerTurnStart;
        CombatEventManager.EndTurnEvent += PlayerTurnEnd;
        CombatEventManager.RemainingEnergyEvent += CheckEnoughEnergy;
    }

    private void OnDisable()
    {
        CombatEventManager.StartTurnEvent -= PlayerTurnStart;
        CombatEventManager.EndTurnEvent -= PlayerTurnEnd;
        CombatEventManager.RemainingEnergyEvent -= CheckEnoughEnergy;
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
        CombatEventManager.DealDamage(EntityType.Player, Target, DefenceRow, Amount);
        CombatEventManager.SpendEnergy(EnergyCost);
    }

    public void AddDefence()
    {
        CombatEventManager.AddDefence(EntityType.Player, Target, DefenceRow, Amount);
        CombatEventManager.SpendEnergy(EnergyCost);
    }

    public void CheckEnoughEnergy(int energyAmount)
    {
        if (EnergyCost > energyAmount)
        {
            this.transform.GetComponent<Button>().interactable = false;
        }
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
        CombatEventManager.DealDamage(EntityType.Player, Target, defenceRow, damage);
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
        CombatEventManager.DealDamage(EntityType.Player, Target, defenceRow, Amount);
        CombatEventManager.SpendEnergy(EnergyCost);
    }
    public void EndTurn()
    {
        CombatEventManager.EndTurn(Target);
    }
}
