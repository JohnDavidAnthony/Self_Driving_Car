using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Neuron
{
    // our inputs (dendrites)
    public List<Dendrite> dendrites;
    // Neuron attributes
    public double bias;
    public double delta;
    public double value;

    // Function to get count of how many inputs we have
    public int NumDendrites(){
        return dendrites.Count;
    }

    // Constructor
    public Neuron(){
        this.bias = UnityEngine.Random.Range(-.5f, .5f);
        this.dendrites = new List<Dendrite>();
    }
    // Constructor
    public Neuron(int bias){
        this.bias = bias;
        this.dendrites = new List<Dendrite>();
    }

}
