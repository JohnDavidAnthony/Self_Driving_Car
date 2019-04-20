using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HumanCarController : MonoBehaviour
{

    // Car properties
    public float acceleration = 5f;
    public float deacceleration = -3f;
    public float turnSpeed = 100f;
    public float speed;
    float torqueForce = 0;
    public CarCheckPoint carCheckPoint;
    Vector3 startingPos;
    float carRotation;


    // How much the car "normally drifts"
    float driftSpeedMoving = .9f;
    // How fast we drift when we let off the gas and are headed sideways
    float driftSpeedStatic = .9f;
    // Max amount that we can slide sideways when stopping
    float maxSideways = .5f;

    // The Car and sensors
    public Rigidbody2D car;
    public CarSensors frontSensor;
    public CarSensors frontLeftSensor;
    public CarSensors frontRightSensor;
    public CarSensors leftSensor;
    public CarSensors rightSensor;

    // Car Flags
    public bool playerStopped;
    public bool playerHitWall;
    public bool hitCheckPoint;


    void Start(){
        // Get the checkpoints
        carCheckPoint = car.GetComponent<CarCheckPoint>();
        playerStopped = false;
        playerHitWall = false;
        hitCheckPoint = false;
        startingPos = car.position;
        carRotation = car.rotation;

}


    void FixedUpdate()
    {
        car = GetComponent<Rigidbody2D>();
        speed = car.velocity.magnitude;

        // How fast we drift
        float driftFactor = driftSpeedStatic;
        if (ForwardVelocity().magnitude > maxSideways){
            driftFactor = driftSpeedMoving;
        }
        // Reduce sideways velocity from previous inertia using the drift speed
        car.velocity = ForwardVelocity() + SideVelocity() * driftFactor;

        // Movement
        if (Input.GetKey(KeyCode.W)){
            // Go forward
            car.AddForce(transform.up * acceleration);

        }
        if (Input.GetKey(KeyCode.S)){
            // Go Backwards
            car.AddForce(transform.up * deacceleration);
        }

        // Turning
        // Don't let car turn if stopped
        torqueForce = Mathf.Lerp(0, turnSpeed, car.velocity.magnitude / 2);
        car.angularVelocity = Input.GetAxis("Horizontal") * torqueForce;

    }

    // Returns our velocity on the forward direction
    Vector2 ForwardVelocity(){
        // Out of our velocity, how much is going in the forward direction
        // Using the dot product of velocity and our forward
        return transform.up * Vector2.Dot(GetComponent<Rigidbody2D>().velocity, transform.up);
    }

    // Returns our velocity on the sideways direction
    Vector2 SideVelocity(){
        return transform.right * Vector2.Dot(GetComponent<Rigidbody2D>().velocity, transform.right);
    }


    // Collision Detection
    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "CheckPoint"){
            return;
        }
        ///Assume anything we hit besides checkpoint is a wall
        playerHitWall = true;
       
    }

    public void ResetPosition(){
        this.car.velocity = Vector2.zero;
        this.car.position = startingPos;
        this.car.rotation = carRotation;
        this.carCheckPoint.nextCheckpoint = 0;

        playerStopped = false;
        playerHitWall = false;
        hitCheckPoint = false;

    }

}
