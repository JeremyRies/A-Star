using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
[RequireComponent(typeof(Graph))]
public class GraphView : MonoBehaviour
{
     
    public GameObject NodeViewPrefab;

     
    public NodeView[,] NodeViews;

     
    public void Init(Graph graph)
    {
        if (graph == null)
        {
            Debug.LogWarning("GRAPHVIEW No graph to initialize!");
            return;
        }

         
        NodeViews = new NodeView[graph.Width, graph.Height];

        foreach (Node n in graph.Nodes)
        {
             
            GameObject instance = Instantiate(NodeViewPrefab, Vector3.zero, Quaternion.identity);
            NodeView nodeView = instance.GetComponent<NodeView>();

            if (nodeView != null)
            {
                 
                nodeView.Init(n);

                 
                NodeViews[n.XIndex, n.YIndex] = nodeView;

                 
                Color originalColor = MapData.GetColorFromNodeType(n.NodeType);

                 
                nodeView.ColorNode(originalColor);
               
            }
        }
    }

     
    public void ColorNodes(List<Node> nodes, Color color, bool lerpColor = false, float lerpValue = 0.5f)
    {
         
        foreach (Node n in nodes)
        {
            if (n != null)
            {
                 
                NodeView nodeView = NodeViews[n.XIndex, n.YIndex];

                 
                Color newColor = color;

                 
                if (lerpColor)
                {
                     
                    Color originalColor = MapData.GetColorFromNodeType(n.NodeType);

                     
                    newColor = Color.Lerp(originalColor, newColor, lerpValue);
                }

                 
                if (nodeView != null)
                {
                    nodeView.ColorNode(newColor);
                }
            }
        }
    }

     
    private void ShowNodeArrows(Node node, Color color)
    {
        if (node != null)
        {
            NodeView nodeView = NodeViews[node.XIndex, node.YIndex];
            if (nodeView != null)
            {
                nodeView.ShowArrow(color);
            }
        }
    }

     
    public void ShowNodeArrows(List<Node> nodes, Color color)
    {
        foreach (Node n in nodes)
        {
            ShowNodeArrows(n, color);
        }
    }
}
