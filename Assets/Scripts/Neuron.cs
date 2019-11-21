using System.Collections.Generic;
using UnityEngine;

public class Neuron
{
    public float activation = 0f;

    public float bias;
    public List<float> weights;

    public Neuron(int nWeights)
    {
        InitWeightsAndBias(nWeights);
    }

    public Neuron(float act) // used for input layer
    {
        activation = act;
        // no weights
        // no bias
    }

    public void ComputeActivation(Neuron[] inputs)
    {
        float sum = 0;
        for(int i = 0; i < inputs.Length; i++)
        {
            sum += inputs[i].activation * weights[i];
        }
        sum += bias;

        activation = sum;
    }

    public void InitWeightsAndBias(int nWeights)
    {
        weights = new List<float>();
        for (int i = 0; i < nWeights; i++)
            weights.Add(Random.Range(-1f, 1f));
        bias = Random.Range(-1f, 1f);
    }
}
