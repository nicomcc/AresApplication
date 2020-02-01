using UnityEngine;

//It's not recommended to use a child object with rigidbody when the parent object also has rigidbody. 
//Therefore, another movement method will be use

public class BaseMovement : MonoBehaviour
{

    public float m_TurnSpeed = 100f;            // How fast weapon base will turn

    private float m_TurnInputValue;             // The current value of the turn input.

  
    void Turn()
    {
        m_TurnInputValue = Input.GetAxis("BaseRotation");
        transform.Rotate(0, m_TurnInputValue * Time.deltaTime * m_TurnSpeed , 0);
    }



    private void FixedUpdate()
    {
        Turn();
    }

}
