using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pool_scripts
{
    public class PrefabRandomSpawner : MonoBehaviour
    {
        [SerializeField] GameObject prefab;
        [SerializeField] float sphereRadius = 3f;
        
        PoolManager _poolManager;
        
        void Awake()
        {
            _poolManager = FindObjectOfType<PoolManager>();
        }
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                GameObject prefabInstance = _poolManager.Get(prefab);
                
                Vector3 randomPoint = Random.onUnitSphere * sphereRadius;
                prefabInstance.transform.position = transform.position + randomPoint;
                
                Quaternion randomRotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
                prefabInstance.transform.rotation = randomRotation;
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                _poolManager.ReleaseOldest(prefab);
            }
        }
        
        
    }
}