using UnityEngine;
using System.Collections.Generic;
using System;

public class NeuralNetwork : MonoBehaviour
{
    public List<Layer> layers;
    public double learningRate;
    // The number of neurons in each layer
    public int[] layerStructure;

    public int NumLayers(){
        return layers.Count;

    }

    // Constructor
    public NeuralNetwork(double learningRate, int[] layers){
        // We must have at least 2 layers (input, output)
        if (layers.Length < 2){
            return;
        }

        this.learningRate = learningRate;
        this.layers = new List<Layer>();
        this.layerStructure = layers;

        // initalize our NN with layers of neurons
        for (int i = 0; i < layers.Length; i++){
            // Make a new layer with right number of neurons
            Layer currentLayer = new Layer(layers[i]);
            this.layers.Add(currentLayer);

            // Add neurons to current layer
            for (int n = 0; n < layers[i]; n++){
                currentLayer.neurons.Add(new Neuron());
            }

            // initalize neurons
            foreach(Neuron neuron in currentLayer.neurons){
                // if we are the first layer set our bias to 0
                if (i == 0){
                    neuron.bias = 0;
                }else{
                    if (i == layers.Length - 1){ // Last layer set bias to .5 --- Only for my setup remove otherwise
                        neuron.bias = .5;
                    }

                    // For each neuron create dendrite to other neurons
                    for (int d = 0; d < layers[i - 1]; d++){
                        neuron.dendrites.Add(new Dendrite());
                    }
                }
            }
        }
    }// End Cosntructor

    // Define signmoid activation fxn
    public double Sigmoid(double x){
        return 1 / (1 + Math.Exp(-x));
    }

    // Encode Neural network into a chromosome that we can evolve
    public List<double> Encode(){
        //// Calculate how long our chromosome will be
        //int lengthList = 0;
        //// First (input) layer we don't include bias or weights
        //// Every other layer (except output) we include bias + weight
        //for (int i = 1; i < this.layerStructure.Length - 1; i++){
        //    // Add room for biases
        //    lengthList += this.layerStructure[i];
        //    // Add room for weights
        //    lengthList += this.layerStructure[i - 1] * this.layerStructure[i];
        //}
        //// Last (output) we add room for its biases
        //lengthList += this.layerStructure[this.layerStructure.Length - 1];

        List<double> chromosome = new List<double>();
        // Get data from NN for chromosome
        for (int i = 1; i < layers.Count; i++){
            // last layer
            for (int j = 0; j < layers[i].neurons.Count; j++){
                chromosome.Add(layers[i].neurons[j].bias);
                // Add each weight input to the neuron
                for (int k = 0; k < layers[i].neurons[j].NumDendrites(); k++){
                    chromosome.Add(layers[i].neurons[j].dendrites[k].weight);
                }
            }
        }
        return chromosome;
    }
}
