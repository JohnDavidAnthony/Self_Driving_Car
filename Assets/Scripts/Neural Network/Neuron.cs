using UnityEngine;
using System.Collections.Generic;

public class Neuron : MonoBehaviour
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
        this.bias = Random.Range(0f, 1f);
        this.dendrites = new List<Dendrite>();
    }
    // Constructor
    public Neuron(int bias){
        this.bias = bias;
        this.dendrites = new List<Dendrite>();
    }

}
