using NUnit.Framework.Internal;
using System.Security.Cryptography;
using System;
using System.ComponentModel;

public class DBLMath
{
    private static RandomNumberGenerator random = new RNGCryptoServiceProvider();

    public static uint Pow(uint b, int exp){
        uint total = 1;
        for (int i = 0; i < exp; i++){
            total *= b;
        }
        return total;
    }

    public static ulong GenerateRandomInt64(){
        byte[] bytes = new byte[8];
        random.GetBytes(bytes);
        return BitConverter.ToUInt64(bytes, 0);
    }
}
