using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
public class Graph : MonoBehaviour
{
     
    public Node[,] Nodes;
     
    int _width;
    public int Width { get { return _width; } }
    int _height;
    public int Height { get { return _height; } }
     
    public static readonly Vector2[] AllDirections =
    {
        new Vector2(0f,1f),
        new Vector2(1f,1f),
        new Vector2(1f,0f),
        new Vector2(1f,-1f),
        new Vector2(0f,-1f),
        new Vector2(-1f,-1f),
        new Vector2(-1f,0f),
        new Vector2(-1f,1f)
    };

     
    public void Init(int[,] mapData)
    {        
        _width = mapData.GetLength(0);
        _height = mapData.GetLength(1);

         
        Nodes = new Node[_width, _height];

         
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {   
                NodeType type = (NodeType)mapData[x, y];
        
                Node newNode = new Node(x, y, type);
                Nodes[x, y] = newNode;
           
                newNode.Position = new Vector3(x, 0, y);
            }
        }
         
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (Nodes[x, y].NodeType != NodeType.Blocked)
                {
                    Nodes[x, y].Neighbors = GetNeighbors(x, y);
                }
            }
        }
    }
     
    public bool IsWithinBounds(int x, int y)
    {
        return (x >= 0 && x < _width && y >= 0 && y < _height);
    }

     
    List<Node> GetNeighbors(int x, int y, Node[,] nodeArray, Vector2[] directions)
    {         
        List<Node> neighborNodes = new List<Node>();
         
        foreach (Vector2 dir in directions)
        {
             
            int newX = x + (int)dir.x;
            int newY = y + (int)dir.y;

             
            if (IsWithinBounds(newX, newY) && nodeArray[newX, newY] != null && nodeArray[newX, newY].NodeType != NodeType.Blocked)
            {
                neighborNodes.Add(nodeArray[newX, newY]);
            }
        }
         
        return neighborNodes;
    }

	 
	List<Node> GetNeighbors(int x, int y)
    {
        return GetNeighbors(x, y, Nodes, AllDirections);
    }

     
    public float GetNodeDistance(Node source, Node target)
    {
        int dx = Mathf.Abs(source.XIndex - target.XIndex);
        int dy = Mathf.Abs(source.YIndex - target.YIndex);

        int min = Mathf.Min(dx, dy);
        int max = Mathf.Max(dx, dy);

        int diagonalSteps = min;
        int straightSteps = max - min;

        return (1.4f * diagonalSteps + straightSteps);
    }

     
    public int GetManhattanDistance(Node source, Node target)
    {
		int dx = Mathf.Abs(source.XIndex - target.XIndex);
		int dy = Mathf.Abs(source.YIndex - target.YIndex);
        return (dx + dy);
    }

}
