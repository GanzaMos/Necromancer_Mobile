using System.Collections;
using System.Collections.Generic;
using Pool_scripts;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;
    PoolManager _poolManager;
    
    
    void Awake()
    {
        _poolManager = FindObjectOfType<PoolManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject arrowInstance = _poolManager.Get(arrowPrefab);
            //arrowInstance.SetActive(false);
            
            Transform arrowInstanceTransform = arrowInstance.transform;
            arrowInstanceTransform.position = transform.position;
            arrowInstanceTransform.rotation = transform.rotation;

            //arrowInstance.SetActive(true);
            
            ArrowHandler arrowHandler = arrowInstance.GetComponent<ArrowHandler>();
            arrowHandler.PoolManager = _poolManager;
            arrowHandler.LaunchArrow();
        }
    }
}
