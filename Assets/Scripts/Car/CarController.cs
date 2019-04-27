using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarController : MonoBehaviour
{

    // Car properties
    public float acceleration = 5f;
    public float deacceleration = 3f;
    public float turnSpeed = 100f;
    public float speed;
    float torqueForce = 0;
    public CarCheckPoint carCheckPoint;
    Vector3 startingPos;
    Quaternion carRotation;
    float idleTime = 5f;
    float timeLeft = 0;
    int firstCheckpoint;

    // How much the car "normally drifts"
    float driftSpeedMoving = .9f;
    // How fast we drift when we let off the gas and are headed sideways
    float driftSpeedStatic = .9f;
    // Max amount that we can slide sideways when stopping
    float maxSideways = .5f;

    // The Car and sensors
    public Rigidbody2D car;
    public List<CarSensors> sensors;

    // Car Flags
    public bool playerStopped;
    public bool playerHitWall;
    public bool hitCheckPoint;
    bool timerStarted;

    // Input from NN 
    public float carDrive;
    public float carTurn;

    void Start(){
        // Get the checkpoints
        carCheckPoint = gameObject.GetComponent<CarCheckPoint>();
        playerStopped = false;
        playerHitWall = false;
        hitCheckPoint = false;
        startingPos = gameObject.transform.position;
        carRotation = gameObject.transform.rotation;
        timerStarted = false;
        firstCheckpoint = carCheckPoint.nextCheckpoint;

    }

    void FixedUpdate(){
        speed = car.velocity.magnitude;


        // Check to see if car hasnt moved
        if (car.velocity.magnitude < .05f){
            // If timer is already going add to it
            if (timerStarted){
                timeLeft += Time.deltaTime;
                if (timeLeft > idleTime){
                    // Been idle for to long
                    //Debug.Log("Player Stopped Moving");
                    playerStopped = true;
                    timerStarted = false;
                    timeLeft = 0;
                }
            }
            // Otherwise start timer
            else{
                timerStarted = true;
            }
        }

        // How fast we drift
        float driftFactor = driftSpeedStatic;
        if (ForwardVelocity().magnitude > maxSideways){
            driftFactor = driftSpeedMoving;
        }
        // Reduce sideways velocity from previous inertia using the drift speed
        car.velocity = ForwardVelocity() + SideVelocity() * driftFactor;

        // Movement
        if (carDrive >0){
            // Go forward
            car.AddForce(transform.up * acceleration);

        }
        if (Input.GetKey(KeyCode.S) || carDrive <= 0){
            // Go Backwards
            car.AddForce(transform.up * deacceleration);
        }

        // Turning
        // Don't let car turn if stopped
        torqueForce = Mathf.Lerp(0, turnSpeed, car.velocity.magnitude / 2);
        //car.angularVelocity = Input.GetAxis("Horizontal") * torqueForce;
        car.angularVelocity = (float)((carTurn) * torqueForce);

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
            if (other.transform == carCheckPoint.checkpointArray[carCheckPoint.nextCheckpoint].transform)
            {
                // Car has reached next checkpoint
                if (carCheckPoint.nextCheckpoint + 1 == carCheckPoint.checkpointArray.Length)
                {
                    // Next Checkpoint is the finish line
                    carCheckPoint.nextCheckpoint = 0;
                    // Increase Lap
                    carCheckPoint.currentLap += 1;
                }
                else
                {
                    // We've reached another checkpoint
                    carCheckPoint.nextCheckpoint += 1;
                    hitCheckPoint = true;
                }
            }
            return;
        }else if(other.gameObject.tag == "Player")
        {
            return;
        }
        ///Assume anything we hit besides checkpoint is a wall

        playerHitWall = true;
       
    }

    public void ResetPosition(){
        this.car.velocity = Vector2.zero;
        this.car.position = startingPos;
        gameObject.transform.rotation = carRotation;
        this.carCheckPoint.nextCheckpoint = firstCheckpoint;
        timeLeft = 0;

        playerStopped = false;
        playerHitWall = false;
        hitCheckPoint = false;
        timerStarted = false;

    }

}
