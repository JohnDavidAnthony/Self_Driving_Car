//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class MultiAgentCarController : MonoBehaviour{
//    // Create a new species
//    GeneticController species;

//    // Get the track we will be driving on

//    // Fitness function
//    public float distanceMultiplier = 2f;
//    public float speedMultiplyer = .6f;
//    public float sensorMultiplyer = .4f; // How important it is to stay in the middle of the track

//    // Car properties
//    public GameObject carObject;
//    public CarController[] controller;
//    public CheckPointScript checkpoints;
//    private Vector3[] lastPosition;
//    public float[] distanceTraveled;
//    public float avgSpeed;
//    public float[] timeElapsed;
//    private float avgSensor;
//    float[] timer;
//    public float bestFitness = 0;
//    public float bestPopFitness = 0;
//    int bestCar;

//    // Genetic Properties
//    public int currentGenome;
//    public int currentGeneration;
//    public float overallFitness;
//    public float lastGenAvgFitness;
//    public int numConcurrentGenomes = 200;
//    public int numCreatures = 200;
//    public float mutationRate = .05f;
//    int numToSimualte;
//    bool[] creatureAlive;


//    // Tell if we are moving backwards
//    float[] lastCheckpointDistance;
//    // How far we have moved since last checked
//    float movement;


//    void Start(){
//        // Create a new species
//        species = new GeneticController(numCreatures, mutationRate);
//        currentGenome = 0;
//        currentGeneration = 1;
//        lastGenAvgFitness = 0f;
//        numToSimualte = numConcurrentGenomes;
//        creatureAlive = new bool[numConcurrentGenomes];
//        bestCar = 0;


//        // Create our genome arrays
//        lastCheckpointDistance = new float[numConcurrentGenomes];
//        lastPosition = new Vector3[numConcurrentGenomes];
//        controller = new CarController[numCreatures];
//        distanceTraveled = new float[numConcurrentGenomes];
//        timeElapsed = new float[numConcurrentGenomes];
//        timer = new float[numConcurrentGenomes];

//        for (int i = 0; i < numConcurrentGenomes; i++) 
//        {
//            // Create our agents
//            GameObject clone = Instantiate(carObject);
//            controller[i] = clone.GetComponent<CarController>();
//            //controller[i].leftSensor = new CarSensors(clone.transform, new Transform(new Vector3()));

//            timer[i] = 10f;
//            creatureAlive[i] = true;
//            lastPosition[i] = controller[0].car.position;
//            lastCheckpointDistance[i] = controller[0].carCheckPoint.distanceToCheckpoint;
//        }


//    }

//    void Update(){
//        // Loop through each of our concurrent agents
//        for (int i = currentGenome; i < currentGenome + numToSimualte; i++)
//        {
//            // Get input from environmemnt
//            List<double> input = new List<double>();
//            for (int j = 0; j < controller[i].sensors.Count; j++)
//            {
//                input.Add(controller[i].sensors[j].hitNormal);
//            }
//            input.Add(controller[i].speed / controller[i].acceleration);

//            // Apply input to NN
//            double[] output = species.population[i].Run(input);

//            // Apply output to environment
//            controller[i].carTurn = (float)output[0];
//            controller[i].carDrive = (float)output[1];

//            CalculateFitness(i);

//            // Check to see if player hit wall or stopped progressing
//            // If fitness stopped improving end creature
//            if (bestFitness > overallFitness)
//            {
//                timer[i] -= Time.deltaTime;
//                if (timer[i] <= 0)
//                {
//                    Reset(i);
//                }
//            }
//            else
//            {
//                bestFitness = overallFitness;
//                timer[i] = 10f;
//            }

//            if (controller[i].playerHitWall)
//            {
//                Reset(i);
//            }
//            if (controller[i].playerStopped)
//            {
//                Reset(i);
//            }

//        }

//    }

//    private void CalculateFitness(int i)
//    {
//        // Check to see if car is moving away from checkpoint
//        movement = Vector3.Distance(controller[i].car.position, lastPosition[i]) * -1;
//        if (controller[i].carCheckPoint.distanceToCheckpoint < lastCheckpointDistance[i])
//        {
//            movement *= -1;
//        }

//        // Calcualte distance travelled
//        distanceTraveled[i] += movement;

//        // Calculate avg sensor value
//        avgSensor = (controller[i].leftSensor.hitNormal +
//            controller[i].frontLeftSensor.hitNormal +
//            controller[i].frontSensor.hitNormal
//            + controller[i].frontRightSensor.hitNormal +
//            controller[i].rightSensor.hitNormal) * movement / 5;

//        // Update last positions
//        lastCheckpointDistance[i] = controller[i].carCheckPoint.distanceToCheckpoint;
//        lastPosition[i] = controller[i].car.position;

//        // Calcualte avg speed
//        timeElapsed[i] += Time.deltaTime;
//        avgSpeed = distanceTraveled[i] / timeElapsed[i];

//        // Calcualte overall fitness
//        overallFitness = (distanceTraveled[i] * distanceMultiplier) + (avgSpeed * speedMultiplyer) + (avgSensor * sensorMultiplyer);
//        if (overallFitness > species.population[bestCar].fitness){
//            bestCar = i;
//        }
//    }

//    // TODO Update fitness fxn for multiple agents
//    void Reset(int i)
//    {
//        // Update the genomes fitness
//        species.population[i].fitness = overallFitness;
//        //Debug.Log("Genome: " + this.currentGenome + " ended with fitness: " + overallFitness);

//        // Mark the creature dead
//        creatureAlive[i] = false;

//        // If everybody is dead, simualte next batch or next generation
//        bool nextBatch = true;
//        for (int j = 0; j < creatureAlive.Length; j++){
//            if (creatureAlive[i]){
//                nextBatch = false;
//                break;
//            }
//        }
//        if (nextBatch)
//        {
//            for (int j = 0; j < creatureAlive.Length; j++){
//                // Reset car position and get ready for next test
//                controller[j].ResetPosition();
//                lastPosition[j] = controller[j].car.position;
//                distanceTraveled[j] = 0;
//                timeElapsed[j] = 0;
//                lastCheckpointDistance[j] = controller[j].carCheckPoint.distanceToCheckpoint;
//                timer[j] = 10f;
//                creatureAlive[j] = true;
//                // Update best Fitness
//                if (species.population[j].fitness > bestPopFitness)
//                {
//                    bestPopFitness = species.population[j].fitness;
//                }
//                bestFitness = 0f;
//            }
//        }
//    }
//}
