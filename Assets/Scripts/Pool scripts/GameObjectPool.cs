using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : PoolBase<GameObject>
{ 
    static Transform _parentTransform;
    
    public GameObjectPool(GameObject prefab, Transform parentTransform, int instancesAtStart, int maxInstanceNumber) : 
        base(() => CreateNew(prefab), GetAction, ReleaseAction)
    {
        _parentTransform = parentTransform;
        MaxInstanceNumber = maxInstanceNumber;
        base.CreateStartInstances(instancesAtStart);
    }

    public static GameObject CreateNew(GameObject prefab)
    {
        GameObject instance = Object.Instantiate(prefab);
        instance.transform.SetParent(_parentTransform);
        instance.name = prefab.name;
        return instance;
    }
    
    public static void GetAction(GameObject @object) => @object.SetActive(true);

    public static void ReleaseAction(GameObject @object)
    {
        @object.SetActive(false);
        @object.transform.SetParent(_parentTransform);
    }
}
