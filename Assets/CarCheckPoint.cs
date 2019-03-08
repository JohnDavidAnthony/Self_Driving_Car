using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarCheckPoint : MonoBehaviour
{
    public Transform[] checkpointArray;
    public int nextCheckpoint = 1;
    public int currentLap = 0;

    public Text checkPointText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkPointText.text = nextCheckpoint.ToString();
    }
}
