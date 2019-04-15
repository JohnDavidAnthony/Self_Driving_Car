using UnityEngine;
using System.Collections.Generic;

public class Layer : MonoBehaviour
{
    public List<Neuron> neurons;

    public int NumNeurons(){
        return neurons.Count;
    }

    // Initalize our layer with the correct number of neurons
    public Layer(int numNeurons){
        this.neurons = new List<Neuron>(numNeurons);

    }
}
