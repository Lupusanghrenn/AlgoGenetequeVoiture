using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiclesParam : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 2.0f;
    public float maxRange = 10.0f;
    public Transform Origin;
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
        //direction du vecteur max range sur la droite
        Vector3 vectorRight = transform.forward;
        vectorRight = Quaternion.AngleAxis(45, Vector3.up) * vectorRight;

        //direction du vecteur max range sur la gauche
        Vector3 vectorLeft = transform.forward;
        vectorLeft = Quaternion.AngleAxis(-45, Vector3.up) * vectorLeft;

        RaycastHit hit;
        if (Physics.Raycast(Origin.position, transform.forward, out hit, maxRange))
        {
            Debug.DrawRay(Origin.position, transform.forward * hit.distance, Color.green);
            Debug.Log("Did Hit");
        }
        else
        {

            Debug.DrawRay(Origin.position, vectorRight * maxRange, Color.red);
            Debug.DrawRay(Origin.position, vectorLeft * maxRange, Color.red);
            Debug.DrawRay(Origin.position, transform.forward * maxRange, Color.red);
            Debug.Log("Did not Hit");
        }
    }

    public void VehiclesMovement()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}
