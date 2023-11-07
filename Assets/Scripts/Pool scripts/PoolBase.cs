using System;
using System.Collections.Generic;

public class PoolBase<T>
{
    readonly Func<T> _createNew;
    readonly Action<T> _getAction;
    readonly Action<T> _releaseAction;

    Queue<T> _pool = new Queue<T>();
    List<T> _active = new List<T>();

    protected int MaxInstanceNumber = int.MaxValue;

    public PoolBase(
        Func<T> createNew,
        Action<T> getAction,
        Action<T> releaseAction,
        int instancesAtStart,
        int maxInstanceNumber)
    {
        _createNew = createNew;
        _getAction = getAction;
        _releaseAction = releaseAction;
        CreateStartInstances(instancesAtStart);
        MaxInstanceNumber = maxInstanceNumber;
    }
    
    public PoolBase(
        Func<T> createNew,
        Action<T> getAction,
        Action<T> releaseAction)
    {
        _createNew = createNew;
        _getAction = getAction;
        _releaseAction = releaseAction;
    }

    protected void CreateStartInstances(int instancesAtStart)
    {
        for (int i = 0; i < instancesAtStart; i++)
        {
            Release(_createNew());
        }
    }

    public T Get()
    {
        T item;

        //too many items in the scene?
        if (_active.Count >= MaxInstanceNumber) 
        {
            item = _active[0]; //our item now = the oldest item in the Active List
            _active.RemoveAt(0); //removing this oldest item from the beginning of the Active List
            _releaseAction(item); //deactivating it
        }
        //have any item in the Inactive Queue?
        else if (_pool.Count > 0) 
        {
            item = _pool.Dequeue(); //get it from there
        }
        //no items in the Inactive Queue?
        else 
        {
            item = _createNew(); //create one          
        }

        _getAction(item); //activate item
        _active.Add(item); //add it to the Active List

        return item;
    }

    public void Release(T item)
    {
        _releaseAction(item);
        _pool.Enqueue(item);
        _active.Remove(item);
    }

    public void ReleaseOldest()
    {
        if (_active.Count > 0)
        {
            var item = _active[0];  
            _releaseAction(item);
            _active.RemoveAt(0);
            _pool.Enqueue(item);  
        }
    }

    public void ReleaseAll()
    {
        foreach (T item in _active.ToArray())
        {
            Release(item);
        }
    }
    
    public void ChangeMaxInstanceNumber(int maxInstanceNumber) => this.MaxInstanceNumber = maxInstanceNumber;
}

