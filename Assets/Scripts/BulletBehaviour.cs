using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    
    private Rigidbody rb;

    public float shootPower = 100f;

    private GameObject BasePoint;
    private GameObject FirePoint;

    void Awake()
    {
        BasePoint = GameObject.FindWithTag("Respawn");
        FirePoint = GameObject.FindWithTag("FirePosition");
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Vector3 projectileDirection = (FirePoint.transform.position - BasePoint.transform.position).normalized;
        //Vector3 projectileDirection = BasePoint.transform.bac;
        rb.AddForce(projectileDirection * shootPower);


        Debug.Log(FirePoint.transform.position);
        Debug.Log(BasePoint.transform.position);
        Debug.Log(projectileDirection);
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
