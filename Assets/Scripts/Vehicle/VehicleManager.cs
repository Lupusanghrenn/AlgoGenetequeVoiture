using System;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    [Header("General parameter")]
    public VehicleMovements vehicleMovements;
    public Rigidbody rb;

    public NeuralNetwork neuralNetwork;

    public bool collision = false;
    public float fitness = 0f;
    public int indexCheckPoint = 0;

    [Header("Parametre Raycast")]
    public float maxRangeDebugRay = 10.0f;
    public Transform pivot;

    private GameObject cylindreRight;
    private GameObject cylindreExtraLeft;
    private GameObject cylindreFront;
    private GameObject cylindreLeft;
    private GameObject cylindreExtraRight;

    RaycastHit hitFront;
    RaycastHit hitLeft;
    RaycastHit hitRight;
    RaycastHit hitExtraLeft;
    RaycastHit hitExtraRight;

    private Material red;
    private Material green;

    public float distanceParcouru;

    public List<Transform> checkpoints;

    [Header("Movement")]
    public float maxAngleTurn = 30;
    [Range(-1, 1)] public float steer;
    [Range(-50, 500)] public float speed = 5f;

    private float timeCreation;
    private Vector3 initPos;

    private void Awake()
    {
        vehicleMovements = new VehicleMovements(this);

        neuralNetwork = new NeuralNetwork();

        //timeCreation = Time.time;

        initPos = transform.position;

        InitRayCast();
    }

    void InitRayCast()
    {

        red = Resources.Load<Material>("Materials/Red");
        green = Resources.Load<Material>("Materials/Green");

        MeshRenderer[] meshes = pivot.GetComponentsInChildren<MeshRenderer>();
        GameObject[] cylinder = new GameObject[5];

        for (int i = 0; i < meshes.Length; i++)
        {
            cylinder[i] = meshes[i].gameObject;
        }
        cylindreFront = cylinder[0];
        cylindreLeft = cylinder[1];
        cylindreRight = cylinder[2];
        cylindreExtraLeft = cylinder[3];
        cylindreExtraRight = cylinder[4];
    }
    
    void Update()
    {
        Raycast();
        float dhf = hitFront.distance / maxRangeDebugRay;
        float dhl = hitLeft.distance / maxRangeDebugRay;
        float dhr = hitRight.distance / maxRangeDebugRay;
        float dhel = hitExtraLeft.distance / maxRangeDebugRay;
        float dher = hitExtraRight.distance / maxRangeDebugRay;

        float[] inputs = { dhf, dhl, dhr, dhel, dher };

        neuralNetwork.FeedInputs(inputs);
        steer = Mathf.Clamp(neuralNetwork.outputs[0].activation, -1.0f, 1.0f);
    }

    private void FixedUpdate()
    {
        if (!collision)
        {
            vehicleMovements.Move();
            fitness = UpdateFitness();
        }
        else
        {
            rb.velocity = new Vector3();
        }
            
    }

    //distance parcouru
    private float UpdateFitness()
    {
        float result = (transform.position - initPos).magnitude;
        return result;
    }

    void reachCheckpoint(int indiceChecpoints)
    {
        
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
        Vector3 vectorExtraLeft = Quaternion.AngleAxis(-90, Vector3.up) * transform.forward;
        Vector3 vectorExtraRight = Quaternion.AngleAxis(90, Vector3.up) * transform.forward;

        //FRONT
        if (Physics.Raycast(pivot.position, transform.forward, out hitFront, maxRangeDebugRay))
        {
            cylindreFront.transform.position = pivot.transform.position + 0.5f * pivot.forward * hitFront.distance;
            Vector3 scale = cylindreFront.transform.localScale;
            scale.y = (0.5f * pivot.forward * hitFront.distance).magnitude;
            cylindreFront.transform.localScale = scale;
            cylindreFront.GetComponent<MeshRenderer>().material = green;
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
        }
        else
        {
            cylindreLeft.transform.position = pivot.transform.position + 0.5f * vectorLeft * maxRangeDebugRay;
            Vector3 scale = cylindreLeft.transform.localScale;
            scale.y = (0.5f * vectorLeft * maxRangeDebugRay).magnitude;
            cylindreLeft.transform.localScale = scale;
            cylindreLeft.GetComponent<MeshRenderer>().material = red;
        }

        //EXTRA LEFT
        if (Physics.Raycast(pivot.position, vectorExtraLeft, out hitExtraLeft, maxRangeDebugRay))
        {
            cylindreExtraLeft.transform.position = pivot.transform.position + 0.5f * vectorExtraLeft * hitExtraLeft.distance;
            Vector3 scale = cylindreExtraLeft.transform.localScale;
            scale.y = (0.5f * vectorExtraLeft * hitExtraLeft.distance).magnitude;
            cylindreExtraLeft.transform.localScale = scale;
            cylindreExtraLeft.GetComponent<MeshRenderer>().material = green;
        }
        else
        {
            cylindreExtraLeft.transform.position = pivot.transform.position + 0.5f * vectorExtraLeft * maxRangeDebugRay;
            Vector3 scale = cylindreExtraLeft.transform.localScale;
            scale.y = (0.5f * vectorExtraLeft * maxRangeDebugRay).magnitude;
            cylindreExtraLeft.transform.localScale = scale;
            cylindreExtraLeft.GetComponent<MeshRenderer>().material = red;
        }

        //EXTRA RIGHT
        if (Physics.Raycast(pivot.position, vectorExtraRight, out hitExtraRight, maxRangeDebugRay))
        {
            cylindreExtraRight.transform.position = pivot.transform.position + 0.5f * vectorExtraRight * hitExtraRight.distance;
            Vector3 scale = cylindreExtraRight.transform.localScale;
            scale.y = (0.5f * vectorExtraRight * hitExtraRight.distance).magnitude;
            cylindreExtraRight.transform.localScale = scale;
            cylindreExtraRight.GetComponent<MeshRenderer>().material = green;
        }
        else
        {
            cylindreExtraRight.transform.position = pivot.transform.position + 0.5f * vectorExtraRight * maxRangeDebugRay;
            Vector3 scale = cylindreExtraRight.transform.localScale;
            scale.y = (0.5f * vectorExtraRight * maxRangeDebugRay).magnitude;
            cylindreExtraRight.transform.localScale = scale;
            cylindreExtraRight.GetComponent<MeshRenderer>().material = red;
        }
    }

    void updateRecompense()
    {
        //calcul du temps vivant (tant que pas de collision)
        float dist = rb.velocity.z * Time.deltaTime * speed;
        distanceParcouru+=Time.deltaTime + dist;
    }

}