using UnityEngine;
using System.Collections.Generic;

public class Academy : MonoBehaviour
{
    public List<NeuralNetwork> population;

    // Constructor creates randomly weighted neural networks
    public Academy(int popSize){
        this.population = new List<NeuralNetwork>(popSize);

        for (int i = 0; i < popSize; i++){
            // Create NN with specific structure
            this.population.Add(new NeuralNetwork(.5,new int[] {6,5,2}));
        }
    }

    public void Breed(NeuralNetwork mother, NeuralNetwork father){


    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
