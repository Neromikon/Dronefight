using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NeuralNetwork
{
	private class Link
	{
		//disabled link should not be in the inputs and outputs of any neuron
		//but should be stored in general list

		public bool enabled;
		public double value;
		public double weight;
		public double squaredError;
	}

	private class Neuron
	{
		public double bias = 0.0;
		public List<Link> inputs = null;
		public List<Link> outputs = null;

		public void Activate()
		{
			double sum = bias;

			for (int i = 0; i < inputs.Count; i++)
			{
				sum += inputs[i].value * inputs[i].weight;
			}

			double outputValue = ActivationFunction(sum);

			for (int i = 0; i < outputs.Count; i++)
			{
				outputs[i].value = outputValue;
			}
		}

		public static double ActivationFunction(double x)
		{
			return Math.Tanh(x);
		}

		public static double ActivationFunctionDerivative(double x)
		{
			double q = Math.Tanh(x);
			return 1.0 - q * q;
		}
	}

	private struct Savedata
	{
		public string namingCode; //a string containing names of all inputs and all outputs
								  //must exactly mutch for loading a network
		public int[] topology;
		public bool[] linkEnability;
		public double[] linkWeights;
		public double[] neuronBiases;
	}

	private int[] topology;
	private int[] layerStarts;
	private List<Link> links;
	private List<Neuron> neurons;
	private List<Link> inputs;
	private List<Link> outputs;
	private double learningRate = 0.0;

	public NeuralNetwork(int[] topology)
	{
		this.topology = topology;
		layerStarts = new int[topology.Length];

		int neuronsCount = 0;
		int linksCount = 0;

		//defining number of neurons and links
		for (int i = 0; i < topology.Length - 1; i++)
		{
			layerStarts[i] = neuronsCount;
			neuronsCount += topology[i];
			linksCount += topology[i] * topology[i + 1];
		}

		//reserving lists space and creating neurons
		neurons = new List<Neuron>(neuronsCount);
		links = new List<Link>(linksCount + topology.First() + topology.Last());

		for (int i = 0; i < neurons.Count; i++)
		{
			neurons.Add(new Neuron());
		}

		//creating input links
		for (int i = 0; i < topology.First(); i++)
		{
			Link newLink = new Link();
			inputs.Add(newLink);
			links.Add(newLink);

			Neuron inputNeuron = neurons[layerStarts.First() + i];
			inputNeuron.inputs.Add(newLink);
		}

		//creating output links
		for (int i = 0; i < topology.Last(); i++)
		{
			Link newLink = new Link();
			outputs.Add(newLink);
			links.Add(newLink);

			Neuron outputNeuron = neurons[layerStarts.Last() + i];
			outputNeuron.outputs.Add(newLink);
		}

		//creating hidden links
		for (int i = 0; i < topology.Length - 1; i++)
		{
			for (int j = 0; j < topology[i + 1]; j++)
			{
				Neuron neuron = neurons[layerStarts[i + 1] + j];
				neuron.inputs = new List<Link>(topology[i]);
			}

			for (int j = 0; j < topology[i]; j++)
			{
				Neuron neuron1 = neurons[layerStarts[i] + j];
				neuron1.outputs = new List<Link>(topology[i + 1]);

				for (int k = 0; k < topology[i + 1]; k++)
				{
					Neuron neuron2 = neurons[layerStarts[i + 1] + j];

					Link newLink = new Link();

					neuron1.outputs.Add(newLink);
					neuron2.inputs.Add(newLink);
					links.Add(newLink);
				}
			}
		}
	}

	void Input(int index, double value)
	{
		System.Diagnostics.Debug.Assert(index < inputs.Count);

		inputs[index].value = value;
	}

	double[] Run(double[] inputValues)
	{
		System.Diagnostics.Debug.Assert(inputValues.Length == inputs.Count);

		double[] result = new double[outputs.Count];

		for (int i = 0; i < inputs.Count; i++) { inputs[i].value = inputValues[i]; }

		foreach (Neuron neuron in neurons) { neuron.Activate(); }

		for (int i = 0; i < outputs.Count; i++) { result[i] = outputs[i].value; }

		return result;
	}

	void Learn(double[] desiredOutputs) //back propagation
	{
		if (learningRate <= 0) { return; }

		System.Diagnostics.Debug.Assert(desiredOutputs.Length == outputs.Count);

		for (int i = 0; i < outputs.Count; i++)
		{
			double absoluteError = desiredOutputs[i] - outputs[i].value;
			outputs[i].squaredError = absoluteError * absoluteError;
		}


	}

	public static double Sigmoid(double x)
	{
		return Math.Tanh(x);
	}
}