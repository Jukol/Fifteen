using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell3 : MonoBehaviour
{
    [SerializeField]
    private int _myNumber;
    public int MyNumber
    {
        get
        {
            return _myNumber;
        }
    }
}
