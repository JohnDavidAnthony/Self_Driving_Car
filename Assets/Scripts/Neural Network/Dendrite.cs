using UnityEngine;
using System.Collections;

public class Dendrite : MonoBehaviour
{
    public double weight;

    public Dendrite(){
        weight = Random.Range(0.0f, 1.0f);
    }

}
