using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool<T> where T : new()
{
    // Public Properties
    public int Count { get { return pool.Count; } }

    // Private Instance Variables
    LinkedList<T> pool;
    System.Func<T> newFunc;

    // Constructor
    public Pool()
    {
        newFunc = () => { return new T(); };
        pool = new LinkedList<T>();
    }

    // Constructor
    public Pool(System.Func<T> newFunc)
    {
        this.newFunc = newFunc;
        pool = new LinkedList<T>();
    }

    // Create the initial elements
    public void WarmUp(int size)
    {
        for (int i = 0; i < size; i++)
        {
            pool.AddLast(newFunc());
        }
    }

    // Grab an instance from the pool
    public T New()
    {
        var node = pool.First;
        if (node != null)
        {
            pool.Remove(node);
            return node.Value;
        }
        else
        {
            return newFunc();
        }
    }

    // Add a previously used item back to the pool
    public void Recycle(T t)
    {
        pool.AddLast(t);
    }

    // Clear the pool
    public void Flush()
    {
        pool.Clear();
    }
}
