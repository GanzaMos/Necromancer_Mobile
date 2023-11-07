using System.Collections.Generic;
using UnityEngine;

namespace Pool_scripts
{
    public class PoolManager : MonoBehaviour
    {
        Dictionary<string, GameObjectPool> _poolDict = new Dictionary<string, GameObjectPool>();
        public int instancesAtStart = 50;
        public int maxInstanceNumber = 500;
        
        public GameObject Get(GameObject prefab)
        {
            CheckIsPoolCreated(prefab);
            return _poolDict[prefab.name].Get();
        }
        
        public void Release(GameObject prefab)
        {
            CheckIsPoolCreated(prefab);
            _poolDict[prefab.name].Release(prefab);
        }
        
        public void ReleaseOldest(GameObject prefab)
        {
            CheckIsPoolCreated(prefab);
            _poolDict[prefab.name].ReleaseOldest();
        }

        public void ChangePoolMaxInstanceNumber(GameObject prefab, int maxInstanceNumber)
        {
            _poolDict[prefab.name].ChangeMaxInstanceNumber(maxInstanceNumber);
        }
        
        void CheckIsPoolCreated(GameObject prefab)
        {
            if (_poolDict.ContainsKey(prefab.name) == false)
            {
                CreateNewPool(prefab);
            }
        }

        void CreateNewPool(GameObject prefab)
        {
            // create a new game object, which will be the parent for objects from the pool
            GameObject poolAnchorObject = new GameObject(prefab.name + " Pool"); 
            
            //set it as a child of game object of this script
            poolAnchorObject.transform.parent = transform;
            
            //create a pool for this Prefab
            _poolDict[prefab.name] = new GameObjectPool(prefab, poolAnchorObject.transform, this.instancesAtStart, this.maxInstanceNumber);
        }
    }
}