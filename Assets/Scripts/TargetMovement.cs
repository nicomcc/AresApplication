using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    public float maxMoveSpeed = 5f;
    public float minMoveSpeed = 3f;
    public float maxFrequency = 10f;
    public float minFrequency = 2.5f;
    public float maxMagnitude  = 2f;
    public float minMagnitude = 0.5f;

    Vector3 pos, localScale;

    private GameObject plane;

    private bool verticalPositiveMovement, horizontalPositiveMovement; //will be randonly set as well

    Vector3 planeSize;

    private const float planeScaleConstant = 5; // plane scale is 10 compared to global one, so we use 10/2 constants

    private float randomVerticalIntensity, randomHorizontalIntensity; //random values so that senoidal movement can have any direction


    private void Awake()
    {
        plane = GameObject.FindWithTag("Plane");
    }

    void Start()
    {
        transform.position = new Vector3 (Random.Range(-plane.transform.localScale.x * planeScaleConstant, plane.transform.localScale.x * planeScaleConstant)
                                         ,transform.localScale.y/2
                                         ,Random.Range(-plane.transform.localScale.z * planeScaleConstant, plane.transform.localScale.z * planeScaleConstant));
        

        randomVerticalIntensity = Random.value;
        randomHorizontalIntensity = Random.value;

        verticalPositiveMovement = (Random.value > 0.5f);
        horizontalPositiveMovement = (Random.value > 0.5f);

        ////random values which will never be zero////
        maxMoveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        maxFrequency = Random.Range(minFrequency, maxFrequency);
        maxMagnitude = Random.Range(minMagnitude, maxMagnitude);

        planeSize = plane.transform.localScale * planeScaleConstant;    //set movement boundaries based on plane's size      

        pos = transform.position;

        localScale = transform.localScale;

    }

  
    void Update()
    {
        Debug.Log(-plane.transform.localScale.x * planeScaleConstant);
        Debug.Log(plane.transform.localScale.x * planeScaleConstant);
        SenoidalVerticalMove();
        SenoidalHorizontalMove();
    }


    void SenoidalHorizontalMove()
    {
        if (transform.position.x >= planeSize.x)
            horizontalPositiveMovement = false;
        else if (transform.position.x <= -planeSize.x)
            horizontalPositiveMovement = true;

        if (horizontalPositiveMovement)
            MoveRight();
        else
            MoveLeft();
    }
   

    void SenoidalVerticalMove()
    {
        if (transform.position.z >= planeSize.z)
            verticalPositiveMovement = false;
        else if (transform.position.z <= -planeSize.z)
            verticalPositiveMovement = true;

        if (verticalPositiveMovement)
            MoveUp();
        else
            MoveDown();
    }

    void MoveRight()
    {
        pos += transform.right * Time.deltaTime * maxMoveSpeed * randomHorizontalIntensity;
        transform.position = pos + transform.forward * Mathf.Sin(Time.time * maxFrequency) * maxMagnitude;

    }

    void MoveLeft()
    {
        pos -= transform.right * Time.deltaTime * maxMoveSpeed * randomHorizontalIntensity;
        transform.position = pos + transform.forward * Mathf.Sin(Time.time * maxFrequency) * maxMagnitude;
    }

    void MoveUp()
    {
    
        pos += transform.forward * Time.deltaTime * maxMoveSpeed * randomVerticalIntensity;
        transform.position = pos + transform.right * Mathf.Sin(Time.time * maxFrequency) * maxMagnitude;
    }

    void MoveDown()
    {
        pos -= transform.forward * Time.deltaTime * maxMoveSpeed * randomVerticalIntensity;
        transform.position = pos + transform.right * Mathf.Sin(Time.time * maxFrequency) * maxMagnitude;
    }
}
