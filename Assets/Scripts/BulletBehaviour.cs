
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    
    private Rigidbody rb;

    public float shootPower = 1300;
    public float selfDestroyTime = 10f;

    private GameObject BasePoint;
    private GameObject FirePoint;

    public bool destroyTargetIfRollingOnFloor = false;

    void Awake()
    {
        BasePoint = GameObject.FindWithTag("Respawn");
        FirePoint = GameObject.FindWithTag("FirePosition");
        rb = GetComponent<Rigidbody>();
    }

     void Start()
    {
        Vector3 projectileDirection = (FirePoint.transform.position - BasePoint.transform.position).normalized;
        rb.AddForce(projectileDirection * shootPower);
        Destroy(gameObject, selfDestroyTime);     //Destroy after some time
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Target")
        {
            if (!destroyTargetIfRollingOnFloor && transform.position.y > 0.3f)
            {
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }
            else if (destroyTargetIfRollingOnFloor)
            {                
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }
        }
    }

   

}
