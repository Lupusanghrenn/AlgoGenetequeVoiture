using System.Collections.Generic;
using UnityEngine;

public class Neuron
{
    public float activation = 0f;

    public float bias;
    public List<float> weights;

    public Neuron() { }

    public Neuron(float act)
    {
        activation = act;
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

    public void InitWeightsAndBias()
    {
        for (int i = 0; i < weights.Count; i++)
            weights[i] = Random.Range(0, 1);
        bias
    }
}
