using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Genetics : MonoBehaviour
{
    public Academy academy;
    public Text genome;
    public Text generation;
    public Text fitness;
    public Text timeScale;
    public Slider slider;
    public Text averageFitness;
    public Text bestFitness;

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = slider.value;
        genome.text = "Genome #: " + (academy.currentGenome+1 - academy.batchSimulate) + " - " + (academy.currentGenome);
        generation.text = "Generation #: " + academy.currentGeneration;
        if (academy.bestCar != null){
            fitness.text = "Current Fitness: " + academy.bestCar.overallFitness;
        }
        timeScale.text = "Timescale: " + slider.value + "x";
        averageFitness.text = "Last Gen Average Fitness: " + academy.species.averageFitness;
        bestFitness.text = "Best Fitness: " + academy.bestGenomeFitness; 

    }
}
