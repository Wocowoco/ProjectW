using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LifeNodeManager : MonoBehaviour
{

    public int StartingHealth;
    public int StartingDefence;

    private int _health;
    private int _defenceTop;
    private int _defenceMiddle;
    private int _defenceBottom;
    private GameObject _defenceRowTop;
    private GameObject _defenceRowMiddle;
    private GameObject _defenceRowBottom; 
    private TextMeshProUGUI _hpText;
    public int Health
    {
        get { return _health; }
        set
        {
            if (value >= 0)
            {
                _health = value;
            }
            else
            {
                _health = 0;
            }

            _hpText.text = Health.ToString();
        }
    }

    public int DefenceTop
    {
        get { return _defenceTop; }
        set
        {
            _defenceTop= value;
            UpdateDefence(DefenceRow.Top, value);
        }
    }

    public int DefenceMiddle
    {
        get { return _defenceMiddle; }
        set
        {
            _defenceMiddle = value;
            UpdateDefence(DefenceRow.Middle, value);
        }
    }


    public int DefenceBottom
    {
        get { return _defenceBottom; }
        set
        {
            _defenceBottom = value;
            UpdateDefence(DefenceRow.Bottom, value);
        }
    }


    // Start is called before the first frame update
    void Start()
    {

        //Init
        _hpText = this.transform.Find("HpCanvas").Find("HpText").gameObject.GetComponent<TextMeshProUGUI>();
        _defenceRowTop = this.transform.Find("DefenceRowTop").gameObject;
        _defenceRowMiddle = this.transform.Find("DefenceRowMiddle").gameObject;
        _defenceRowBottom = this.transform.Find("DefenceRowBottom").gameObject;

        //Set starting values
        Health = StartingHealth;
        DefenceTop = StartingDefence;
        DefenceMiddle = StartingDefence;
        DefenceBottom = StartingDefence;
        _hpText.text = Health.ToString();
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        CombatEventManager.DealDamageEvent += TakeDamage;
    }

    private void OnDisable()
    {
        CombatEventManager.DealDamageEvent -= TakeDamage;
    }

    private void UpdateDefence(DefenceRow defenceRow, int defenceAmount)
    {
        GameObject defenceRowObject = null;
        switch (defenceRow)
        {
            case DefenceRow.Top:
                defenceRowObject = _defenceRowTop;
                break;
            case DefenceRow.Middle:
                defenceRowObject = _defenceRowMiddle;
                break;
            case DefenceRow.Bottom:
                defenceRowObject = _defenceRowBottom;
                break;
            default:
                break;
        }

        for (int i = 0; i < 10; i++)
        {
            if (i < defenceAmount)
            {
                defenceRowObject.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                defenceRowObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void TakeDamage(DefenceRow defenceRow, int amountOfDamage)
    {
        int defence = 0;
        switch (defenceRow)
        {
            case DefenceRow.Top:
                defence = DefenceTop;
                break;
            case DefenceRow.Middle:
                defence = DefenceMiddle;
                break;
            case DefenceRow.Bottom:
                defence = DefenceBottom;
                break;
            default:
                break;
        }

        if (defence > 0)
        {
            if (defence >= amountOfDamage)
            {
                defence -= amountOfDamage;  
            }
            else
            {
                int unresolvedDamage = amountOfDamage - defence;
                defence = 0;
                Health -= unresolvedDamage;
            }
        }
        else
        {
            Health -= amountOfDamage;
        }

        switch (defenceRow)
        {
            case DefenceRow.Top:
                DefenceTop = defence;
                break;
            case DefenceRow.Middle:
                DefenceMiddle = defence;
                break;
            case DefenceRow.Bottom:
                DefenceBottom = defence;
                break;
            default:
                break;
        }
    }

    public void Reset()
    {
        Health = StartingHealth;
        DefenceTop = StartingDefence;
        DefenceMiddle = StartingDefence;
        DefenceBottom = StartingDefence;
    }


    public enum DefenceRow
    {
        Top = 1,
        Middle,
        Bottom
    }


}
