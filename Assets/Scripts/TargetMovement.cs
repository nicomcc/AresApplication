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

    Vector3 pos, localScale;

    private GameObject plane;

    private bool verticalPositiveMovement, horizontalPositiveMovement; //will be randonly set as well

    Vector3 planeSize;

    private const float planeScaleConstant = 5; // plane scale is 10 compared to global one, so we use 10/2 constants

    [SerializeField]
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

        maxHorizontalMoveSpeed = Random.Range(minHorizontalMoveSpeed, maxHorizontalMoveSpeed);

        planeSize = plane.transform.localScale * planeScaleConstant;    //set movement boundaries based on plane's size      

        pos = transform.position;

        localScale = transform.localScale;

    }

  
    void Update()
    {
        //SenoidalVerticalMove();
        // SenoidalHorizontalMove();
        HorizontalHorizontalMove();
        HorizontalVerticalMove();
    }

    void HorizontalVerticalMove()
    {
        if (transform.position.z >= planeSize.z)
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
        if (transform.position.x >= planeSize.x)
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
