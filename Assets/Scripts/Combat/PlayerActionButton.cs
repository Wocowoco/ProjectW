using System.Collections;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CombatEventManager;

public class PlayerActionButton : MonoBehaviour
{

    public PlayerAction PlayerAction;


    //Should be ItemCombatDetails
    public Texture Image;
    public int EnergyCost;
    public bool IsDefensive = false;
    public DamageType DamageType;
    public DefenceType DefenceType;
    public int ActionAmount;
    public int Cooldown;
    public AttackStyle AttackStyle;


    public GameObject EnergyDisplay;
    public RawImage ImageSlot;
    public RawImage ActionTypeIcon;
    public RawImage CooldownIcon;
    public TextMeshProUGUI ActionText;
    public GameObject CooldownObject;
    public TextMeshProUGUI CooldownText;
    public TextMeshProUGUI TopChanceText;
    public TextMeshProUGUI MiddleChanceText;
    public TextMeshProUGUI BottomChanceText;

    public RawImage ActionBorder;
    public RawImage ActionBackground;

    private Color _meleeColor = new Color32(88, 22, 22, 255);
    private Color _meleeColorBackground = new Color32(255, 200, 200, 255);

    private Color _magicColor = new Color32(37, 43, 86, 255);
    private Color _magicColorBackground = new Color32(200, 200, 255, 255);

    private Color _rangedColor = new Color32(51, 85, 37, 255);
    private Color _rangedColorBackground = new Color32(200, 255, 200, 255);

    private int _currentCooldown = 0;


    private void Awake()
    {
        DisableButton();
    }
    void Start()
    {
        AttackStyle = new() { Top = 5, Middle = 45, Bottom = 50 };

        PrintEnergy();
        SetAction();
        SetAttackStyle();
        SetCooldownText();
    }

    private void OnEnable()
    {
        CombatEventManager.EndTurnEvent += PlayerTurnEnd;
        CombatEventManager.RemainingEnergyEvent += CheckEnoughEnergy;
    }

    private void OnDisable()
    {
        CombatEventManager.EndTurnEvent -= PlayerTurnEnd;
        CombatEventManager.RemainingEnergyEvent -= CheckEnoughEnergy;
    }

    void FixedUpdate()
    {
        ActionTypeIcon.enabled = false;
        ActionTypeIcon.enabled = true;
        CooldownIcon.enabled = false;
        CooldownIcon.enabled = true;
    }

    private void SetAttackStyle()
    {
        //TODO - Add colors based on %
        TopChanceText.text = AttackStyle.Top.ToString() + "%";
        MiddleChanceText.text = AttackStyle.Middle.ToString() + "%";
        BottomChanceText.text = AttackStyle.Bottom.ToString() + "%";
    }

    private void SetAction()
    {
        //Set icon
        if (IsDefensive)
        {
            switch (DefenceType)
            {
                case DefenceType.None:
                    break;
                case DefenceType.Normal:
                    ActionTypeIcon.texture = CombatEventManager.ActionTextures.Normal;
                    break;
                case DefenceType.MeleeImmune:
                    ActionTypeIcon.texture = CombatEventManager.ActionTextures.MeleeImmune;
                    break;
                case DefenceType.RangedImmune:
                    ActionTypeIcon.texture = CombatEventManager.ActionTextures.RangedImmune;
                    break;
                case DefenceType.MagicImmune:
                    ActionTypeIcon.texture = CombatEventManager.ActionTextures.MagicImmune;
                    break;
                default:
                    break;
            }
        }
        else //Check attack types
        {
            switch (DamageType)
            {
                case DamageType.Melee:
                    ActionTypeIcon.texture = CombatEventManager.ActionTextures.Melee;
                    ActionBorder.color = _meleeColor;
                    ActionBackground.color = _meleeColorBackground;
                    break;
                case DamageType.Ranged:
                    ActionTypeIcon.texture = CombatEventManager.ActionTextures.Ranged;
                    ActionBorder.color = _rangedColor;
                    ActionBackground.color = _rangedColorBackground;
                    break;
                case DamageType.Magic:
                    ActionTypeIcon.texture = CombatEventManager.ActionTextures.Magic;
                    ActionBorder.color = _magicColor;
                    ActionBackground.color = _magicColorBackground;
                    break;
                default:
                    break;
            }
        }

        //Set text
        ActionText.text = ActionAmount.ToString();

        //Set item texture
        ImageSlot.texture = Image;

    }

    private void PrintEnergy()
    {
        GameObject background = EnergyDisplay.transform.Find("Background").gameObject;
        GameObject foreground = EnergyDisplay.transform.Find("Foreground").gameObject;
        //Delete placeholder energy
        Destroy(background.transform.GetChild(0).gameObject);
        Destroy(foreground.transform.GetChild(0).gameObject);

        for (int i = 0; i < EnergyCost; i++)
        {
            Instantiate(CombatEventManager.EnergyObjects.EnergyBackground, background.transform);
            Instantiate(CombatEventManager.EnergyObjects.EnergyForeground, foreground.transform);
        }
    }

    private void SetCooldownText()
    {

        //Item is currently not on cooldown, just display cooldown
        if (_currentCooldown == 0)
        {
            CooldownText.text = Cooldown.ToString();
        }
        else //Item is on cooldown, display total current and total cooldown
        {
            CooldownText.text = $"{_currentCooldown - 1} ({Cooldown})";
        }        
    }

    public void OnButtonClick()
    {
        //Start cooldown if item has a cooldown
        if (Cooldown != 0)
        {
            _currentCooldown = Cooldown;
            SetCooldownText();
            DisableButton();
        }

        if (IsDefensive)
        {
            DefenceRow defenceRow = CalculateDefenceRow();
            CombatEventManager.AddDefence(EntityType.Player, EntityType.Player, defenceRow, ActionAmount, DefenceType);
        }
        else
        {
            DefenceRow defenceRow = CalculateDefenceRow();
            CombatEventManager.DealDamage(EntityType.Player, EntityType.Enemy, defenceRow, ActionAmount, DamageType);
        }

        CombatEventManager.SpendEnergy(EnergyCost);
    }

    private DefenceRow CalculateDefenceRow()
    {
        //Calculate defencerow
        int randNumber = Random.Range(0, 101);

        if (randNumber > 100 - AttackStyle.Top)
        {
            return DefenceRow.Top;
        }
        else if (randNumber > 100 - AttackStyle.Top - AttackStyle.Middle)
        {
            return DefenceRow.Middle;
        }
        else
        {
            return DefenceRow.Bottom;
        }
    }

    private void PlayerTurnEnd(EntityType entity)
    {
        if (entity == EntityType.Player)
        {
            DisableButton();

            if (_currentCooldown != 0)
            {
                //Reduce cooldown by one
                _currentCooldown--;
            }
        } 
    }
    public void CheckEnoughEnergy(int energyAmount)
    {
        SetCooldownText();

        if (EnergyCost > energyAmount)
        {
            DisableButton();
        }
        else
        {
            TryEnableButton();
        }
    }

    private void DisableButton()
    {
        this.transform.GetComponent<Button>().interactable = false;
    }

    private void TryEnableButton()
    {
        if (_currentCooldown == 0)
        {
            this.transform.GetComponent<Button>().interactable = true;
        }
    }
}


