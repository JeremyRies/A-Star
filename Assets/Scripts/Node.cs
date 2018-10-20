using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

 
public enum NodeType
{
    Open = 0,
    Blocked = 1,
    LightTerrain = 2,
    MediumTerrain = 3,
    HeavyTerrain = 4
}

 
public class Node: IComparable<Node>
{
     
    public readonly NodeType NodeType = NodeType.Open;

     
    public readonly int XIndex = -1;
    public readonly int YIndex = -1;

     
    public Vector3 Position;

     
    public List<Node> Neighbors = new List<Node>();

     
    public float DistanceTraveled = Mathf.Infinity;

     
    public Node Previous = null;

     
    public float Priority;

     
    public Node(int xIndex, int yIndex, NodeType nodeType)
    {
        this.XIndex = xIndex;
        this.YIndex = yIndex;
        this.NodeType = nodeType;
    }

     
    public int CompareTo(Node other)
    {
        if (this.Priority < other.Priority)
        {
            return -1;
        }
        else if (this.Priority > other.Priority)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
   
     
    public void Reset()
    {
        Previous = null;
    }


}
