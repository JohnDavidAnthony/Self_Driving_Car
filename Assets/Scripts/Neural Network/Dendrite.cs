using System;

[Serializable]
public class Dendrite
{
    public double weight;

    public Dendrite(){
        CryptoRandom n = new CryptoRandom();
        this.weight = n.RandomValue;
    }

}
