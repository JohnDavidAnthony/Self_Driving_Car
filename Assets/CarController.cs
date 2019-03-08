using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    // Car properties
    float acceleration = 15f;
    float deacceleration = 10f;
    float turnSpeed = 200f;
    // The type of drift we are used to 
    float driftSpeedMoving = .9f;
    // How fast we drift when we let off the gas and are headed sideways
    float driftSpeedStatic = .9f;

    // Max amount that we can slide sideways when stopping
    float maxSideways = .5f;
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {



        Rigidbody2D car = GetComponent<Rigidbody2D>();

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
            car.velocity = car.velocity * .99f;
        }

        // Turning
        // Don't let car turn if stopped
        float torqueForce = Mathf.Lerp(0, turnSpeed, car.velocity.magnitude / 2);
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

    void RayCasting(Vector2 rayStart, Vector2 rayEnd){

    }

}
