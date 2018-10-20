using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

 
public class PriorityQueue<T> where T : IComparable<T>
{
     
    private readonly List<T> _data;

     
    public int Count { get { return _data.Count; }}

     
    public PriorityQueue()
    {
        this._data = new List<T>();
    }

     
    public void Enqueue(T item)
    {
         
        _data.Add(item);

         
        int childindex = _data.Count - 1;

         
        while (childindex > 0)
        {
             
            int parentindex = (childindex - 1) / 2;

             
            if (_data[childindex].CompareTo(_data[parentindex]) >= 0)
            {
                break;
            }

             
            T tmp = _data[childindex];
            _data[childindex] = _data[parentindex];
            _data[parentindex] = tmp;

             
            childindex = parentindex;

        }
    }

     
    public T Dequeue()
    {
         
        int lastindex = _data.Count - 1;

         
        T frontItem = _data[0];

         
        _data[0] = _data[lastindex];

         
        _data.RemoveAt(lastindex);

         
        lastindex--;

         
        int parentindex = 0;

         
        while (true)
        {
             
            int childindex = parentindex * 2 + 1;

             
            if (childindex > lastindex)
            {
                break;
            }

             
            int rightchild = childindex + 1;

             
            if (rightchild <= lastindex && _data[rightchild].CompareTo(_data[childindex]) < 0)
            {
                childindex = rightchild;
            }

             
            if (_data[parentindex].CompareTo(_data[childindex]) <= 0)
            {
                break;
            }

             
            T tmp = _data[parentindex];
            _data[parentindex] = _data[childindex];
            _data[childindex] = tmp;

             
            parentindex = childindex;

        }

         
        return frontItem;
    }

     
    public T Peek()
    {
        T frontItem = _data[0];
        return frontItem;
    }

     
    public bool Contains(T item)
    {
        return _data.Contains(item);
    }

     
    public List<T> ToList()
    {
        return _data;
    }

}
