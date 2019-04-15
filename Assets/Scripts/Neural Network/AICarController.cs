using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AICarController : MonoBehaviour
{
    Academy academy;
    public CarController controller;

    // Use this for initialization
    void Start(){
        academy = new Academy(2, .05f);

    }

    // Update is called once per frame
    void Update(){

        List<double> input = new List<double>();
        input.Add(controller.leftSensor.distance);
        input.Add(controller.frontLeftSensor.distance);
        input.Add(controller.frontSensor.distance);
        input.Add(controller.frontRightSensor.distance);
        input.Add(controller.rightSensor.distance);
        input.Add(controller.car.velocity.magnitude);
       

        double[] output = academy.population[0].run(input);

        controller.carTurn = output[0];
        controller.carDrive = output[1];


    }
}
