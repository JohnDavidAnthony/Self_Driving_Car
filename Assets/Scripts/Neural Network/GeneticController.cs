using UnityEngine;
using System.Collections.Generic;

public class GeneticController 
{
    public List<NeuralNetwork> population;
    public List<NeuralNetwork> nextGeneration;
    private double populationFitness;
    public float mutationRate;
    public float averageFitness;
    public float bestFitness;
    int popSize;

    // Constructor creates randomly weighted neural networks
    public GeneticController(int popSize, float mutationRate){
        this.population = new List<NeuralNetwork>(popSize);
        this.populationFitness = 0f;
        this.mutationRate = mutationRate;
        this.averageFitness = 0f;
        this.popSize = popSize;
        bestFitness = 0;

        for (int i = 0; i < popSize; i++){
            // Create NN with specific structure
            this.population.Add(new NeuralNetwork(new int[] {6,5,2}));
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
        NeuralNetwork child1 = new NeuralNetwork(mother.layerStructure);
        NeuralNetwork child2 = new NeuralNetwork(mother.layerStructure);

        List<double> motherChromosome = mother.Encode();
        List<double> fatherChromosome = father.Encode();

        Crossover(motherChromosome, fatherChromosome);

        child1.Decode(motherChromosome);
        child2.Decode(fatherChromosome);

        return new NeuralNetwork[] {child1, child2};
    }

    // Mutates creatire
    public void Mutate(NeuralNetwork creature){
        List<double> chromosome = creature.Encode();
        // for each gene in creature mutate with small chance
        for (int i = 0; i < chromosome.Count; i++) {
            if (this.mutationRate > UnityEngine.Random.Range(0f, 1f)){
                Debug.Log("Mutate");
                chromosome[i] = UnityEngine.Random.Range(-1f, 1f);
            }
        }

        creature.Decode(chromosome);

    }

    // Creates next generation through Roulette wheel selection
    public void NextGeneration(){
        // Create a new generation 
        this.nextGeneration = new List<NeuralNetwork>();
        this.populationFitness = 0f;

        // Calcualte population fitness
        for (int i = 0; i < this.population.Count; i++){
            this.populationFitness += population[i].fitness;
        }

        // Calcualte fitness ratio for each population member
        for (int i = 0; i < this.population.Count; i++){
            population[i].fitnessRatio = population[i].fitness / this.populationFitness;
        }

        // Calcuate the average fitness of the population
        averageFitness = (float)(this.populationFitness / this.population.Count);

        // Sort population list by fitness ratio
        population.Sort((x, y) => y.fitnessRatio.CompareTo(x.fitnessRatio));

        // Create 2 children for every breeding of NN
        for (int i = 0; i < this.population.Count / 2; i++){
            // Select parents to breed
            int parent1Index = -1;
            int parent2Index = -1;
            double chance = UnityEngine.Random.Range(0f, 100f) / 100;
            double chance2 = UnityEngine.Random.Range(0f, 100f) / 100;
            double range = 0;

            //Debug.Log("Chance1: " + chance);
            //Debug.Log("Chance2: " + chance2);
            for (int j = 0; j < this.population.Count; j++){
                
                range += population[j].fitnessRatio;
                //Debug.Log("Fitness ratio: " + population[j].fitnessRatio);
                //Debug.Log("Range: " + range);
                // This creature isnt selected move on
                if (chance > range && chance2 > range){
                    continue;
                }
                // At this point one of the parents been selected

                if (chance <= range && parent1Index < 0){ // Parent 1 selected
                    parent1Index = j;
                }

                if (chance2 <= range && parent2Index < 0){ // Parent 2 selected
                    // avoid two of the same parent
                    if (parent1Index == j){
                        // Parent 2 is the next availible parent
                        parent2Index = (j + 1) % population.Count;
                    }
                    else{
                        parent2Index = j;
                    }
                }
                if (parent1Index >= 0 && parent2Index >= 0){
                    break;

                }
            }
            // If somehow we have no parents chosen choose the worst parent
            if (parent1Index < 0){
                parent1Index = this.population.Count-1;
            }
            if (parent2Index < 0)
            {
                parent2Index = this.population.Count - 1;
            }

            //Breed the two selected parents and add them to the next generation
            Debug.Log("Breeding: " + parent1Index + " with fitness " + population[parent1Index].fitness);
            Debug.Log("and " + parent2Index + " with fitness " + population[parent2Index].fitness);

            NeuralNetwork[] children = Breed(population[parent1Index], population[parent2Index]);

            // Mutate children
            Mutate(children[0]);
            Mutate(children[1]);

            // Add the children to the next generation
            nextGeneration.Add(children[0]);
            nextGeneration.Add(children[1]);
        } //  End foor loop -- Breeding

        // Make the children adults
        for (int i = 0; i < popSize; i++){
            population[i] = nextGeneration[i];
        }
    }// End NextGeneration()



}
