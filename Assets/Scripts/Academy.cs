using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Academy : MonoBehaviour
{
    public int numGenomes;
    public int numSimulate;
    public float mutationRate;
    public Vector3 startingPos;
    public GameObject carFab;
    public TrackScript track;
    GameObject[] cars;
    AICarController[] carController;
    public GeneticController species;
    public TargetCamera bestCamera;

    public int currentGenome;
    public float bestGenomeFitness;
    public int batchSimulate;
    public int currentGeneration = 1;
    public AICarController bestCar;
    public GameObject Network_GUI;
    private UI_Network networkUI;
   
    void Start()
    {
        // create our neural networks for the genomes
        species = new GeneticController(numGenomes, mutationRate);
        cars = new GameObject[numSimulate];
        carController = new AICarController[numSimulate];
        bestCar = carController[0];

        CarCheckPoint checkpoint = carFab.GetComponent<CarCheckPoint>();
        checkpoint.track = track;

        // Instantiate the number of cars we are going to simulate
        // Add assign them an AI Controller

        for (int i = 0; i < numSimulate; i++){
            cars[i] = Instantiate(carFab, startingPos, carFab.transform.rotation);
            carController[i] = cars[i].GetComponent<AICarController>();
            carController[i].network = species.population[i];         
        }

        currentGenome = numSimulate;
        batchSimulate = numSimulate;

        Network_GUI = Instantiate(Network_GUI);
        UI_Genetics genetics = Network_GUI.GetComponentInChildren<UI_Genetics>();
        genetics.academy = this;
        networkUI = Network_GUI.GetComponentInChildren<UI_Network>();
    }

    void Update(){
        //check to see if any cars are still alive
        float bestCarFitness = 0;
        bool allCarsDead = true;
        foreach (AICarController car in carController){
            if (car.alive) {
                allCarsDead = false;
                // Find the best car that is alive
                if (car.overallFitness > bestCarFitness){
                    bestCarFitness = car.overallFitness;
                    bestCamera.target = car.transform;
                    bestCar = car;
                    if (bestCar.overallFitness > bestGenomeFitness){
                        bestGenomeFitness = bestCar.overallFitness;
                        networkUI.Display(bestCar.network);
                    }
                }

            }
        }

        if (allCarsDead){ // Simulate next batch or get next Generation
            Debug.Log("All Cars Dead");
            // If we have simualted all genomes, reset and get next gen
            if (currentGenome == numGenomes)
            {
                Debug.Log("New Gen");
                species.NextGeneration();
                for (int i = 0; i < numSimulate; i++)
                {
                    carController[i].network = species.population[i];
                    carController[i].Reset();
                }
                currentGeneration++;
                currentGenome = numSimulate;
            }
            else // We still need to simualte another batch
            {
                if (currentGenome + numSimulate <= numGenomes)
                {
                    Debug.Log("Full Sim");
                    batchSimulate = numSimulate;
                }
                else
                {
                    Debug.Log("Partial Sim");
                    batchSimulate = numGenomes - currentGenome;
                }

                for (int i = 0; i < batchSimulate; i++)
                {
                    carController[i].network = species.population[currentGenome + i];
                    carController[i].Reset();
                }
                currentGenome += batchSimulate;
            }
        }
    }
}
