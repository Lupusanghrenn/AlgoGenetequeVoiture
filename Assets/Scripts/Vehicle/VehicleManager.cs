using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    public VehicleMovements vehicleMovements;
    public Rigidbody rb;
    public Transform pivot;

    public float maxRangeDebugRay = 10.0f;

    public bool collision = false;
    public float fitness = 0f;

    [Header("Parametre Raycast")]
    public GameObject cylindreFront;
    public GameObject cylindreLeft;
    public GameObject cylindreRight;

    private Material red;
    private Material green;

    private void Awake()
    {
        red = Resources.Load<Material>("Materials/Red");
        green = Resources.Load<Material>("Materials/Green");
    }

    void Update()
    {
        Raycast();
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

    public void Raycast()
    {
        Vector3 vectorRight = Quaternion.AngleAxis(45, Vector3.up) * transform.forward;
        Vector3 vectorLeft = Quaternion.AngleAxis(-45, Vector3.up) * transform.forward;

        //Debug rays hit
        RaycastHit hitFront;
        RaycastHit hitLeft;
        RaycastHit hitRight;
        //FRONT
        if (Physics.Raycast(pivot.position, transform.forward, out hitFront, maxRangeDebugRay))
        {
            cylindreFront.transform.position = pivot.transform.position + 0.5f * pivot.forward * hitFront.distance;
            Vector3 scale = cylindreFront.transform.localScale;
            scale.y = (0.5f * pivot.forward * hitFront.distance).magnitude;
            cylindreFront.transform.localScale = scale;
            cylindreFront.GetComponent<MeshRenderer>().material = green;
            Debug.Log("Did Hit Front");
        }
        else
        {
            cylindreFront.transform.position = pivot.transform.position + 0.5f * pivot.forward * maxRangeDebugRay;
            Vector3 scale = cylindreFront.transform.localScale;
            scale.y = (0.5f * pivot.forward * maxRangeDebugRay).magnitude;
            cylindreFront.transform.localScale = scale;
            cylindreFront.GetComponent<MeshRenderer>().material = red;
        }

        //RIGHT
        if (Physics.Raycast(pivot.position, vectorRight, out hitRight, maxRangeDebugRay))
        {
            cylindreRight.transform.position = pivot.transform.position + 0.5f * vectorRight * hitRight.distance;
            Vector3 scale = cylindreRight.transform.localScale;
            scale.y = (0.5f * vectorRight * hitRight.distance).magnitude;
            cylindreRight.transform.localScale = scale;
            cylindreRight.GetComponent<MeshRenderer>().material = green;
            Debug.Log("Did Hit");
        }
        else
        {
            cylindreRight.transform.position = pivot.transform.position + 0.5f * vectorRight * maxRangeDebugRay;
            Vector3 scale = cylindreRight.transform.localScale;
            scale.y = (0.5f * vectorRight * maxRangeDebugRay).magnitude;
            cylindreRight.transform.localScale = scale;
            cylindreRight.GetComponent<MeshRenderer>().material = red;
        }

        //LEFT
        if (Physics.Raycast(pivot.position, vectorLeft, out hitLeft, maxRangeDebugRay))
        {
            cylindreLeft.transform.position = pivot.transform.position + 0.5f * vectorLeft * hitLeft.distance;
            Vector3 scale = cylindreLeft.transform.localScale;
            scale.y = (0.5f * vectorLeft * hitLeft.distance).magnitude;
            cylindreLeft.transform.localScale = scale;
            cylindreLeft.GetComponent<MeshRenderer>().material = green;
            Debug.Log("Did Hit");
        }
        else
        {
            cylindreLeft.transform.position = pivot.transform.position + 0.5f * vectorLeft * maxRangeDebugRay;
            Vector3 scale = cylindreLeft.transform.localScale;
            scale.y = (0.5f * vectorLeft * maxRangeDebugRay).magnitude;
            cylindreLeft.transform.localScale = scale;
            cylindreLeft.GetComponent<MeshRenderer>().material = red;
            Debug.DrawRay(pivot.position, vectorLeft * maxRangeDebugRay, Color.red);
        }
    }
}
