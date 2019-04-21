using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour{
    Vector3 wheelAngle;
    float steerAngle, maxSteerAngle = 30f;
    public Rigidbody2D car;
    public CarController aicar;
    // Update is called once per frame
    void Update(){
        steerAngle = maxSteerAngle * aicar.carTurn + car.rotation;
    }

    void LateUpdate(){

        wheelAngle = transform.eulerAngles;
        wheelAngle.z = steerAngle;
        transform.eulerAngles = wheelAngle;
    }
}
