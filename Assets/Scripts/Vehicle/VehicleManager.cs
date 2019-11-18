using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    public float maxRange = 10.0f;

    void Update()
    {
        DebugRaycast();
    }

    public void DebugRaycast()
    {
        //right max range vector
        Vector3 vectorRight = Quaternion.AngleAxis(45, Vector3.up) * transform.forward;

        //left max range vector
        Vector3 vectorLeft = Quaternion.AngleAxis(-45, Vector3.up) * transform.forward;
        
        //--Debug rays hit--
        //FRONT
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxRange))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.green);
            Debug.Log("Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * maxRange, Color.red);
        }

        //RIGHT
        if (Physics.Raycast(transform.position, vectorRight, out hit, maxRange))
        {
            Debug.DrawRay(transform.position, vectorRight * hit.distance, Color.green);
            Debug.Log("Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, vectorRight * maxRange, Color.red);
        }

        //LEFT
        if (Physics.Raycast(transform.position, vectorLeft, out hit, maxRange))
        {
            Debug.DrawRay(transform.position, vectorLeft * hit.distance, Color.green);
            Debug.Log("Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, vectorLeft * maxRange, Color.red);
        }
    }
}
