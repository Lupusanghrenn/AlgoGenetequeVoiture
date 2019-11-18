using UnityEngine;

public class VehicleMovements : MonoBehaviour
{
    public VehicleManager manager;
    public float maxAngleTurn = 30;

    [Range(-1, 1)] public float steer;
    [Range(-50, 500)] public float speed = 5f;

    public void Move()
    {
        manager.rb.velocity = transform.forward * speed * Time.deltaTime;

        Vector3 eulerAngleVelocity = new Vector3(0, steer * maxAngleTurn, 0);
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
        manager.rb.MoveRotation(manager.rb.rotation * deltaRotation);
    }
}
