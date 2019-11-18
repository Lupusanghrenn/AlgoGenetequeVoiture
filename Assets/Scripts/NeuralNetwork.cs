using UnityEngine;

public class NeuralNetwork : MonoBehaviour
{
    public int nInputs = 3;
    public int nOutputs = 2;

    public int nHiddenLayer = 1;
    public int nNeuronPerLayer = 4;

    Neuron[][] layers;
    float[][] links;

    private void Awake()
    {
        layers = new Neuron[nHiddenLayer + 2][];

        layers[0] = new Neuron[nInputs]; // input layer
        for (int i = 1; i < nHiddenLayer + 1; i++)
        {
            layers[i] = new Neuron[nNeuronPerLayer];
        }
        layers[nHiddenLayer + 1] = new Neuron[nOutputs]; // output layer

        links = new float[nHiddenLayer + 1][];
    }

    void ComputeOutputs()
    {
        for (int i = 0; i < nHiddenLayer + 1; i++) // go through links
        {
            links[i] = new float[layers[i].Length * layers[i + 1].Length];
            
            for (int l = 0; l < layers[i + 1].Length; l++) // receiver layer
            {
                links[i][l] = 0;
                for(int n = 0; n < layers[i].Length; n++) // donor layer
                {
                    links[i][l] += layers[i + 1][l].activation * layers[i][n].activation;
                }
            }
        }
    }
}
