using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Dendrite
{
    public double weight;

    public Dendrite(){
        weight = UnityEngine.Random.Range(0.0f, 1.0f);
    }

}
