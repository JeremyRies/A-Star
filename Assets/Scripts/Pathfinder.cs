using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    private List<Node> _exploredNodes;


    private PriorityQueue<Node> _frontierNodes;


    private Node _goalNode;
    private Node _startNode;


    private Graph _graph;

    private int _iterations;


    public List<Node> FindPath(Graph graph, Node start, Node goal)
    {
        if (start == null || goal == null || graph == null)
        {
            Debug.LogWarning("PATHFINDER Init error: missing component(s)!");
            return null;
        }


        if (start.NodeType == NodeType.Blocked || goal.NodeType == NodeType.Blocked)
        {
            Debug.LogWarning("PATHFINDER Init error: start and goal nodes must be unblocked!");
            return null;
        }


        _graph = graph;
        _startNode = start;
        _goalNode = goal;


        _frontierNodes = new PriorityQueue<Node>();
        _frontierNodes.Enqueue(start);


        _exploredNodes = new List<Node>();


        for (var x = 0; x < _graph.Width; x++)
        for (var y = 0; y < _graph.Height; y++)
            _graph.Nodes[x, y].Reset();

        _iterations = 0;
        _startNode.DistanceTraveled = 0;

        return SearchRoutine();
    }


    private List<Node> SearchRoutine()
    {
        while (_frontierNodes != null)
        {
            if (_frontierNodes.Count > 0 || _iterations < 10000)
            {
                var currentNode = _frontierNodes.Dequeue();
                _iterations++;


                if (!_exploredNodes.Contains(currentNode))
                    _exploredNodes.Add(currentNode);


                ExpandFrontierAStar(currentNode);


                if (_frontierNodes.Contains(_goalNode))
                {
                    return GetPathNodes(_goalNode);
                }
            }
            else 
            {
                Debug.LogError("Path not found");
                return null;
            }
        }

        return null;
    }


    private void ExpandFrontierAStar(Node node)
    {
        if (node != null)
            for (var i = 0; i < node.Neighbors.Count; i++)

                if (!_exploredNodes.Contains(node.Neighbors[i]))
                {
                    var distanceToNeighbor = _graph.GetNodeDistance(node, node.Neighbors[i]);


                    var newDistanceTraveled = distanceToNeighbor + node.DistanceTraveled + (int) node.NodeType;


                    if (float.IsPositiveInfinity(node.Neighbors[i].DistanceTraveled) ||
                        newDistanceTraveled < node.Neighbors[i].DistanceTraveled)
                    {
                        node.Neighbors[i].Previous = node;
                        node.Neighbors[i].DistanceTraveled = newDistanceTraveled;
                    }

                    if (!_frontierNodes.Contains(node.Neighbors[i]))
                    {
                        var distanceToGoal = _graph.GetNodeDistance(node.Neighbors[i], _goalNode);
                        node.Neighbors[i].Priority = node.Neighbors[i].DistanceTraveled + distanceToGoal;

                        _frontierNodes.Enqueue(node.Neighbors[i]);
                    }
                }
    }


    private List<Node> GetPathNodes(Node endNode)
    {
        var path = new List<Node>();
        if (endNode == null) return path;


        path.Add(endNode);


        var currentNode = endNode.Previous;

        while (currentNode != null)
        {
            path.Insert(0, currentNode);


            currentNode = currentNode.Previous;
        }


        return path;
    }
}