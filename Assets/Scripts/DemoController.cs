using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoController : MonoBehaviour
{
     
    public MapData MapData;

     
    public Graph Graph;

     
    public int StartX = 0;
    public int StartY = 0;

     
    public int GoalX = 15;
    public int GoalY = 1;

    void Start()
    {
        if (MapData != null && Graph != null)
        {
             
            int[,] mapInstance = MapData.MakeMap();

             
            Graph.Init(mapInstance);

             
            GraphView graphView = Graph.gameObject.GetComponent<GraphView>();
            if (graphView != null)
            {
                graphView.Init(Graph);
            }

             
            if (Graph.IsWithinBounds(StartX,StartY) && Graph.IsWithinBounds(GoalX, GoalY))
            {
                Node startNode = Graph.Nodes[StartX, StartY];
                Node goalNode = Graph.Nodes[GoalX, GoalY];
                var pathfinder = new Pathfinder();

                var path = pathfinder.FindPath(Graph, startNode, goalNode);

                foreach (var node in path)
                {
                    Debug.Log(node.Position);
                }
            }
        }
    }

}
