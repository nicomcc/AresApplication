using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject target1, target2, target3;

    public int Targets;

   //public GameInspector shotsControlInspec;
    public GameObject shotControl;
    public ShotControl shotControlInspec;


    

void Start()
    {
        shotControlInspec = shotControl.GetComponent<ShotControl>();

       

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



    private int CountTargets()
    {
         GameObject[] thingyToFind = GameObject.FindGameObjectsWithTag("Target");
         return thingyToFind.Length;
    }


    private void Update()
    {
       // Debug.Log(CountTargets());
    }

    private void OnGUI()
    {
        GUI.contentColor = Color.black;
        GUILayout.Label("");
        GUILayout.Label("Shots Fired: " + shotControlInspec.shotsFired);
        GUILayout.Label("Targets Left: " + CountTargets() + "/" + Targets);
    }
}
