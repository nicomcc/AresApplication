using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   
    public float m_Speed = 12f;                 // How fast the player moves forward and back.
    public float m_TurnSpeed = 180f;            // How fast the player turns in degrees per second.


    private Rigidbody m_Rigidbody;              // Reference used to move the player.
    private float m_MovementInputValue;         // The current value of the movement input.
    private float m_TurnInputValue;             // The current value of the turn input.

    private TCPServer client;

    public float currentTime = 0, previousVerticalTime = 0;
    public float moveDeltaTime = 200f;

    private bool getTimeOnce = true;

    private void Awake()
    {
        client = GameObject.FindWithTag("Server").GetComponent<TCPServer>();
    }

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move();
        Turn();
    }


    void Move()
    {
        // m_MovementInputValue = Input.GetAxis("Vertical");
        //client.ReadAndChangeInput('w', 's', currentTime, previousVerticalTime, moveDeltaTime);
       
        
        m_MovementInputValue = client.v_input;
        //client.setClientMessage("l");
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

        // Apply this movement to the rigidbody's position.
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);

        client.ReadInputOnlyOnce('w', 's', currentTime, previousVerticalTime, moveDeltaTime);
      //  currentTime = Time.time;
        /*if ((client.getClientMessage()[0] == 'w' || client.getClientMessage()[0] == 's'))
        {
            if (getTimeOnce)
            {
                previousVerticalTime = currentTime;
                getTimeOnce = false;
            }
            if (currentTime - previousVerticalTime > moveDeltaTime)
            {
                client.setClientMessage("l");
                client.v_input = 0f;
                previousVerticalTime = currentTime;
                getTimeOnce = true;
            }*/
        

        // client.v_input = 0;
    }


    void Turn()
    {
        // m_TurnInputValue = Input.GetAxis("Horizontal");


        m_TurnInputValue = client.h_input;
       // client.setClientMessage("l");
        // Determine the number of degrees to be turned based on the input, speed and time between frames.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        // Make this into a rotation in the y axis.
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        // Apply this rotation to the rigidbody's rotation.
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);

        client.ReadInputOnlyOnce('a', 'd', currentTime, previousVerticalTime, moveDeltaTime);
    }
}