using UnityEngine;
using System.Collections;

public class DBLMath
{
    public static uint Pow(uint b, int exp){
        uint total = 1;
        for (int i = 0; i < exp; i++){
            total *= b;
        }
        return total;
    }
}
