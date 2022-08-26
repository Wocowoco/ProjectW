using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LifeNodeManager;

public class CombatEventManager : MonoBehaviour
{
    // Start is called before the first frame update

    public delegate void DamageEvent(DefenceRow defenceRow, int damageAmount);
    public static event DamageEvent DealDamageEvent;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void DealDamage(DefenceRow defenceRow, int damageAmount)
    {
        //Check if there are any subscribers on damageEvent before invoking it
        if (DealDamageEvent != null)
        {
            DealDamageEvent.Invoke(defenceRow, damageAmount);
        }
    }
}
