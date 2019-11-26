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

    //float bias / list<float> weights
    public Neuron(float b, List<float> w)
    {
        bias = b;
        weights = w;
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

    public string Print()
    {
        string print = "";
        print += "activation = " + activation + "\nbiai = " + bias + "\n";
        for(int i = 0; i < weights.Count; i++)
        {
            print += "weight[" + i + "] = " + weights[i] + "\n";
        }
        return print;
    }
}
