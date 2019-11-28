using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
        string print = this.nInputs + " " + this.nHiddenLayer + " " + this.nNeuronPerLayer + " " + this.nOutputs+"\n";
        for(int i = 1; i <= nHiddenLayer; i++)
        {
            print += "Couche " + i + " : \n";
            for(int j = 0; j < nNeuronPerLayer; j++)
            {
                print += "Neuron " + j + " {" + layers[i][j].weights.Count + "} = \n" + layers[i][j].Print();
            }
        }
        for(int i = 0; i < nOutputs; i++)
        {
            print += "Output " + i + " {" + layers[1+nHiddenLayer][i].weights.Count + "} = \n" + layers[1 + nHiddenLayer][i].Print();
        }
        Debug.Log(print);
    }

    public void exportToFile(string name)
    {
        StreamWriter streamWriter = new StreamWriter(name + ".net");
        //ecriture
        //infos : nb input/ nombre de couche chache * nbneurone par layer/nb output 
        streamWriter.WriteLine(this.nInputs + " " + this.nHiddenLayer + " " + this.nNeuronPerLayer + " " + this.nOutputs);

        for (int i = 1; i <= nHiddenLayer; i++)
        {
            streamWriter.WriteLine("Couche " + i + " : ");
            for (int j = 0; j < nNeuronPerLayer; j++)
            {
                streamWriter.Write("Neuron " + j +" {"+layers[i][j].weights.Count+"} = \n" + layers[i][j].Print());
            }
        }
        for (int i = 0; i < nOutputs; i++)
        {
             streamWriter.Write("Output " + i + " {" + layers[1+nHiddenLayer][i].weights.Count + "} = \n" + layers[1 + nHiddenLayer][i].Print());
        }

        streamWriter.Close();
    }

    public NeuralNetwork (string path)
    {
        StreamReader reader = new StreamReader(path+".net");

        string[] infos = reader.ReadLine().Split(' ');
        this.nInputs = int.Parse(infos[0]);
        this.nHiddenLayer = int.Parse(infos[1]);
        this.nNeuronPerLayer = int.Parse(infos[2]);
        this.nOutputs = int.Parse(infos[3]);

        InitArrays();

        //remplissage des couches cachés
        for(int nbCouche = 0; nbCouche < this.nHiddenLayer; nbCouche++)
        {
            string line = reader.ReadLine();
            int indexCouche = int.Parse(line.Split(' ')[1]);
            char[] sep = { '{', '}' };
            int nbWeight = int.Parse(line.Split(sep)[1]);
            for(int i = 0; i < nNeuronPerLayer; i++)
            {
                int indexNeuron = int.Parse(reader.ReadLine().Split(' ')[1]);
                Neuron n = new Neuron(nbWeight);
                //activation
                reader.ReadLine();
                n.activation = 0f;
                //biai
                n.bias = float.Parse(reader.ReadLine().Split(' ')[2]);
                for (int j = 0; j < nbWeight; j++)
                {
                    //remplissage des poids
                    string lineNeuron = reader.ReadLine();
                    float valueWeight = float.Parse(lineNeuron.Split(' ')[2]);
                    char[] separators = { '[', ']' };
                    int index = int.Parse(lineNeuron.Split(separators)[1]);
                    n.weights[index] = valueWeight;
                }
                layers[nbCouche][i] = n;
            }
        }

        //output
        for(int i = 0; i < nOutputs; i++)
        {
            Neuron output = new Neuron(nNeuronPerLayer);
            //activation
            reader.ReadLine();
            output.activation = 0f;
            //biai
            output.bias = float.Parse(reader.ReadLine().Split(' ')[2]);
            for (int j = 0; j < nNeuronPerLayer; j++)
            {
                //remplissage des poids
                string lineNeuron = reader.ReadLine();
                float valueWeight = float.Parse(lineNeuron.Split(' ')[2]);
                char[] separators = { '[', ']' };
                int index = int.Parse(lineNeuron.Split(separators)[1]);
                output.weights[index] = valueWeight;
            }
            layers[1+nHiddenLayer][i] = output;
        }
    }
}
