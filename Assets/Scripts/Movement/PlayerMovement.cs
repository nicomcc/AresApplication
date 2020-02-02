using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   
    public float m_Speed = 12f;                 // How fast the player moves forward and back.
    public float m_TurnSpeed = 180f;            // How fast the player turns in degrees per second.


    private Rigidbody m_Rigidbody;              // Reference used to move the player.
    private float m_MovementInputValue;         // The current value of the movement input.
    private float m_TurnInputValue;             // The current value of the turn input.


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
        m_MovementInputValue = Input.GetAxis("Vertical");

        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

        // Apply this movement to the rigidbody's position.
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }
 

     void Turn()
    {
        m_TurnInputValue = Input.GetAxis("Horizontal");

        // Determine the number of degrees to be turned based on the input, speed and time between frames.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        // Make this into a rotation in the y axis.
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        // Apply this rotation to the rigidbody's rotation.
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }
}