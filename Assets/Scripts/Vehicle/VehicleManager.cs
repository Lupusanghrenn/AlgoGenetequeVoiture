using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    public float maxRange = 10.0f;

    public Transform pivot;

    void Update()
    {
        DebugRaycast();
    }

    public void DebugRaycast()
    {
        //right max range vector
        Vector3 vectorRight = Quaternion.AngleAxis(45, Vector3.up) * pivot.forward;

        //left max range vector
        Vector3 vectorLeft = Quaternion.AngleAxis(-45, Vector3.up) * pivot.forward;
        
        //--Debug rays hit--
        //FRONT
        RaycastHit hit;
        if (Physics.Raycast(pivot.position, pivot.forward, out hit, maxRange))
        {
            Debug.DrawRay(pivot.position, pivot.forward * hit.distance, Color.green);
            Debug.Log("Hit");
        }
        else
        {
            Debug.DrawRay(pivot.position, pivot.forward * maxRange, Color.red);
        }

        //RIGHT
        if (Physics.Raycast(pivot.position, vectorRight, out hit, maxRange))
        {
            Debug.DrawRay(pivot.position, vectorRight * hit.distance, Color.green);
            Debug.Log("Hit");
        }
        else
        {
            Debug.DrawRay(pivot.position, vectorRight * maxRange, Color.red);
        }

        //LEFT
        if (Physics.Raycast(pivot.position, vectorLeft, out hit, maxRange))
        {
            Debug.DrawRay(pivot.position, vectorLeft * hit.distance, Color.green);
            Debug.Log("Hit");
        }
        else
        {
            Debug.DrawRay(pivot.position, vectorLeft * maxRange, Color.red);
        }
    }
}
