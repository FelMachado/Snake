using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Libraries_Formulas : MonoBehaviour
{
    public static int GetDirection(int num1, int num2)
    {
        if (num1 > num2)
            return -1;
        else
            return 1;
    }
}
