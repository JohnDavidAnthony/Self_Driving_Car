using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Network_Layer : MonoBehaviour
{
    public RectTransform contents;
    public List<UI_Network_Layer_Nodes> nodes;

    // Build and Display layer with the nodes
    public void Display(Layer layer) {
        Display(layer.NumNeurons());
    }

    public void Display(int numNeurons){
        UI_Network_Layer_Nodes nodeTemplate = nodes[0];

        // Create all the nodes for our layer
        for (int i = 0; i < numNeurons -1; i++)
        {
            UI_Network_Layer_Nodes node = Instantiate(nodeTemplate);
            node.transform.SetParent(contents.transform, false);
            nodes.Add(node);
        }
    }

    public void DisplayConnections(Layer currentLayer, UI_Network_Layer nextLayer, NeuralNetwork network, float scaleFactor)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].DisplayConnections(i, currentLayer, nextLayer, network, scaleFactor);
        }
            
    }

}
