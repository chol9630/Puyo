using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
       public string tag;
       public int size;
       public GameObject PreFab;
        
    }

    public List<Pool> listPool;
    public Dictionary<string, Queue<GameObject>> DictionaryPool;
    public static ObjectPool Instance;

    private void Awake()
    {
        if(Instance ==null)
        {
            Instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
        DictionaryPool = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in listPool )
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i=0; i<pool.size; i++)
            {
                GameObject go = Instantiate(pool.PreFab);
                go.SetActive(false);
                objectPool.Enqueue(go);

            }
            DictionaryPool.Add(pool.tag ,objectPool);

        }


    }
    
   public GameObject SpawnPool(string tag , Vector3 Postion , Quaternion rotation)
    {

        if(!DictionaryPool.ContainsKey(tag))
        {
            return null;
        }

        GameObject objectFromPool = DictionaryPool[tag].Dequeue();

        if(objectFromPool.activeInHierarchy ==true)
        {
            DictionaryPool[tag].Enqueue(objectFromPool);
            objectFromPool = Instantiate(objectFromPool);
        }

        objectFromPool.SetActive(true);
        objectFromPool.transform.position = Postion;
        objectFromPool.transform.rotation = rotation;

        

        DictionaryPool[tag].Enqueue(objectFromPool);


        return objectFromPool;
    }


}
