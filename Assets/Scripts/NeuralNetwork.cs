using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork
{
    public int nInputs = 3;
    public int nOutputs = 2;

    public int nHiddenLayer = 1;
    public int nNeuronPerLayer = 4;

    Neuron[][] layers;

    public NeuralNetwork()
    {
        InitArrays();
        InitWeightsAndBias();
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
                layers[l][n].InitWeightsAndBias();
            }
        }
    }

    void InitInputs(float[] inp)
    {
        for(int i = 0; i < inp.Length; i++)
        {
            layers[0][i] = new Neuron(inp[i]);
        }
    }

    void Propagation() // propagation to hidden layers
    {
        for(int l = 1; l < layers.Length - 1; l++)
        {
            Neuron[] previousLayer = layers[l - 1];

            for(int n = 0; n < layers[l].Length; n++)
            {
                layers[l][n].ComputeActivation(previousLayer);
            }
        }
    }

    void ComputeOutputs()
    {
        for(int n = 0; n < layers[layers.Length - 1].Length; n++)
        {
            layers[layers.Length - 1][n].ComputeActivation(layers[layers.Length - 2]);
        }
    }
}
