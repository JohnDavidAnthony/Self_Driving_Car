using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;


public class CarAgent : Agent
{
    public CarController carController;
    //Agent Stuff
    public override void AgentReset()
    {
        Debug.Log("Agent Reset");
        carController.car = GetComponent<Rigidbody2D>();
        carController.car.angularVelocity = 0f;
        carController.car.velocity = Vector3.zero;
        carController.car.transform.position = new Vector3(-21.58777f, 6.837944f, -1);
        carController.car.rotation = -30f;
    }
    // Collections the observations about the world
    public override void CollectObservations()
    {
        // Get Distances
        AddVectorObs(carController.frontSensor.distance);
        AddVectorObs(carController.frontLeftSensor.distance);
        AddVectorObs(carController.frontRightSensor.distance);
        AddVectorObs(carController.leftSensor.distance);
        AddVectorObs(carController.rightSensor.distance);

        // Get Speed
        AddVectorObs(carController.car.velocity.magnitude);

    }
    // Get the Agents Actions
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        Debug.Log(vectorAction[0] + " " + vectorAction[1] + " " + vectorAction[2]);
        // Gas
        carController.carDrive = vectorAction[0];
        // Left and right
        carController.carTurn = vectorAction[1];
        // Brake
        carController.carBrake = vectorAction[2];

        if (carController.playerStopped){
            carController.playerStopped = false;
            AddReward(-25f);
            Done();
        }

        if (carController.playerHitWall){
            Debug.Log("Hit wall");
            carController.playerHitWall = false;
            SetReward(-100f);
            Done();
        }

        if (carController.playerHitCheckPoint){
            carController.playerHitCheckPoint = false;
            SetReward(10f);
        }
        SetReward(-.0001f);
    }
}
