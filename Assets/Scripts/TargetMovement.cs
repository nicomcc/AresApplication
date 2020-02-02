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

    public float maxHorizontalMoveSpeed = 20f;
    public float minHorizontalMoveSpeed = 10f;

    public float minRotationRadius = 2f;
    public float maxRotationRadius = 5f;
    public float maxAngularSpeed = 4f;
    public float minAngularSpeed = 2f;

    [SerializeField]
    private int movementBehaviour;

    private float posX, posZ, angle = 0f;

    Vector3 pos, localScale;

    private GameObject plane;

    private bool verticalPositiveMovement, horizontalPositiveMovement; //will be randonly set as well
    private bool clockWiseCircleMovement;

    Vector3 planeSize;

    private const float planeScaleConstant = 5; // plane scale is 10 compared to global one, so we use 10/2 constants

    private float randomVerticalIntensity, randomHorizontalIntensity; //random values so that senoidal movement can have any direction

    //preventing debouncing effect on circulat movement change
    public float moveDelta = 0.2F;
    private float previousTime = 0;

    private void Awake()
    {
        plane = GameObject.FindWithTag("Plane");
    }

    void Start()
    {
        movementBehaviour = Random.Range(1, 4);  //define movement behaviour between 1 and 3

        transform.position = new Vector3 (Random.Range(-plane.transform.localScale.x * planeScaleConstant, plane.transform.localScale.x * planeScaleConstant)
                                         ,transform.localScale.y/2
                                         ,Random.Range(-plane.transform.localScale.z * planeScaleConstant, plane.transform.localScale.z * planeScaleConstant));
        
        //if moviment is circular, instantiate prefab at max of 85% of plane's area
        if (movementBehaviour == 3)
            transform.position = new Vector3(Random.Range(-plane.transform.localScale.x * planeScaleConstant, plane.transform.localScale.x * planeScaleConstant) * 0.85f
                                         , transform.localScale.y / 2
                                         , Random.Range(-plane.transform.localScale.z * planeScaleConstant * 0.8f, plane.transform.localScale.z * planeScaleConstant) * 0.85f);

        //random directions
        randomVerticalIntensity = Random.value;
        randomHorizontalIntensity = Random.value;
        verticalPositiveMovement = (Random.value > 0.5f);
        horizontalPositiveMovement = (Random.value > 0.5f);
        clockWiseCircleMovement = (Random.value > 0.5f);        
        

         ////random values to create different movement patterns////
        maxMoveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        maxFrequency = Random.Range(minFrequency, maxFrequency);
        maxMagnitude = Random.Range(minMagnitude, maxMagnitude);
        maxAngularSpeed = Random.Range(minAngularSpeed, maxAngularSpeed);
        maxRotationRadius = Random.Range(minRotationRadius, maxRotationRadius);
        maxHorizontalMoveSpeed = Random.Range(minHorizontalMoveSpeed, maxHorizontalMoveSpeed);

        //set movement boundaries based on plane's size 
        planeSize = plane.transform.localScale * planeScaleConstant;        

        pos = transform.position;
        localScale = transform.localScale;
    }

  
    void Update()
    {
        switch(movementBehaviour)
        {
            case 1:
                SenoidalVerticalMove();
                SenoidalHorizontalMove();
                break;

            case 2:
                HorizontalHorizontalMove();
                HorizontalVerticalMove();
                break;

            case 3:
                CircularMove();
                break;
        }
    }

    void CircularMove()
    {

        if (clockWiseCircleMovement)  //clockwise movement 
        {
            posX = transform.position.x - Mathf.Cos(angle) * maxRotationRadius;
            posZ = transform.position.z - Mathf.Sin(angle) * maxRotationRadius;
            transform.position = new Vector3(posX, transform.localScale.y / 2, posZ);
            angle = angle - Time.deltaTime * maxAngularSpeed;
        }

        else //anticlockwise movement
        {
            posX = transform.position.x + Mathf.Cos(angle) * maxRotationRadius;
            posZ = transform.position.z + Mathf.Sin(angle) * maxRotationRadius;
            transform.position = new Vector3(posX, transform.localScale.y / 2, posZ);
            angle = angle + Time.deltaTime * maxAngularSpeed;
        }

        //debouning to prevent change direction lock bug        
        if (transform.position.z >= planeSize.z || transform.position.x >= planeSize.x || transform.position.z <= -planeSize.z || transform.position.x <= -planeSize.x)
        {
            if (Time.time - previousTime >= moveDelta)
            {
                previousTime = Time.time;
                clockWiseCircleMovement = !clockWiseCircleMovement;
            }            
        }

        //angles limit value
        if (angle > 360f)
            angle = 0f;
    }

    void HorizontalVerticalMove()
    {
        if (transform.position.z >= planeSize.z)  //change direction when reach boundary
            verticalPositiveMovement = false;
        else if (transform.position.z <= -planeSize.z)
            verticalPositiveMovement = true;

        if (verticalPositiveMovement)
        {
            pos += transform.forward * Time.deltaTime * maxHorizontalMoveSpeed * randomVerticalIntensity;
            transform.position = pos + transform.right;
        }

        else
        {
            pos -= transform.forward * Time.deltaTime * maxHorizontalMoveSpeed * randomVerticalIntensity;
            transform.position = pos + transform.right;
        }
    }

    void HorizontalHorizontalMove()
    {
        if (transform.position.x >= planeSize.x) //change direction when reach boundary
            horizontalPositiveMovement = false;
        else if (transform.position.x <= -planeSize.x)
            horizontalPositiveMovement = true;

        if (horizontalPositiveMovement)
        {
            pos += transform.right * Time.deltaTime * maxHorizontalMoveSpeed * randomHorizontalIntensity;
            transform.position = pos + transform.forward;
        }

        else
        {
            pos -= transform.right * Time.deltaTime * maxHorizontalMoveSpeed * randomHorizontalIntensity;
            transform.position = pos + transform.forward;
        }
    }

    void SenoidalHorizontalMove()
    {
        if (transform.position.x >= planeSize.x)
            horizontalPositiveMovement = false;
        else if (transform.position.x <= -planeSize.x)
            horizontalPositiveMovement = true;

        if (horizontalPositiveMovement)
            SenoidalMoveRight();
        else
            SenoidalMoveLeft();
    }
   

    void SenoidalVerticalMove()
    {
        if (transform.position.z >= planeSize.z)
            verticalPositiveMovement = false;
        else if (transform.position.z <= -planeSize.z)
            verticalPositiveMovement = true;

        if (verticalPositiveMovement)
            SenoidalMoveUp();
        else
            SenoidalMoveDown();
    }

    void SenoidalMoveRight()
    {
        pos += transform.right * Time.deltaTime * maxMoveSpeed * randomHorizontalIntensity;
        transform.position = pos + transform.forward * Mathf.Sin(Time.time * maxFrequency) * maxMagnitude;

    }

    void SenoidalMoveLeft()
    {
        pos -= transform.right * Time.deltaTime * maxMoveSpeed * randomHorizontalIntensity;
        transform.position = pos + transform.forward * Mathf.Sin(Time.time * maxFrequency) * maxMagnitude;
    }

    void SenoidalMoveUp()
    {
    
        pos += transform.forward * Time.deltaTime * maxMoveSpeed * randomVerticalIntensity;
        transform.position = pos + transform.right * Mathf.Sin(Time.time * maxFrequency) * maxMagnitude;
    }

    void SenoidalMoveDown()
    {
        pos -= transform.forward * Time.deltaTime * maxMoveSpeed * randomVerticalIntensity;
        transform.position = pos + transform.right * Mathf.Sin(Time.time * maxFrequency) * maxMagnitude;
    }
}
