using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Network_Layer_Nodes : MonoBehaviour
{
    public List<Image> connections;
    public Color posColour;
    public Color negColour;

    public void DisplayConnections(int neuronIndex, Layer currentLayer, UI_Network_Layer nextLayer, NeuralNetwork network, float scaleFactor)
    {
        Image node = connections[0];
        for (int i = connections.Count; i < nextLayer.nodes.Count; i++)
        {
            Image newNode = Instantiate(node);
            newNode.transform.SetParent(this.transform, false);
            connections.Add(newNode);
        }

        // Position Connections
        for (int i = 0; i < connections.Count; i++)
        {
            PositionConnections(connections[i], nextLayer.nodes[i], neuronIndex, i, currentLayer.GetWeights(network), scaleFactor);
        }
    }

    private void PositionConnections(Image connection, UI_Network_Layer_Nodes otherNode, int nodeIndex, int connectedNodeIndex, double[,] weights, float scaleFactor)
    {
        //Set local position to 0
        connection.transform.localPosition = Vector3.zero;

        //Set connection width
        Vector2 sizeDelta = connection.rectTransform.sizeDelta;
        double weight = weights[nodeIndex, connectedNodeIndex];
        sizeDelta.x = (float)System.Math.Abs(weight*4);
        if (sizeDelta.x < 1)
            sizeDelta.x = 1;

        //Set conenction color
        if (weight >= 0) {
            posColour.a = 1f;
            connection.color = posColour;
        }
            
        else {
            negColour.a = 1f;
            connection.color = negColour;
        }
            
        //Set connection length (height)
        Vector2 connectionVec = this.transform.position - otherNode.transform.position;
        sizeDelta.y = connectionVec.magnitude / scaleFactor;

        connection.rectTransform.sizeDelta = sizeDelta;

        //Set connection rotation
        float angle = Vector2.Angle(Vector2.up, connectionVec);
        connection.transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
    }

}
