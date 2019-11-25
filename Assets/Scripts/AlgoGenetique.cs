using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgoGenetique : MonoBehaviour
{
    float GenerationTime;


    [Header("Genetique parameter")]
    public int nbGenerationsMax;
    public float MaxGenerationTime;
    public int nbIndividus = 100;
    public int nbBestIndividusToKeep = 10;
    public float timeSpeed;
    public int currentGeneration = 0;

    [Header("Init vehicule")]
    public Transform posGeneration;
    public GameObject vehicle;

    List<GameObject> allIndividus;

    private List<NeuralNetwork> stockBestGeneration;
    void Awake()
    {
        allIndividus = new List<GameObject>();
        stockBestGeneration = new List<NeuralNetwork>();
        Time.timeScale = timeSpeed;
        GenerationTime = MaxGenerationTime;
    }

    void Start()
    {
        GenerateGeneration();
    }

    void Update()
    {
        if (nbGenerationsMax > currentGeneration)
        {
            //if (GenerationTime <= 0.0f)
            //{
            //    EndGeneration();
            //    GenerationTime = MaxGenerationTime;
            //    GenerateGenerationFromBest();
            //    currentGeneration++;
            //}
            //else
            //{
               // GenerationTime -= Time.deltaTime;
                if (AllCollisionVehicle())
                {
                    EndGeneration();
                    GenerationTime = MaxGenerationTime;
                    GenerateGenerationFromBest();
                    currentGeneration++;
                }

            //}
        }
    }

    bool AllCollisionVehicle()
    {
        foreach(GameObject vehicle in allIndividus)
        {
            if (vehicle.GetComponent<VehicleManager>().collision == false)
            {
                return false;
            }
        }
        return true;
    }

    void GenerateGeneration()
    {
        for (int i = 0; i < nbIndividus; i++)
        {
            GameObject individu = Instantiate(vehicle, posGeneration.position, Quaternion.identity);
            allIndividus.Add(individu);
        }
    }

    void GenerateGenerationFromBest()
    {
        for (int i = 0; i < nbBestIndividusToKeep; i++)
        {
            GameObject fille1 = Instantiate(vehicle, posGeneration.position, Quaternion.identity);

            fille1.GetComponent<VehicleManager>().neuralNetwork = stockBestGeneration[i];

            allIndividus.Add(fille1);
        }
        for (int i = nbBestIndividusToKeep; i < nbIndividus; i+=2)
        {
            //croisement des parents pour la nouvele generations
            NeuralNetwork pere1 = allIndividus[Random.Range(0, nbBestIndividusToKeep)].GetComponent<VehicleManager>().neuralNetwork;
            NeuralNetwork pere2 = allIndividus[Random.Range(0, nbBestIndividusToKeep)].GetComponent<VehicleManager>().neuralNetwork;

            NeuralNetwork fille1NN = Croisement(pere1, pere2);//70% de pere1
            NeuralNetwork fille2NN = Croisement(pere2, pere1);//70% de pere2

            GameObject fille1 = Instantiate(vehicle, posGeneration.position, Quaternion.identity);
            GameObject fille2 = Instantiate(vehicle, posGeneration.position, Quaternion.identity);

            fille1.GetComponent<VehicleManager>().neuralNetwork = fille1NN;
            fille2.GetComponent<VehicleManager>().neuralNetwork = fille2NN;

            allIndividus.Add(fille1);
            allIndividus.Add(fille2);

        }
    }

    NeuralNetwork Croisement(NeuralNetwork pere1, NeuralNetwork pere2) 
    {
        NeuralNetwork nn = pere1;
        for(int i = 1; i <= nn.nHiddenLayer; i++)
        {
            for (int j = 0; j < nn.nNeuronPerLayer; j++)
            {
                for (int k = 0; k < nn.layers[i][j].weights.Count; k++)
                {
                    float rng = Random.Range(0.0f, 100.0f);
                    if (rng >= 99f)
                    {
                        nn.layers[i][j].weights[k] = Random.Range(-1.0f, 1.0f);
                    }
                    else if(rng <= 29.5f)
                    {
                        nn.layers[i][j].weights[k] = pere2.layers[i][j].weights[k];
                    }
                }            
            }
        }


        return nn;
    }

    void EndGeneration()
    {
        allIndividus.Sort(Compare);
        stockBestGeneration.Clear();
        for (int i = 0; i < nbBestIndividusToKeep; i++)
        {
            stockBestGeneration.Add(allIndividus[i].GetComponent<VehicleManager>().neuralNetwork);
        }

        foreach(GameObject g in allIndividus)
        {
            Destroy(g);
        }
        allIndividus.Clear();

        
    }

    static public int Compare(GameObject x, GameObject y)
    {
        VehicleManager vehicleManagerX = x.GetComponent<VehicleManager>();
        VehicleManager vehicleManagerY = y.GetComponent<VehicleManager>();

        if (vehicleManagerX.fitness > vehicleManagerY.fitness)
        {
            return 1;
        }
        else if (vehicleManagerX.fitness < vehicleManagerY.fitness)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}
