
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    
    private Rigidbody rb;

    public float shootPower = 100f;
    public float selfDestroyTime = 10f;

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
        rb.AddForce(projectileDirection * shootPower);
        Destroy(gameObject, selfDestroyTime);     //Destroy after some time
    }
    
}
