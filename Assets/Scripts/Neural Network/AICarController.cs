using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AICarController : MonoBehaviour
{
    // Our genetic controller controls the genetics of the species
    GeneticController species;

    // Fitness function
    public float distanceMultiplier = 2f;
    public float speedMultiplyer = .6f;
    public float sensorMultiplyer = .4f; // How important it is to stay in the middle of the track

    // Car properties
    public CarController controller;
    public CheckPointScript checkpoints;
    private Vector3 lastPosition;
    public float distanceTraveled;
    public float avgSpeed;
    public float timeElapsed;
    private float avgSensor;
    float timer = 0;
    public float bestFitness = 0;
    public float bestPopFitness = 0;

    // Genetic Properties
    public int currentGenome;
    public int currentGeneration;
    public float overallFitness;
    public float lastGenAvgFitness;

    // Tell if we are moving backwards
    float lastCheckpointDistance;
    // How far we have moved since last checked
    float movement;

    void Start(){
        // Create new species with specified genetics
        species = new GeneticController(200, .05f);
        currentGenome = 0;
        currentGeneration = 1;
        timeElapsed = 0;
        lastPosition = this.controller.car.position;
        lastCheckpointDistance = controller.carCheckPoint.distanceToCheckpoint;
        lastGenAvgFitness = 0f;
    }

    void Update(){
        // Get input from environmemnt
        List<double> input = new List<double>();
        input.Add(controller.leftSensor.hitNormal);
        input.Add(controller.frontLeftSensor.hitNormal);
        input.Add(controller.frontSensor.hitNormal);
        input.Add(controller.frontRightSensor.hitNormal);
        input.Add(controller.rightSensor.hitNormal);
        input.Add(controller.speed/controller.acceleration);
       
        // Apply input to NN
        double[] output = species.population[currentGenome].Run(input);

        // Apply output to environment
        controller.carTurn = (float)output[0];
        controller.carDrive = (float)output[1];

        CalculateFitness();

        // Check to see if player hit wall or stopped progressing
        // If fitness stopped improving end creature
        if (bestFitness > overallFitness){
            timer -= Time.deltaTime;
            if (timer <= 0){
                Reset();
            }
        }else{
            bestFitness = overallFitness;
            timer = 10f;
        }

        if (controller.playerHitWall){
            Reset();
        }
        if (controller.playerStopped){
            Reset();
        }


    }

    private void CalculateFitness(){
        // Check to see if car is moving away from checkpoint
        movement = Vector3.Distance(controller.car.position, lastPosition) * -1;
        if (controller.carCheckPoint.distanceToCheckpoint < lastCheckpointDistance){
            movement *= -1;
        }

        // Calcualte distance travelled
        distanceTraveled += movement;

        // Calculate avg sensor value
        avgSensor = (controller.leftSensor.hitNormal +
            controller.frontLeftSensor.hitNormal + 
            controller.frontSensor.hitNormal 
            + controller.frontRightSensor.hitNormal + 
            controller.rightSensor.hitNormal) * movement / 5;

        // Update last positions
        lastCheckpointDistance = controller.carCheckPoint.distanceToCheckpoint;
        lastPosition = controller.car.position;

        // Calcualte avg speed
        timeElapsed += Time.deltaTime ;
        avgSpeed = distanceTraveled / timeElapsed;

        // Calcualte overall fitness
        overallFitness = (distanceTraveled * distanceMultiplier) + (avgSpeed * speedMultiplyer) + (avgSensor * sensorMultiplyer);
    }

    void Reset(){
        // Update the genomes fitness
        species.population[currentGenome].fitness = overallFitness;
        //Debug.Log("Genome: " + this.currentGenome + " ended with fitness: " + overallFitness);

        // Test Next Genome
        this.currentGenome = (currentGenome + 1) % species.population.Count;
        //Debug.Log("Current Genome: " + this.currentGenome);

        // We've gone through each genome, apply genetic algorithm 
        if (this.currentGenome == 0){
            currentGeneration++;
            species.NextGeneration();
            //ToDO Remove this
            //Save best of the generation

            lastGenAvgFitness = species.averageFitness;
        }

        // Reset car position and get ready for next test
        this.controller.ResetPosition();
        lastPosition = this.controller.car.position;
        distanceTraveled = 0;
        timeElapsed = 0;
        lastCheckpointDistance = controller.carCheckPoint.distanceToCheckpoint;
        timer = 10f;
        // Update best Fitness
        if (overallFitness > bestPopFitness){
            bestPopFitness = overallFitness;
        }
        bestFitness = 0f;

    }


}
