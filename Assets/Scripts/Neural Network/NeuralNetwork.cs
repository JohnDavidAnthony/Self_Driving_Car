using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class NeuralNetwork
{
    public List<Layer> layers;
    public double learningRate;
    // The number of neurons in each layer
    public int[] layerStructure;
    public float fitness;
    public double fitnessRatio;

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
        this.fitness = 0f;
        this.fitnessRatio = 0f;

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
                        //neuron.bias = 0;
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
    double tanh(double x)
    {
        return System.Math.Tanh(x);
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
            for (int j = 0; j < layers[i].neurons.Count; j++){
                // Add the neruon's bias to the chromosome
                chromosome.Add(layers[i].neurons[j].bias);
                // Add each weight input to the chromosome
                for (int k = 0; k < layers[i].neurons[j].NumDendrites(); k++){
                    chromosome.Add(layers[i].neurons[j].dendrites[k].weight);
                }
            }
        }
        return chromosome;
    }

    // Apply new chromosome to NN
    public void Decode(List<double> chromosome){
        int geneIndex = 0;

        for (int i = 1; i < layers.Count; i++){
            for (int j = 0; j < layers[i].neurons.Count; j++){
                layers[i].neurons[j].bias = chromosome[geneIndex];
                geneIndex++;
                // Add each weight input to the neuron
                for (int k = 0; k < layers[i].neurons[j].NumDendrites(); k++){
                    layers[i].neurons[j].dendrites[k].weight = chromosome[geneIndex];
                    geneIndex++;
                }
            }
        }

    }

    // Run the NN
    public double[] run(List<double> input){
        // Check to see if input is right size
        if (input.Count != this.layers[0].neurons.Count){
            return null;
        }

        // Pass input through each layer of network
        for (int l = 0; l < layers.Count; l++){
            Layer currentLayer = layers[l];

            for (int n = 0; n < currentLayer.neurons.Count; n++){
                Neuron neuron = currentLayer.neurons[n];

                // if first layer pass in input
                if (l == 0){
                    neuron.value = input[n];
                }else{ // Get input from neurons before it and apply weights
                    neuron.value = 0;
                    for (int lastNeuron = 0; lastNeuron < this.layers[l - 1].neurons.Count; lastNeuron++){
                        neuron.value += this.layers[l - 1].neurons[lastNeuron].value * neuron.dendrites[lastNeuron].weight;
                    }

                    // Call activation fxn
                    if (l != layers.Count - 1){
                        neuron.value = tanh(neuron.value + neuron.bias);
                    }
                    else{
                        neuron.value = tanh(neuron.value + neuron.bias);
                    }

                } // End if

            }// End inner for

        }// End outter for

        // Get output layer
        Layer lastLayer = this.layers[this.layers.Count - 1];
        int numOutput = lastLayer.neurons.Count;
        double[] output = new double[numOutput];
        // Add output to layer
        for (int i = 0; i < numOutput; i++){
            output[i] = lastLayer.neurons[i].value;
        }
        return output;

    }// End run

}

