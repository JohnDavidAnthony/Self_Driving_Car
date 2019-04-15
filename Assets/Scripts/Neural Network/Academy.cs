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

    // Cross over the chromosomes
    public void Crossover(List<double> mother, List<double> father){
        int middle = Mathf.FloorToInt(mother.Count / 2);
        List<double> tempM = new List<double>();
        List<double> tempF = new List<double>();
        for (int i = 0; i < middle; i++){
            tempM.Add(mother[i]);
            tempF.Add(father[i]);
        }
        mother.RemoveRange(0, middle);
        father.RemoveRange(0, middle);

        mother.InsertRange(0, tempF);
        father.InsertRange(0, tempM);

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
