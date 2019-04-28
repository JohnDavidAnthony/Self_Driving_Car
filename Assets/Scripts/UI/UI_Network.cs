using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Network : MonoBehaviour
{
    public List<UI_Network_Layer> layers;
    public Canvas canvas;

    // Build and display Neural Network
    public void Display(NeuralNetwork network)
    {
        UI_Network_Layer layer = layers[0];
        for (int i = 0; i < network.NumLayers() -1; i++){
            UI_Network_Layer newLayer = Instantiate(layer);
            newLayer.transform.SetParent(this.transform, false);
            layers.Add(newLayer);
        }

        //Set input and hidden layer contents
        for (int l = 0; l < this.layers.Count - 1; l++){
            this.layers[l].Display(network.layers[l]);
        }

        // Last Layer
        this.layers[layers.Count - 1].Display(network.layerStructure[network.layerStructure.Length-1]);
    }

    // Draw the connections (coroutine).
    public void DrawConnections(NeuralNetwork network)
    {
        //Draw node connections
        for (int l = 0; l < this.layers.Count - 1; l++)
            this.layers[l].DisplayConnections(network.layers[l], this.layers[l + 1], network, canvas.scaleFactor);

    }
}
