using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarCheckPoint : MonoBehaviour
{
    public CarController carController;
    public Transform[] checkpointArray;
    public int nextCheckpoint = 1;
    public int currentLap = 0;
    public float distanceToCheckpoint;

    public Text checkPointText;

    // Start is called before the first frame update
    void Start(){
        distanceToCheckpoint = Vector2.Distance(carController.car.position, checkpointArray[nextCheckpoint].position);
    }

    // Update is called once per frame
    void Update(){
        distanceToCheckpoint = Vector2.Distance(carController.car.position, checkpointArray[nextCheckpoint].position);
        checkPointText.text = "Checkpoint: " + nextCheckpoint.ToString();
    }
}
