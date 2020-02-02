using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject target1, target2, target3;

    public int Targets;

    void Start()
    {
        int prefab;
        for(int i = 0; i < Targets; i++)
        {
           prefab = Random.Range(1, 4);
        if(prefab == 1)
           Instantiate(target1);
        if (prefab == 2)
                Instantiate(target2);
        if (prefab == 3)
                Instantiate(target3);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
