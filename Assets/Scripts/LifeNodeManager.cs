using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LifeNodeManager : MonoBehaviour
{

    public int StartingHealth;
    public int StartingDefence;


    private int _health = 0;
    private int _defence;
    private GameObject _defenceRow;
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

    public int Defence
    {
        get { return _defence; }
        set
        {
            _defence = value;
            UpdateDefence();
        }
    }


    // Start is called before the first frame update
    void Start()
    {

        //Init
        _hpText = this.transform.Find("HpCanvas").Find("HpText").gameObject.GetComponent<TextMeshProUGUI>();
        _defenceRow = this.transform.Find("DefenceRow").gameObject;

        //Set starting values
        Health = StartingHealth;
        Defence = StartingDefence;
        _hpText.text = Health.ToString();



    }


    // Update is called once per frame
    void Update()
    {

    }


    private void UpdateDefence()
    {
        for (int i = 0; i < 10; i++)
        {
            if (i < Defence)
            {
                _defenceRow.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                _defenceRow.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (Defence > 0)
        {
            if (Defence >= amount)
            {
                Defence -= amount;  
            }
            else
            {
                int unresolvedDamage = amount - Defence;
                Defence = 0;
                Health -= unresolvedDamage;
            }
        }
        else
        {
            Health -= amount;
        }
    }

    public void Reset()
    {
        Health = StartingHealth;
        Defence = StartingDefence;
    }


}
