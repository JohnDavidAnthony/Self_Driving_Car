using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NeuralNet : MonoBehaviour
{
    public AICarController controller;
    public Text genome;
    public Text generation;
    public Text fitness;
    public Text timeScale;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = slider.value;
        genome.text = "Genome #: " + controller.currentGenome;
        generation.text = "Generation #: " + controller.currentGeneration;
        fitness.text = "Current Fitness: " + controller.overallFitness;
        timeScale.text = "Timescale: " + slider.value + "x";

    }
}
