
using UnityEngine;
using System;



public class CannonElevation : MonoBehaviour
{


    public float m_ElevateSpeed = 40f;            // How fast cannon will elevate
    private float m_ElevateInputValue;             // The current value of the elevate input.

    public float minAngle = -10;
    public float maxAngle = 60;

    private const int THRESHOLD = 20;

    private float currentTime = 0, previousVerticalTime = 0;
    public float moveDeltaTime = 0.05f;
    private TCPServer client;

    private void Awake()
    {
        client = GameObject.FindWithTag("Server").GetComponent<TCPServer>();
    }

    void Elevate()
    {

        //m_ElevateInputValue = Input.GetAxis("CannonElevation");
        m_ElevateInputValue = client.elevate_input;
        m_ElevateInputValue = m_ElevateInputValue * Time.deltaTime * m_ElevateSpeed;

        transform.Rotate(0, 0, -m_ElevateInputValue);

        client.ReadInputOnlyOnce('i', 'k', currentTime, previousVerticalTime, moveDeltaTime);

        //set maximum elevation angle
        if (transform.localEulerAngles.z > maxAngle && transform.localEulerAngles.z < maxAngle + THRESHOLD)
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, maxAngle);

        //set minimum elevation angle for two possibilities, over or below zero
        if (minAngle < 0)
        {
            if (transform.localEulerAngles.z <= 360 + minAngle && transform.localEulerAngles.z > 360 + minAngle - THRESHOLD)
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, minAngle);
        }
        else
            if (transform.localEulerAngles.z <= minAngle || transform.localEulerAngles.z > maxAngle + THRESHOLD)
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, minAngle);

    }


    private void FixedUpdate()
    {
        Elevate();
    }

    public float AngleValue()
    {
        return (transform.localEulerAngles.z > 180) ? transform.localEulerAngles.z - 360 : transform.localEulerAngles.z;
    }


    private void OnGUI()
    {
        GUI.contentColor = Color.black;
        GUILayout.Label("");
        GUILayout.Label(" Elevation: " + Math.Round(AngleValue(), 2));
    }

}
