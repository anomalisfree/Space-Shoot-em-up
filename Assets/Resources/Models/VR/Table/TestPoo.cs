using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPoo : MonoBehaviour
{ 
    
    // Create a simple pool class:
// - pool shouldn't start with any pre-allocated objects of type T
// - pool should have limit to amount of objects stored (defined through constructor) - exception should be thrown when it's exceeded
// - Action<T> delegate (defined through constructor) should be called whenever object is taken from pool
// - Action<T> delegate (defined through constructor) should be called whenever object is returned to pool

    public class Pool<T> where T : new()
    {
        private static Action<T> isTaken;
        private static Action<T> isReturnd;
        
        private static List<T> _available = new List<T>();
        private static List<T> _inUse = new List<T>();

        
        public static T PooledObject()
        {
            lock(_available)
            {
                var po = new T();
                
                if (_available.Count != 0)
                {
                    po = _available[0];
                    _inUse.Add(po);
                    _available.RemoveAt(0);
                }
                else
                {
                    _inUse.Add(po);
                    return po;
                }
                
                isTaken?.Invoke(po);
                return po;
            }
        }
        
        public static void ReleaseObject(T po)
        {
            CleanUp(po);
 
            lock (_available)
            {
                _available.Add(po);
                _inUse.Remove(po);
                
                isReturnd?.Invoke(po);
            }
        }
 
        private static void CleanUp(T po)
        {
           //Clear T data
        }
    }
}
