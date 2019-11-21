using UnityEngine;

public class VehicleMovements
{
    public VehicleManager manager;

    public VehicleMovements(VehicleManager vm)
    {
        manager = vm;
    }

    public void Move()
    { 
        manager.rb.velocity = manager.transform.forward * manager.speed * Time.deltaTime;

        Vector3 eulerAngleVelocity = new Vector3(0, manager.steer * manager.maxAngleTurn, 0);
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
        manager.rb.MoveRotation(manager.rb.rotation * deltaRotation);
    }
}