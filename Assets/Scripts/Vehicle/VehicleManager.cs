using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    public VehicleMovements vehicleMovements;
    public Rigidbody rb;
    public Transform pivot;

    public float maxRangeDebugRay = 10.0f;

    public bool collision = false;
    public float distanceTravelled = 0f;

    void Update()
    {
        DebugRaycast();
    }

    private void FixedUpdate()
    {
        if (!collision)
        {
            vehicleMovements.Move();
        }
        else
            rb.velocity = new Vector3();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Circuit_Wall")
            collision = true;
    }

    public void DebugRaycast()
    {
        Vector3 vectorRight = Quaternion.AngleAxis(45, Vector3.up) * pivot.forward;
        Vector3 vectorLeft = Quaternion.AngleAxis(-45, Vector3.up) * pivot.forward;
        
        //--Debug rays hit--
        //FRONT
        RaycastHit hit;
        if (Physics.Raycast(pivot.position, pivot.forward, out hit, maxRangeDebugRay))
        {
            Debug.DrawRay(pivot.position, pivot.forward * hit.distance, Color.green);
        }
        else
        {
            Debug.DrawRay(pivot.position, pivot.forward * maxRangeDebugRay, Color.red);
        }

        //RIGHT
        if (Physics.Raycast(pivot.position, vectorRight, out hit, maxRangeDebugRay))
        {
            Debug.DrawRay(pivot.position, vectorRight * hit.distance, Color.green);
        }
        else
        {
            Debug.DrawRay(pivot.position, vectorRight * maxRangeDebugRay, Color.red);
        }

        //LEFT
        if (Physics.Raycast(pivot.position, vectorLeft, out hit, maxRangeDebugRay))
        {
            Debug.DrawRay(pivot.position, vectorLeft * hit.distance, Color.green);
        }
        else
        {
            Debug.DrawRay(pivot.position, vectorLeft * maxRangeDebugRay, Color.red);
        }
    }
}
