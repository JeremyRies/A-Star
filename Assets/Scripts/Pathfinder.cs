using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private List<Node> _exploredNodes;


    private PriorityQueue<Node> _frontierNodes;


    private Node _goalNode;
    private Node _startNode;


    private Graph _graph;
    private GraphView _graphView;

    private int _iterations;

    private List<Node> _pathNodes;

    public Color ArrowColor = new Color(0.85f, 0.85f, 0.85f, 1f);
    public Color ExploredColor = Color.gray;
    public Color FrontierColor = Color.magenta;
    public Color GoalColor = Color.red;
    public Color StartColor = Color.green;
    public Color HighlightColor = new Color(1f, 1f, 0.5f, 1f);
    public Color PathColor = Color.cyan;

    public bool ExitOnGoal = true;
    public bool IsComplete;
    public bool ShouldShowColors = true;
    public bool ShowArrows = true;
    public bool ShowIterations = true;

    public void Init(Graph graph, GraphView graphView, Node start, Node goal)
    {
        if (start == null || goal == null || graph == null || graphView == null)
        {
            Debug.LogWarning("PATHFINDER Init error: missing component(s)!");
            return;
        }


        if (start.NodeType == NodeType.Blocked || goal.NodeType == NodeType.Blocked)
        {
            Debug.LogWarning("PATHFINDER Init error: start and goal nodes must be unblocked!");
            return;
        }


        _graph = graph;
        _graphView = graphView;
        _startNode = start;
        _goalNode = goal;


        ShowColors(graphView, start, goal);


        _frontierNodes = new PriorityQueue<Node>();
        _frontierNodes.Enqueue(start);


        _exploredNodes = new List<Node>();
        _pathNodes = new List<Node>();


        for (var x = 0; x < _graph.Width; x++)
        for (var y = 0; y < _graph.Height; y++)
            _graph.Nodes[x, y].Reset();


        IsComplete = false;
        _iterations = 0;
        _startNode.DistanceTraveled = 0;
    }


    private void ShowColors(bool lerpColor = false, float lerpValue = 0.5f)
    {
        ShowColors(_graphView, _startNode, _goalNode, lerpColor, lerpValue);
    }


    private void ShowColors(GraphView graphView, Node start, Node goal, bool lerpColor = false, float lerpValue = 0.5f)
    {
        if (graphView == null || start == null || goal == null) return;


        if (_frontierNodes != null) graphView.ColorNodes(_frontierNodes.ToList(), FrontierColor, lerpColor, lerpValue);

        if (_exploredNodes != null) graphView.ColorNodes(_exploredNodes, ExploredColor, lerpColor, lerpValue);

        if (_pathNodes != null && _pathNodes.Count > 0)
            graphView.ColorNodes(_pathNodes, PathColor, lerpColor, lerpValue * 2f);


        var startNodeView = graphView.NodeViews[start.XIndex, start.YIndex];

        if (startNodeView != null)
        {
            startNodeView.ColorNode(StartColor);
            startNodeView.HoeDoor.SetActive(true);
        }

        var goalNodeView = graphView.NodeViews[goal.XIndex, goal.YIndex];

        if (goalNodeView != null)
        {
            goalNodeView.ColorNode(GoalColor);
            goalNodeView.Door.SetActive(true);
        }
    }


    public IEnumerator SearchRoutine(float timeStep = 0.1f)
    {
        var timeStart = Time.realtimeSinceStartup;


        yield return null;


        while (!IsComplete && _frontierNodes != null)

            if (_frontierNodes.Count > 0)
            {
                var currentNode = _frontierNodes.Dequeue();
                _iterations++;


                if (!_exploredNodes.Contains(currentNode))
                    _exploredNodes.Add(currentNode);


                ExpandFrontierAStar(currentNode);


                if (_frontierNodes.Contains(_goalNode))
                {
                    _pathNodes = GetPathNodes(_goalNode);


                    if (ExitOnGoal)
                    {
                        IsComplete = true;
                        Debug.Log("Path length = " +
                                  _goalNode.DistanceTraveled);
                    }
                }


                if (ShowIterations)
                {
                    ShowDiagnostics(true);

                    yield return new WaitForSeconds(timeStep);
                }
            }

            else
            {
                IsComplete = true;
            }


        ShowDiagnostics(true);

        Debug.Log("PATHFINDER SearchRoutine: elapse time = " + (Time.realtimeSinceStartup - timeStart) + " seconds");
        Debug.Log("Iterations: " + _iterations);
    }


    private void ShowDiagnostics(bool lerpColor = false, float lerpValue = 0.5f)
    {
        if (ShouldShowColors) ShowColors(lerpColor, lerpValue);


        if (_graphView != null && ShowArrows)
        {
            _graphView.ShowNodeArrows(_frontierNodes.ToList(), ArrowColor);


            if (_frontierNodes.Contains(_goalNode)) _graphView.ShowNodeArrows(_pathNodes, HighlightColor);
        }
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