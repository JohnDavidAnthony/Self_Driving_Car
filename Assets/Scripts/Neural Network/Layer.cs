using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Layer
{
    public List<Neuron> neurons;
    private int layerIndex;
    public object Neurons { get; internal set; }

    public int NumNeurons(){
        return neurons.Count;
    }

    // Initalize our layer with the correct number of neurons
    public Layer(int numNeurons, int layerIndex){
        this.neurons = new List<Neuron>(numNeurons);
        this.layerIndex = layerIndex;

    }

    // Return an array of the wights for this layer
    public double[,] GetWeights(NeuralNetwork network)
    {
        Layer nextLayer = network.layers[layerIndex + 1];
        double[,] weights = new double[NumNeurons(),nextLayer.NumNeurons()];
        for (int i = 0; i < NumNeurons(); i++)
        {
            for (int j = 0; j < nextLayer.neurons.Count; j++){
                weights[i, j] = nextLayer.neurons[j].dendrites[i].weight;
            }
        }
        return weights;
    }

}
