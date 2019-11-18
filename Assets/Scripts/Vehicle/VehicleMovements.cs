using UnityEngine;

public class VehicleMovements : MonoBehaviour
{
    public Rigidbody rb;
    [Range(0, 100)]
    public float speed = 0.5f;
    public float maxAngleTurn = 30;

    [Range(-1, 1)]
    public float steer;
       
    private void FixedUpdate()
    {
        rb.velocity = transform.forward * speed * Time.deltaTime;

        Vector3 eulerAngleVelocity = new Vector3(0, steer * maxAngleTurn, 0);
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}
