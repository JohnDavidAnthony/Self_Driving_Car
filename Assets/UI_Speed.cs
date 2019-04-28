using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Speed : MonoBehaviour
{
    public Rigidbody2D car;
    public Text speedText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        speedText.text = "Speed: " + car.velocity.magnitude.ToString();
    }
}
