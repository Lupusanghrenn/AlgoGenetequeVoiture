using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork
{
    public int nInputs = 5;
    public int nOutputs = 1;

    public int nHiddenLayer = 1;
    public int nNeuronPerLayer = 4;

    public Neuron[][] layers;

    public Neuron[] outputs;

    public NeuralNetwork()
    {
        InitArrays();
        InitWeightsAndBias();
    }

    public NeuralNetwork(NeuralNetwork nn)
    {
        InitArrays();
        for(int i = 0; i < 1 + nHiddenLayer + 1; i++)
        {
            nn.layers[i].CopyTo(layers[i], 0);
        }
    }

    void InitArrays()
    {
        layers = new Neuron[1 + nHiddenLayer + 1][];

        layers[0] = new Neuron[nInputs];
        for(int i = 1; i <= nHiddenLayer; i++)
        {
            layers[i] = new Neuron[nNeuronPerLayer];
        }
        layers[layers.Length - 1] = new Neuron[nOutputs]; // propagation to ouput layer
    }

    void InitWeightsAndBias()
    {
        for(int l = 1; l < layers.Length; l++)
        {
            for(int n = 0; n < layers[l].Length; n++)
            {
                layers[l][n] = new Neuron(layers[l - 1].Length);
            }
        }
    }

    public void FeedInputs(float[] inp)
    {
        for(int i = 0; i < inp.Length; i++)
        {
            layers[0][i] = new Neuron(inp[i]);
        }
        Propagation();
    }

    public void Propagation()
    {
        for(int l = 1; l < layers.Length - 1; l++)
        {
            Neuron[] previousLayer = layers[l - 1];

            for(int n = 0; n < layers[l].Length; n++)
            {
                layers[l][n].ComputeActivation(previousLayer);
            }
        }

        ComputeOutputs();
    }

    void ComputeOutputs()
    {
        for(int n = 0; n < layers[layers.Length - 1].Length; n++)
        {
            layers[layers.Length - 1][n].ComputeActivation(layers[layers.Length - 2]);
        }
        outputs = layers[layers.Length - 1];
    }

    public void Print()
    {
        string print = "";
        for(int i = 1; i <= nHiddenLayer; i++)
        {
            print += "Couche " + i + " : \n";
            for(int j = 0; j < nNeuronPerLayer; j++)
            {
                print += "Neuron " + j + " = \n" + layers[i][j].Print();
            }
        }
        for(int i = 0; i < nOutputs; i++)
        {
            print += "Output " + i + " = \n" + layers[1 + nHiddenLayer][i].Print();
        }
        //Debug.Log(print);
    }
}
