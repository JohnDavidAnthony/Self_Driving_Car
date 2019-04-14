using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    public CarController car;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        // Check if the collider is the player
        if (!other.CompareTag("Player")){
            return;
        }

        CarCheckPoint carCheckPoint = car.GetComponent<CarCheckPoint>();
        if (transform == carCheckPoint.checkpointArray[carCheckPoint.nextCheckpoint].transform){
            // Car has reached next checkpoint
            if (carCheckPoint.nextCheckpoint + 1 == carCheckPoint.checkpointArray.Length){
                // Next Checkpoint is the finish line
                carCheckPoint.nextCheckpoint = 0;
                // Increase Lap
                carCheckPoint.currentLap += 1;
            }else{
                // We've reached another checkpoint
                Debug.Log("Player hit checkpoint");
                car.playerHitCheckPoint = true;
                carCheckPoint.nextCheckpoint += 1;
                car.hitCheckPoint = true;
            }
        }


    }
}
