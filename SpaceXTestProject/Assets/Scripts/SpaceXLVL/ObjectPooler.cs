using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;

    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;
    int leftInPool;
    GameObject objToSend;

    void Awake()
    {
        SharedInstance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        leftInPool = amountToPool;
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.transform.SetParent(this.transform);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public GameObject GetPooledObject()
    {        
        if (leftInPool > 0)
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    leftInPool--;
                    objToSend = pooledObjects[i];
                }
            }
        }
        else
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            amountToPool++;
            objToSend = obj;
        }
        return objToSend;
    }
    public void ReturnToPool()
    {
        leftInPool = amountToPool;
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = pooledObjects[i];
            obj.transform.SetParent(this.transform);
            obj.SetActive(false);
        }
    }
}
