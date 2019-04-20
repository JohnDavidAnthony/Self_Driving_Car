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
    public float distanceTraveled;
    public float avgSpeed;
    private float timeElapsed;
    private float avgSensor;



    public CarController controller;
    public CheckPointScript checkpoints;
    public int currentGenome;
    public int currentGeneration;
    public float overallFitness;
    public float lastGenAvgFitness;

    float lastCheckpointDistance;
    float timer;
    bool ready;
    float movement;

    // Use this for initialization
    void Start(){
        // Create new species with specified genetics
        species = new GeneticController(30, .8f);
        currentGenome = 0;
        currentGeneration = 1;
        timer = 60f;
        ready = true;
        timeElapsed = .0001f;
        lastPosition = this.controller.car.position;
        lastCheckpointDistance = controller.carCheckPoint.distanceToCheckpoint;
        lastGenAvgFitness = 0f;
    }

    // Update is called once per frame
    void Update(){
        if (!ready){
        }
        // Get input from environmemnt
        List<double> input = new List<double>();
        input.Add(controller.leftSensor.hitNormal);
        input.Add(controller.frontLeftSensor.hitNormal);
        input.Add(controller.frontSensor.hitNormal);
        input.Add(controller.frontRightSensor.hitNormal);
        input.Add(controller.rightSensor.hitNormal);
        input.Add(controller.speedNormal);
       
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
        //if (controller.hitCheckPoint){
        //    species.population[currentGenome].fitness += 5;
        //    controller.hitCheckPoint = false;
        //}

        lastGenAvgFitness = species.averageFitness;


    }

    private void CalculateFitness(){
        // Check to see if car is moving away from checkpoint
        movement = Vector3.Distance(controller.car.position, lastPosition) * -1;
        if (controller.carCheckPoint.distanceToCheckpoint < lastCheckpointDistance){
            movement *= -1;
        }

        distanceTraveled += movement;
        avgSensor = (controller.leftSensor.hitNormal + controller.frontLeftSensor.hitNormal + controller.frontSensor.hitNormal + controller.frontRightSensor.hitNormal + controller.rightSensor.hitNormal) * movement / 5;

        lastCheckpointDistance = controller.carCheckPoint.distanceToCheckpoint;
        lastPosition = controller.car.position;
        timeElapsed += Time.deltaTime / Time.timeScale;
        avgSpeed = distanceTraveled / timeElapsed;
        avgSensor = (controller.leftSensor.distance + controller.frontLeftSensor.distance + controller.frontSensor.distance + controller.frontRightSensor.distance + controller.rightSensor.distance) / 5;
        overallFitness = (distanceTraveled * distanceMultiplier) + (avgSpeed * speedMultiplyer) + (avgSensor * sensorMultiplyer);
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
        avgSpeed = 0;
        lastCheckpointDistance = controller.carCheckPoint.distanceToCheckpoint;

        // Reset Timer
        timer = 60f;
        ready = true;
    }


}
