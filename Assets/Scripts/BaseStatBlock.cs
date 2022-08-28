using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStatBlock
{
	private int _health;
	private int _strength;
	private int _dexterity;
	private int _intelligence;

	public BaseStatBlock(int health, int strength, int dexterity, int intelligence)
	{
		_health = health;
		_strength = strength;
		_dexterity = dexterity;
		_intelligence = intelligence;
	}

    public int GetBaseStrength()
    {
		return _strength;
    }

    public int GetBaseDexterity()
	{
		return _dexterity;
	}

	public int GetBaseIntelligence()
	{
		return _intelligence;
	}

	public int GetBaseHealth()
	{
		return _health;
	}
}
