using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiclesParam : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 2.0f;
    public float maxRange = 10.0f;
    public Transform Origin;

    public GameObject cylindreFront;
    public GameObject cylindreLeft;
    public GameObject cylindreRight;

    public Material red;
    public Material green;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //VehiclesMovement();
        Raycast();
    }

    public void Raycast()
    {
        Vector3 vectorRight = Quaternion.AngleAxis(45, Vector3.up) * transform.forward;
        Vector3 vectorLeft = Quaternion.AngleAxis(-45, Vector3.up) * transform.forward;

        //Debug rays hit
        RaycastHit hit;
        //FRONT
        if (Physics.Raycast(Origin.position, transform.forward, out hit, maxRange))
        {
            cylindreFront.transform.position = Origin.transform.position + 0.5f * Origin.forward * hit.distance;
            Vector3 scale = cylindreFront.transform.localScale;
            scale.y = (0.5f * Origin.forward * hit.distance).magnitude;
            cylindreFront.transform.localScale = scale;
            cylindreFront.GetComponent<MeshRenderer>().material = green;
            Debug.Log("Did Hit Front");
        }
        else
        {
            cylindreFront.transform.position = Origin.transform.position + 0.5f * Origin.forward * maxRange;
            Vector3 scale = cylindreFront.transform.localScale;
            scale.y = (0.5f * Origin.forward * maxRange).magnitude;
            cylindreFront.transform.localScale = scale;
            cylindreFront.GetComponent<MeshRenderer>().material = red;
        }

        //RIGHT
        if (Physics.Raycast(Origin.position, vectorRight, out hit, maxRange))
        {
            cylindreRight.transform.position = Origin.transform.position + 0.5f * vectorRight * hit.distance;
            Vector3 scale = cylindreRight.transform.localScale;
            scale.y = (0.5f * vectorRight * hit.distance).magnitude;
            cylindreRight.transform.localScale = scale;
            cylindreRight.GetComponent<MeshRenderer>().material = green;
            Debug.Log("Did Hit");
        }
        else
        {
            cylindreRight.transform.position = Origin.transform.position + 0.5f * vectorRight * maxRange;
            Vector3 scale = cylindreRight.transform.localScale;
            scale.y = (0.5f * vectorRight * maxRange).magnitude;
            cylindreRight.transform.localScale = scale;
            cylindreRight.GetComponent<MeshRenderer>().material = red;
        }

        //LEFT
        if (Physics.Raycast(Origin.position, vectorLeft, out hit, maxRange))
        {
            cylindreLeft.transform.position = Origin.transform.position + 0.5f * vectorLeft * hit.distance;
            Vector3 scale = cylindreLeft.transform.localScale;
            scale.y = (0.5f * vectorLeft * hit.distance).magnitude;
            cylindreLeft.transform.localScale = scale;
            cylindreLeft.GetComponent<MeshRenderer>().material = green;
            Debug.Log("Did Hit");
        }
        else
        {
            cylindreLeft.transform.position = Origin.transform.position + 0.5f * vectorLeft * maxRange;
            Vector3 scale = cylindreLeft.transform.localScale;
            scale.y = (0.5f * vectorLeft * maxRange).magnitude;
            cylindreLeft.transform.localScale = scale;
            cylindreLeft.GetComponent<MeshRenderer>().material = red;
            Debug.DrawRay(Origin.position, vectorLeft * maxRange, Color.red);
        }

        /*RaycastHit hit;
        if (Physics.Raycast(Origin.position, transform.forward, out hit, maxRange))
        {
            Debug.DrawRay(Origin.position, transform.forward * hit.distance, Color.green);
            Debug.Log("Did Hit");
        }
        else
        {
            *//*LineRenderer lr = GetComponent<LineRenderer>();
            lr.positionCount = 4;
            lr.SetPosition(0, Origin.position);
            lr.SetPosition(1, Origin.transform.position + Origin.forward * maxRange);
            lr.SetPosition(2, Origin.position);
            lr.SetPosition(3, Origin.transform.position + vectorRight * maxRange);*//*
            //GameObject cylinderFront = Instantiate(cylindre, Origin.transform.position + 0.5f * Origin.forward * maxRange, Quaternion.identity);
            //cylinderFront.transform.position = Origin.transform.position + 0.5f * Origin.forward * maxRange;
            *//*Vector3 scale = cylinderFront.transform.localScale;
            scale.y= (0.5f * Origin.forward * maxRange).magnitude;
            cylinderFront.transform.localScale = scale; *//*
            
            Debug.Log(vectorRight * maxRange);
            Debug.DrawRay(Origin.position, vectorRight * maxRange, Color.red);
            Debug.DrawRay(Origin.position, vectorLeft * maxRange, Color.red);
            Debug.DrawRay(Origin.position, transform.forward * maxRange, Color.red);
            Debug.Log("Did not Hit");
        }*/
    }

    public void VehiclesMovement()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}
