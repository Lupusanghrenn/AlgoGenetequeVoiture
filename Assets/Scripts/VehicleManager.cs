using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    public float speed = 2.0f;
    public float maxRange = 10.0f;
    public Transform Origin;

    private bool obstacleHit = false;

    void Update()
    {
        //VehiclesMovement();
        DebugRaycast();
    }

    public void DebugRaycast()
    {
        obstacleHit = false;
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
            obstacleHit = true;
        }
        else
        {
            Debug.DrawRay(Origin.position, transform.forward * maxRange, Color.red);
        }

        if (Physics.Raycast(Origin.position, vectorRight, out hit, maxRange))
        {
            Debug.DrawRay(Origin.position, vectorRight * hit.distance, Color.green);
            Debug.Log("Did Hit");
            obstacleHit = true;
        }
        else
        {
            Debug.DrawRay(Origin.position, vectorRight * maxRange, Color.red);
        }

        if (Physics.Raycast(Origin.position, vectorLeft, out hit, maxRange))
        {
            Debug.DrawRay(Origin.position, vectorLeft * hit.distance, Color.green);
            Debug.Log("Did Hit");
            obstacleHit = true;
        }
        else
        {
            Debug.DrawRay(Origin.position, vectorLeft * maxRange, Color.red);
        }
    }

    public void VehiclesMovement()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}
