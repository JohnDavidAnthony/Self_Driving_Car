using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AICarController : MonoBehaviour
{
    public NeuralNetwork network = null;

    // Car properties
    public CarController controller;
    private Vector3 lastPosition;
    public float distanceTraveled;
    public float avgSpeed;
    public float timeElapsed;
    private float avgSensor;
    float timer = 0;
    public float bestFitness = 0;
    public float bestPopFitness = 0;
    public float overallFitness;
    public float distanceMultiplier;
    public float speedMultiplier;
    public float sensorMultiplier;
    public bool alive = true;

    // Tell if we are moving backwards
    float lastCheckpointDistance;
    // How far we have moved since last checked
    float movement;

    void Start(){
        // Create new species with specified genetics
        timeElapsed = 0;
        lastPosition = this.controller.car.position;
        lastCheckpointDistance = controller.carCheckPoint.distanceToCheckpoint;
    }

    void Update(){
        if (alive)
        {
            // Get input from environmemnt
            List<double> input = new List<double>();
            for (int i = 0; i < controller.sensors.Count; i++)
            {
                input.Add(controller.sensors[i].hitNormal);
            }
            input.Add(controller.speed / controller.acceleration);

            // Apply input to NN
            double[] output = network.Run(input);

            // Apply output to environment
            controller.carTurn = (float)output[0];
            controller.carDrive = (float)output[1];

            CalculateFitness();

            // Check to see if player hit wall or stopped progressing
            // If fitness stopped improving end creature
            if (bestFitness > overallFitness)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    Stop();
                }
            }
            else
            {
                bestFitness = overallFitness;
                timer = 10f;
            }

            if (controller.playerHitWall)
            {
                Stop();
            }
            if (controller.playerStopped)
            {
                Stop();
            }
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
        for (int i = 0; i < controller.sensors.Count; i++)
        {
            avgSensor += controller.sensors[i].hitNormal;
        }
        avgSpeed /= controller.sensors.Count;

        // Update last positions
        lastCheckpointDistance = controller.carCheckPoint.distanceToCheckpoint;
        lastPosition = controller.car.position;

        // Calcualte avg speed
        timeElapsed += Time.deltaTime ;
        avgSpeed = distanceTraveled / timeElapsed;

        // Calcualte overall fitness
        overallFitness = (distanceTraveled * distanceMultiplier) + (avgSpeed * speedMultiplier) + (avgSensor * sensorMultiplier);
    }

    void Stop(){
        alive = false;
        controller.carTurn = 0;
        controller.carDrive = 0;
        controller.car.isKinematic = true;
        controller.car.velocity = Vector3.zero;
    }

    void Reset(){
        // Reset car position and get ready for next test
        this.controller.ResetPosition();
        lastPosition = this.controller.car.position;
        distanceTraveled = 0;
        timeElapsed = 0;
        lastCheckpointDistance = controller.carCheckPoint.distanceToCheckpoint;
        timer = 10f;
        avgSensor = 0;
        // Update best Fitness
        if (overallFitness > bestPopFitness){
            bestPopFitness = overallFitness;
        }
        bestFitness = 0f;
    }
}
