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
    GeneticController species;
    // Start is called before the first frame update
    void Start()
    {
        // create our neural networks for the genomes
        species = new GeneticController(numGenomes, mutationRate);
        cars = new GameObject[numSimulate];

        CarCheckPoint checkpoint = carFab.GetComponent<CarCheckPoint>();
        checkpoint.track = track;

        // Instantiate the number of cars we are going to simulate
        // Add assign them an AI Controller

        for (int i = 0; i < numSimulate; i++){
            cars[i] = Instantiate(carFab, startingPos, carFab.transform.rotation);
            AICarController controller = cars[i].GetComponent<AICarController>();
            controller.network = species.population[i];         
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
