using System;
using System.Security.Cryptography;

public class CryptoRandom
{
    public double RandomValue { get; set; }
 
    public CryptoRandom()
    {
        using (RNGCryptoServiceProvider p = new RNGCryptoServiceProvider())
        {
            Random r = new Random(p.GetHashCode());
            this.RandomValue = r.NextDouble();
        }
    }
 
}