using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        VehicleManager vehicle = other.GetComponent<VehicleManager>();
        if(vehicle != null)
        {
            vehicle.fitness += (vehicle.indexCheckPoint + 1) * 100;
            vehicle.indexCheckPoint++;
        }
    }
}
