using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSensors : MonoBehaviour{
    public Transform car;
    public float distance = 0;
    public float hitNormal = 0;

    //public Text distanceText;

    private void Start(){
    }

    // Update is called once per frame
    void LateUpdate(){

        // Get the direction vector of the ray
        Vector2 direction = gameObject.transform.position - car.position;

        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask1 = 1 << 8;
        // The ignore raycast layer
        int layerMask2 = 1 << 2;
        // Bit or to combine them
        int layerMask = layerMask1 | layerMask2;
        // This would cast rays only against our layer mask.
        // But instead we want to collide against everything except selected layers
        layerMask = ~layerMask;
        hitNormal = 1;

        // Cast the ray
        RaycastHit2D hit = Physics2D.Raycast(car.position, direction, direction.magnitude, layerMask);
        if (hit.collider != null){

            // Normalize hit distance to be 0 to 1
            hitNormal = hit.distance / direction.magnitude;

            Debug.DrawRay(car.position, direction, Color.red);

            //distanceText.text = hit.distance.ToString("0.00");

        }else{
            distance = 1;
            Debug.DrawRay(car.position, direction, Color.white);
            //distanceText.text = "Inf";
        }

    }
}
