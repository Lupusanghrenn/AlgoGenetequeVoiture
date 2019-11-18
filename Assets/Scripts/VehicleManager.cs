using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    public float maxRange = 10.0f;
    public Transform Origin;

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
        
        //Debug rays hit
        RaycastHit hit;
        if (Physics.Raycast(Origin.position, transform.forward, out hit, maxRange))
        {
            Debug.DrawRay(Origin.position, transform.forward * hit.distance, Color.green);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(Origin.position, transform.forward * maxRange, Color.red);
        }

        if (Physics.Raycast(Origin.position, vectorRight, out hit, maxRange))
        {
            Debug.DrawRay(Origin.position, vectorRight * hit.distance, Color.green);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(Origin.position, vectorRight * maxRange, Color.red);
        }

        if (Physics.Raycast(Origin.position, vectorLeft, out hit, maxRange))
        {
            Debug.DrawRay(Origin.position, vectorLeft * hit.distance, Color.green);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(Origin.position, vectorLeft * maxRange, Color.red);
        }
    }
}
