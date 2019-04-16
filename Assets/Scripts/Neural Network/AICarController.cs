using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AICarController : MonoBehaviour
{
    GeneticController species;
    public float distanceMultiplier = 1.4f;
    public float speedMultiplyer = .4f;
    // How important it is to stay in the middle of the track
    public float sensorMultiplyer = .05f;
    private Vector3 lastPosition;
    private float distanceTraveled;
    private float avgSpeed;
    private float timeElapsed;
    private float avgSensor;



    public CarController controller;
    public int currentGenome;
    public int currentGeneration;
    public float overallFitness;
    float timer;
    bool ready;

    // Use this for initialization
    void Start(){
        // Create new species with specified genetics
        species = new GeneticController(30, .5f);
        currentGenome = 0;
        currentGeneration = 1;
        timer = 60f;
        ready = true;
        timeElapsed = .0001f;
    }

    // Update is called once per frame
    void Update(){
        if (!ready){
        }
        // Get input from environmemnt
        List<double> input = new List<double>();
        input.Add(controller.leftSensor.distance);
        input.Add(controller.frontLeftSensor.distance);
        input.Add(controller.frontSensor.distance);
        input.Add(controller.frontRightSensor.distance);
        input.Add(controller.rightSensor.distance);
        input.Add(controller.car.velocity.magnitude);
       
        // Apply input to NN
        double[] output = species.population[currentGenome].run(input);

        // Apply output to environment
        controller.carTurn = output[0];
        controller.carDrive = output[1];

        CalculateFitness();

        // Check to see if player is out of time or hit wall 
        timer -= Time.deltaTime;
        if (timer < 0){
            Reset();
        }
        if (controller.playerHitWall){
            // Apply penalty for hitting wall
            //species.population[currentGenome].fitness -= 1000;
            Reset();
        }
        if (controller.playerStopped){
            //Apply penalty for standing still
            //species.population[currentGenome].fitness -= 1500;
            Reset();
        }
        // Check to see if player hit checkpoint
        if (controller.hitCheckPoint){
            species.population[currentGenome].fitness += 5;
            controller.hitCheckPoint = false;
        }




    }

    private void CalculateFitness(){
        distanceTraveled += Vector3.Distance(controller.car.position, lastPosition);
        lastPosition = controller.car.position;
        timeElapsed += Time.deltaTime/Time.timeScale;
        avgSpeed = distanceTraveled / timeElapsed;
        avgSensor = (controller.leftSensor.distance + controller.frontLeftSensor.distance + controller.frontSensor.distance + controller.frontRightSensor.distance + controller.rightSensor.distance) / 5;
        overallFitness = (distanceTraveled * distanceMultiplier) + (avgSpeed * speedMultiplyer);
    }

    void Reset(){
        ready = false;

        species.population[currentGenome].fitness += overallFitness;
        Debug.Log("Genome: " + this.currentGenome + " ended with fitness: " + species.population[currentGenome].fitness);

        this.currentGenome = (currentGenome + 1) % species.population.Count;
        Debug.Log("Current Genome: " + this.currentGenome);
        // We've gone through each genome, apply genetic algorithm 
        if (this.currentGenome == 0){
            currentGeneration++;
            species.NextGeneration();
            overallFitness = species.averageFitness;
        }
        // Reset car position
        species.population[currentGenome].fitness = 0;
        this.controller.ResetPosition();
        lastPosition = this.controller.car.position;
        distanceTraveled = 0;
        avgSpeed = 0; ;

    // Reset Timer
    timer = 60f;
        ready = true;
    }


}
