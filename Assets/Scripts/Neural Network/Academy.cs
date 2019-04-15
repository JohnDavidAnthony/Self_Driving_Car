using UnityEngine;
using System.Collections.Generic;

public class Academy
{
    public List<NeuralNetwork> population;
    public List<NeuralNetwork> nextGeneration;
    private double populationFitness;
    public float mutationRate;

    // Constructor creates randomly weighted neural networks
    public Academy(int popSize, float mutationRate){
        this.population = new List<NeuralNetwork>(popSize);
        this.nextGeneration = new List<NeuralNetwork>();
        this.populationFitness = 0f;
        this.mutationRate = mutationRate;

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

    // Crossover two Neural networks and return 2 new neural network children
    public NeuralNetwork[] Breed(NeuralNetwork mother, NeuralNetwork father){
        NeuralNetwork child1 = new NeuralNetwork(mother.learningRate, mother.layerStructure);
        NeuralNetwork child2 = new NeuralNetwork(mother.learningRate, mother.layerStructure);
        List<double> motherChromosome = mother.Encode();
        List<double> fatherChromosome = father.Encode();

        Crossover(motherChromosome, fatherChromosome);

        child1.Decode(motherChromosome);
        Debug.Log("Child1: ");
        foreach (var item in motherChromosome)
            Debug.Log(item);
        child2.Decode(fatherChromosome);
        Debug.Log("Child2: ");
        foreach (var item in fatherChromosome)
            Debug.Log(item);

        return new NeuralNetwork[] {child1, child2};
    }

    // Mutates a single gene in a creature
    public void Mutate(NeuralNetwork creature){
        List<double> chromosome = creature.Encode();
        int geneIndex = Random.Range(0, chromosome.Count);
        //chromosome[geneIndex] = Random.Range(0f, 1f);
        chromosome[0] = 100f;
        creature.Decode(chromosome);

    }

    // Creates next generation through Roulette wheel selection
    public void NextGeneration(){
        // Calcualte population fitness
        for (int i = 0; i < this.population.Count; i++){
            this.populationFitness += population[i].fitness;

        }
        // Calcualte fitness ratio for each population member
        for (int i = 0; i < this.population.Count; i++){
            population[i].fitnessRatio = population[i].fitness / this.populationFitness;
            Debug.Log(i + " Fitness: " + population[i].fitness);
            Debug.Log("Population fitness: " + this.populationFitness);

        }
        // Sort population list by fitness ratio
        population.Sort((x, y) => y.fitnessRatio.CompareTo(x.fitnessRatio));


        // Create 2 children for every breeding of NN
        for (int i = 0; i < this.population.Count / 2; i++){
            // Select parents to breed
            int parent1Index = -1;
            int parent2Index = -1;
            double chance = Random.Range(0f, 100f) / 100;
            double chance2 = Random.Range(0f, 100f) / 100;
            double range = 0;

            Debug.Log("Chance1: " + chance);
            Debug.Log("Chance2: " + chance2);
            for (int j = 0; j < this.population.Count; j++){
                Debug.Log("Current Loop: " + j);
                
                range += population[j].fitnessRatio;
                Debug.Log("Fitness ratio: " + population[j].fitnessRatio);
                Debug.Log("Range: " + range);
                // This creature isnt selected move on
                if (chance > range && chance2 > range){
                    continue;
                }
                // At this point one of the parents been selected
                // Parent 1 selected
                if (chance <= range && parent1Index < 0){
                    parent1Index = j;
                }
                // Parent 2 selected
                if (chance2 <= range && parent2Index < 0){
                    parent2Index = j;
                }
                if (parent1Index >= 0 && parent2Index >= 0){
                    break;

                }
            }
            if (parent1Index < 0){
                parent1Index = this.population.Count-1;
            }
            if (parent2Index < 0)
            {
                parent2Index = this.population.Count - 1;
            }

            Debug.Log("Parent1: " + parent1Index);
            Debug.Log("Parent2: " + parent2Index);

            //Breed the two selected parents and add them to the next generation
            NeuralNetwork[] children = Breed(population[parent1Index], population[parent2Index]);

            // Mutate 1st child chance
            chance = Random.Range(0f, 1f);
            if (chance < this.mutationRate){
                Mutate(children[0]);
            }
            // Mutate 2nd child chance
            chance = Random.Range(0f, 1f);
            if (chance < this.mutationRate)
            {
                Mutate(children[1]);
            }

            // Add the children to the next generation
            nextGeneration.Add(children[0]);
            nextGeneration.Add(children[1]);
        }


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
