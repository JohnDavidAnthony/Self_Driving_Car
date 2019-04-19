using System;
using UnityEngine;

[Serializable]
public class Dendrite
{
    public double weight;

    public Dendrite(){ 
        this.weight = UnityEngine.Random.Range(-1f, 1f);
    }

}
