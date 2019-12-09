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
    public float maxTimeGeneration;

    [Header("Init vehicule")]
    public Transform posGeneration;
    public GameObject vehicle;

    [Header("Text UI parameter")]
    public Text pop;
    public Text vitesseJeu;
    public Text nbMeilleur;
    public Text nbGene;
    public Text currentGene;
    public Text distance;
    public Text avgDistance;

    [SerializeField]
    List<GameObject> allIndividus;
    float bestFitness = 0;
    double avgFitness = 0.0d;
    public Text displayBest;

    List<NeuralNetwork> stockBestGeneration;
    public float currentTime = 0.0f;
    void Awake()
    {
        allIndividus = new List<GameObject>();
        stockBestGeneration = new List<NeuralNetwork>();
    }

    void Start()
    {
        resetUI();
        GenerateGeneration();
        //GenerateGeneration("test");
    }

    void Update()
    {
        if (currentTime > maxTimeGeneration)
        {
            currentGeneration++;
            FindBest();
            stockBestGeneration.Clear();
            EndGeneration();
            GenerateGenerationFromBest();
            bestFitness = float.MinValue;
            currentTime = 0f;
        }
        Time.timeScale = timeSpeed;
        if (nbGenerationsMax > currentGeneration)
        {
            if (AllCollisionVehicle())
            {
                currentGeneration++;
                FindBest();
                stockBestGeneration.Clear();
                EndGeneration();
                GenerateGenerationFromBest();
                bestFitness = float.MinValue;
                currentTime = 0f;
            }
        }
        currentTime += Time.deltaTime;
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

    void GenerateGeneration(string pathToNet)
    {
        for (int i = 0; i < nbIndividus; i++)
        {
            GameObject individu = Instantiate(vehicle, posGeneration.position, Quaternion.identity);
            individu.GetComponent<VehicleManager>().neuralNetwork.importSettings(pathToNet);
            allIndividus.Add(individu);
        }
    }

    void GenerateGenerationFromBest()
    {
        stockBestGeneration[0].exportToFile("test");
        for (int i = 0; i < nbBestIndividusToKeep; i++)
        {
            GameObject fille1 = Instantiate(vehicle, posGeneration.position, Quaternion.identity);
            fille1.GetComponent<VehicleManager>().neuralNetwork = new NeuralNetwork(stockBestGeneration[i]);
            if (i == 0)
                fille1.GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;
            allIndividus.Add(fille1);
        }
        for (int i = nbBestIndividusToKeep; i < nbIndividus; i+=2)
        {
            //croisement des parents pour la nouvele generations
            int rngP1 = Random.Range(0, nbBestIndividusToKeep);
            int rngP2 = Random.Range(0, nbBestIndividusToKeep);
            while(rngP2 == rngP1)
                rngP2 = Random.Range(0, nbBestIndividusToKeep);

            NeuralNetwork pere1 = new NeuralNetwork(allIndividus[rngP1].GetComponent<VehicleManager>().neuralNetwork);
            NeuralNetwork pere2 = new NeuralNetwork(allIndividus[rngP2].GetComponent<VehicleManager>().neuralNetwork);

            //prendre des segments du neurones
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
        NeuralNetwork nn = new NeuralNetwork();
        int nbNeuronPere = Mathf.RoundToInt(nn.nNeuronPerLayer * 0.7f);
        for(int i = 1; i <= nn.nHiddenLayer; i++)
        {
            List<int> indexUse = new List<int>(nn.nNeuronPerLayer);
            for (int j = 0; j < nbNeuronPere; j++)
            {
                int rngN;
                do
                    rngN = Random.Range(0, nn.nNeuronPerLayer);
                while (indexUse.Contains(rngN));
                indexUse.Add(rngN);

                nn.layers[i][rngN] = pere1.layers[i][rngN];        
            }

            for(int j = 0; j < nn.nNeuronPerLayer; j++)
            {
                if(!indexUse.Contains(j))
                    nn.layers[i][j] = pere2.layers[i][j];
            }
        }
        return nn;
    }

    void EndGeneration()
    {
        GetBest();
        double summ = 0.0d;
        foreach (GameObject g in allIndividus)
        {
            summ += g.GetComponent<VehicleManager>().fitness;
            Destroy(g);
        }
        avgFitness = summ / (double)allIndividus.Count;
        resetUI();
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

    void GetBest()
    {
        for(int i = 0; i < nbBestIndividusToKeep; i++)
        {
            VehicleManager best = allIndividus[0].GetComponent<VehicleManager>();
            for(int j = 1; j < allIndividus.Count; j++)
            {
                if(allIndividus[j].GetComponent<VehicleManager>().fitness > best.fitness)
                {
                    best = allIndividus[j].GetComponent<VehicleManager>();
                }
            }
         
            if (best.fitness > bestFitness)
                bestFitness = best.fitness;
            stockBestGeneration.Add(new NeuralNetwork(best.neuralNetwork));
            allIndividus.Remove(best.gameObject);
            Destroy(best.gameObject);
        }
    }

    void resetUI()
    {
        pop.text = nbIndividus.ToString();
        vitesseJeu.text = timeSpeed.ToString();
        nbMeilleur.text = nbBestIndividusToKeep.ToString();
        nbGene.text = nbGenerationsMax.ToString();
        currentGene.text = currentGeneration.ToString();
        distance.text = bestFitness.ToString(".000");
        avgDistance.text = avgFitness.ToString(".000");
    }
}
