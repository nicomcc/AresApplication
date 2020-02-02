using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotControl : MonoBehaviour
{
    public GameObject projectile;

    public float fireDelta = 0.5F;
    private float nextFire = 0.5F;
    private GameObject newProjectile;
    private float myTime = 0.0F;

    public int shotsFired = 0;

   

    // Update is called once per frame
    void Update()
    {
        myTime = myTime + Time.deltaTime;

        if (Input.GetButton("Fire1") && myTime > nextFire)
        { 
             nextFire = myTime + fireDelta;
             newProjectile = Instantiate(projectile, transform.position, transform.rotation) as GameObject;

        // create code here that animates the newProjectile

             nextFire = nextFire - myTime;
             myTime = 0.0F;
            shotsFired++;
        }
    }
}
