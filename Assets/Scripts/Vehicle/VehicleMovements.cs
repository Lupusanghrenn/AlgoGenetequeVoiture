using UnityEngine;

public class VehicleMovements : MonoBehaviour
{
    public Rigidbody rb;
    [Range(0, 1)]
    public float speed = 0.5f;

    [Range(-1, 1)]
    public float steer;
       
    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + new Vector3(0, 0, speed));

        
    }
}
