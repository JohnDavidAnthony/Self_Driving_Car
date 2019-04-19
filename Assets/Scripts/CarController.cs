using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarController : MonoBehaviour
{

    // Car properties
    float acceleration = 5f;
    float deacceleration = 10f;
    float turnSpeed = 100f;
    // The type of drift we are used to 
    float driftSpeedMoving = .9f;
    // How fast we drift when we let off the gas and are headed sideways
    float driftSpeedStatic = .9f;

    // Max amount that we can slide sideways when stopping
    float maxSideways = .5f;

    // For calcualting average speed of car
    public List<float> speeds = new List<float>();
    float speedTimer;

    public Rigidbody2D car;

    // The Cars Sensors
    public CarSensors frontSensor;
    public CarSensors frontLeftSensor;
    public CarSensors frontRightSensor;
    public CarSensors leftSensor;
    public CarSensors rightSensor;

    public float speedNormal;

    public bool hitCheckPoint = false;

    float torqueForce = 0;
    public float AITurn = 1;

    public double carDrive = 0;
    public double carTurn = 0;

    public bool playerStopped = false;
    public bool playerHitWall = false;
    CarCheckPoint carCheckPoint;



    void Start(){
        //System.Diagnostics.Debug.WriteLine("Ddwad");
        //System.Diagnostics.Trace.WriteLine("message");
        //System.Diagnostics.Debug.WriteLine("Send to debug output.");
        //Academy a = new Academy(2, .01f);
        //Debug.Log("Mother");
        //List<double>b = a.population[0].Encode();
        //foreach (var item in b)
        //    Debug.Log(item); // Replace this with your version of printing

        //Debug.Log("Father");
        //List<double> c = a.population[1].Encode();
        //foreach (var item in c)
        //    Debug.Log(item); // Replace this with your version of printing


        //Debug.Log("\n Breeding");
        //a.NextGeneration();

        //Debug.Log("Child 1");
        //List<double> d = a.nextGeneration[0].Encode();
        //foreach (var item in d)
        //    Debug.Log(item); // Replace this with your version of printing

        //Debug.Log("Child 2");
        //List<double> e = a.nextGeneration[1].Encode();
        //foreach (var item in e)
        //    Debug.Log(item); // Replace this with your version of printing


        //Debug.Log("Mother after birth");
        //b = a.population[0].Encode();
        //foreach (var item in b)
        //    Debug.Log(item); // Replace this with your version of printing

        //Debug.Log("Father after birth");
        //c = a.population[1].Encode();
        //foreach (var item in c)
        //Debug.Log(item); // Replace this with your version of printing


        //Debug.Log("Mother");
        //foreach (var item in b)
        //    Debug.Log(item); // Replace this with your version of printing

        //Debug.Log("Father");
        //foreach (var item in c)
        //Debug.Log(item); // Replace this with your version of printing


        speedTimer = .2f;
        carCheckPoint = car.GetComponent<CarCheckPoint>();
        this.car.position = new Vector3(-21f, 6.8f, -1f);
    }

    // Update is called once per frame
    float timeLeft = 0;
    bool timerStarted = false;
    void FixedUpdate()
    {
        car = GetComponent<Rigidbody2D>();
        speedNormal = car.velocity.magnitude / acceleration;

        //Debug.Log(frontSensor.hitNormal);


        float delay = 5f;
        // Check to see if car hasnt moved
        if (car.velocity.magnitude < .05f){
            // If timer is already going add to it
            if (timerStarted){
                timeLeft += Time.deltaTime;
                if (timeLeft > delay){
                    // Been idle for to long
                    Debug.Log("Player Stopped Moving");
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
        if (Input.GetKey(KeyCode.W) || carDrive >0){
            // Go forward
            car.AddForce(transform.up * acceleration);

        }
        if (Input.GetKey(KeyCode.S) || carDrive <= 0){
            // Go Backwards
            car.velocity = car.velocity * .99f;
        }

        // Turning
        // Don't let car turn if stopped
        torqueForce = Mathf.Lerp(0, turnSpeed, car.velocity.magnitude / 2);
        //car.angularVelocity = Input.GetAxis("Horizontal") * torqueForce;
        car.angularVelocity = (float)((carTurn) * torqueForce);

        // calcuate speed every 1 second
        speedTimer += Time.deltaTime;
        if (speedTimer > 1)
        {
            CalculateSpeed();
            speedTimer = 0;
        }
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

    private void OnTriggerEnter2D(Collider2D other){
        //Player collided with wall
        //collided = true;
        if (other.gameObject.tag == "CheckPoint"){
            return;
        }
        ///Debug.Log("Player hit Wall");
        playerHitWall = true;
       
    }

    public void ResetPosition(){
        this.car.velocity = Vector2.zero;
        this.carDrive = 0;
        this.carTurn = 0;
        this.car.position = new Vector3(-21f,6.8f,-1f);
        this.car.rotation = -22f;
        playerHitWall = false;
        hitCheckPoint = false;
        speeds.Clear();
        speedTimer = 999;
        this.carCheckPoint.nextCheckpoint = 0;
        this.playerStopped = false;

    }


    void CalculateSpeed(){
        float speed = car.velocity.magnitude;
        speeds.Add(speed);
    }

    public float GetAverageSpeed() {
        float averageTotal = 0;

        for (int i = 0; i < speeds.Count; i++){

            averageTotal += speeds[i];
        }

        return averageTotal / speeds.Count;
    }
}
