using UnityEngine;

//It's not recommended to use a child object with rigidbody when the parent object also has rigidbody. 
//Therefore, another movement method will be use

public class BaseMovement : MonoBehaviour
{

    public float m_TurnSpeed = 100f;            // How fast weapon base will turn

    private float m_TurnInputValue;             // The current value of the turn input.


    private float currentTime = 0, previousVerticalTime = 0;
    public float moveDeltaTime = 0.05f;
    private TCPServer client;

    private void Awake()
    {
        client = GameObject.FindWithTag("Server").GetComponent<TCPServer>();
    }

    void Turn()
    {
        // m_TurnInputValue = Input.GetAxis("BaseRotation");
        m_TurnInputValue = client.base_input;

        transform.Rotate(0, m_TurnInputValue * Time.deltaTime * m_TurnSpeed , 0);

        client.ReadInputOnlyOnce('j', 'l', currentTime, previousVerticalTime, moveDeltaTime);
    }



    private void FixedUpdate()
    {
        Turn();
    }

}
