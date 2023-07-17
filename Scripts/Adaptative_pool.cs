using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adaptative_pool : MonoBehaviour
{
    // script Álvaro 

    public GameObject attack_Prefab, slow_attack_prefab;
    public int MaxSize;
    public List<GameObject> pool_list_1, pool_list_2; 

    void Start()
    {
        pool_list_1 = new List<GameObject>();
        for (int i = 0; i < MaxSize; i++)
        {
            GameObject obj = Instantiate(attack_Prefab);
            obj.SetActive(false);
            pool_list_1.Add(obj);
        }
        /*pool_list_2 = new List<GameObject>();
        for (int i = 0; i < MaxSize; i++)
        {
            GameObject obj = Instantiate(slow_attack_prefab);
            obj.SetActive(false);
            pool_list_2.Add(obj);
        }*/
    }
   
    void Update()
    {
        
    }
    
    public GameObject GetPoolObject_1()
    {
        for (int i = 0; i < MaxSize; i++)
        {
            if (!pool_list_1[i].activeInHierarchy)
            {
                return pool_list_1[i];
            }           
        }
        GameObject newObj = Instantiate(attack_Prefab);
        pool_list_1.Add(attack_Prefab);
        return newObj;
    }
    public GameObject GetPoolObject_2()
    {
        for (int i = 0; i < MaxSize; i++)
        {
            if (!pool_list_2[i].activeInHierarchy)
            {
                return pool_list_2[i];
            }           
        }
        GameObject newObj = Instantiate(slow_attack_prefab);
        pool_list_2.Add(slow_attack_prefab);
        return newObj;
    }

}
