using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Network_Layer_Nodes : MonoBehaviour
{
    public List<Image> connections;
    public Color posColour;
    public Color negColour;

    public void DisplayConnections(int neuronIndex, Layer currentLayer, UI_Network_Layer nextLayer, NeuralNetwork network)
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
            PositionConnections(connections[i], nextLayer.nodes[i], neuronIndex, i, currentLayer.GetWeights(network));
        }
    }

    private void PositionConnections(Image connection, UI_Network_Layer_Nodes otherNode, int nodeIndex, int connectedNodeIndex, double[,] weights)
    {
        //Set local position to 0
        connection.transform.localPosition = Vector3.zero;

        //Set connection width
        Vector2 sizeDelta = connection.rectTransform.sizeDelta;
        double weight = weights[nodeIndex, connectedNodeIndex];
        sizeDelta.x = (float)System.Math.Abs(weight);
        if (sizeDelta.x < 1)
            sizeDelta.x = 1;

        //Set conenction color
        if (weight >= 0)
            connection.color = posColour;
        else
            connection.color = negColour;

        //Set connection length (height)
        Vector2 connectionVec = this.transform.position - otherNode.transform.position;
        sizeDelta.y = connectionVec.magnitude;

        connection.rectTransform.sizeDelta = sizeDelta;

        //Set connection rotation
        float angle = Vector2.Angle(Vector2.up, connectionVec);
        connection.transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
    }

    public void HideConnections()
    {
        //Destory all but dummy connection
        for (int i = this.connections.Count - 1; i >= 1; i++)
        {
            Image toBeDestroyed = connections[i];
            connections.RemoveAt(i);
            Destroy(toBeDestroyed);
        }

        //Hide dummy connection
        connections[0].gameObject.SetActive(false);
    }
}
