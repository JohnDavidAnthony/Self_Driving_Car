using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarRayCast : MonoBehaviour{
    public Transform car;
    public Transform rayEnd;

    public Text distanceText;

    private void Start(){
    }

    // Update is called once per frame
    void LateUpdate(){

        Vector2 direction = rayEnd.position - car.position;


        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8.
        layerMask = ~layerMask;

        // Cast the ray
        RaycastHit2D hit = Physics2D.Raycast(car.position, direction, direction.magnitude, layerMask);
        if (hit.collider != null){
            // Check to see if collision is a checkpoint
            if (hit.collider.transform.CompareTag("CheckPoint")){
                Debug.DrawRay(car.position, direction, Color.green);
            }
            else{
                Debug.DrawRay(car.position, direction, Color.red);
            }
            distanceText.text = hit.distance.ToString("0.00");

        }else{
            Debug.DrawRay(car.position, direction, Color.white);
            distanceText.text = "Null";
        }

    }
}
