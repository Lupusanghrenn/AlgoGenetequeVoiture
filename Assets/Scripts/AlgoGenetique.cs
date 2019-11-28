using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlgoGenetique : MonoBehaviour
{
    [Header("Genetique parameter")]
    public int nbGenerationsMax;
    public int nbIndividus = 100;
    public int nbBestIndividusToKeep = 10;
    [Range(0, 70)] public float timeSpeed;
    public int currentGeneration = 0;

    [Header("Init vehicule")]
    public Transform posGeneration;
    public GameObject vehicle;

    [SerializeField]
    List<GameObject> allIndividus;
    float bestFitness = float.MinValue;
    public Text displayBest;

    List<NeuralNetwork> stockBestGeneration;
    void Awake()
    {
        allIndividus = new List<GameObject>();
        stockBestGeneration = new List<NeuralNetwork>();
    }

    void Start()
    {
        GenerateGeneration();
    }

    void Update()
    {
        Time.timeScale = timeSpeed;
        if (nbGenerationsMax > currentGeneration)
        {
            if (AllCollisionVehicle())
            {
                currentGeneration++;
                Debug.Log("Generation " + currentGeneration);
                EndGeneration();
                GenerateGenerationFromBest();
            }
        }
        FindBest();
    }

    void FindBest()
    {
        foreach(GameObject nn in allIndividus)
        {
            bestFitness = Mathf.Max(nn.GetComponent<VehicleManager>().fitness, bestFitness);
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
        Debug.Log("Generate");
        for (int i = 0; i < nbBestIndividusToKeep; i++)
        {
            GameObject fille1 = Instantiate(vehicle, posGeneration.position, Quaternion.identity);

            fille1.GetComponent<VehicleManager>().neuralNetwork = new NeuralNetwork(stockBestGeneration[i]);

            allIndividus.Add(fille1);
        }
        for (int i = 0; i < nbBestIndividusToKeep; i++)
        {
            allIndividus[i].GetComponent<VehicleManager>().neuralNetwork.Print();
        }
        Debug.Log("Croisement ");
        for (int i = nbBestIndividusToKeep; i < nbIndividus; i+=2)
        {
            //croisement des parents pour la nouvele generations
            int rngP1 = Random.Range(0, nbBestIndividusToKeep);
            int rngP2 = Random.Range(0, nbBestIndividusToKeep);
            while(rngP2 == rngP1)
                rngP2 = Random.Range(0, nbBestIndividusToKeep);

            NeuralNetwork pere1 = new NeuralNetwork(allIndividus[rngP1].GetComponent<VehicleManager>().neuralNetwork);
            NeuralNetwork pere2 = new NeuralNetwork(allIndividus[rngP2].GetComponent<VehicleManager>().neuralNetwork);

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
        NeuralNetwork nn = new NeuralNetwork(pere1);
        for(int i = 1; i <= nn.nHiddenLayer; i++)
        {
            for (int j = 0; j < nn.nNeuronPerLayer; j++)
            {
                float rngN = Random.Range(0.0f, 100.0f);
                nn.layers[i][j].activation = 0;
                if (rngN >= 99f)
                {
                    nn.layers[i][j].bias = Random.Range(-1.0f, 1.0f);
                }
                else if (rngN <= 29.5f)
                {
                    nn.layers[i][j].bias = pere2.layers[i][j].bias;
                }

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
            stockBestGeneration.Add(new NeuralNetwork(allIndividus[i].GetComponent<VehicleManager>().neuralNetwork));
        }
        foreach (GameObject g in allIndividus)
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
            return -1;
        }
        else if (vehicleManagerX.fitness < vehicleManagerY.fitness)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
