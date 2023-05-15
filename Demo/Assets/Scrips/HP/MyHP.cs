using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHP : MonoBehaviour, IHpBarInterface
{
    public int _currentHP = 100;
    public int currentHP
    {
        get
        {
            return _currentHP;
        }
        set
        {
            if (value > 100)
            {
                _currentHP = 100;
            }
            else if (value < 0)
            {
                _currentHP = 0;
            }
            else
            {
                _currentHP = value;
            }
        }
    }

    public int HP()
    {
        return currentHP;
    }
}

