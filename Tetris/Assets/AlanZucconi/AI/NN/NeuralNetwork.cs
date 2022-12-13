using UnityEngine;

using System;
using System.Collections.Generic;

namespace AlanZucconi.AI.NN
{
    // A simple neural networks made out of fully connected layers
    [Serializable]
    public class NeuralNetwork
    {
        [HideInInspector]
        public List<Layer> Layers = new List<Layer>();

        

        public NeuralNetwork ()
        {

        }

        // Adds a new Layer
        public void AddLayer (int size, Activation activation)
        {
            // Gets the last layer
            Layer previous =
                Layers.Count == 0
                ? null
                : Layers[Layers.Count - 1];

            // Creates and adds the new layer
            // (it is built automatically)
            Layer layer = new Layer(previous, size, activation);
            Layers.Add(layer);

            if (previous != null)
                previous.Next = layer;
        }

        public void AddLayer (int size)
        {
            AddLayer(size, Activation.LeakyReLU);
        }

        public void Update ()
        {
            for (int i = 0; i < Layers.Count; i++)
                Layers[i].Update();
        }

        #region Inputs
        // First layer: input
        public Layer InputLayer
        {
            get
            {
                return Layers[0];
            }
        }

        // Sets an input value
        public void SetInput (int i, float input)
        {
            InputLayer[i] = input;
        }
        public void SetInputs (params float [] inputs)
        {
            for (int i = 0; i < inputs.Length; i++)
                InputLayer[i] = inputs[i];
        }
        #endregion

        #region Outputs

        // Last layer: output
        public Layer OutputLayer
        {
            get
            {
                return Layers[Layers.Count - 1];
            }
        }
        public float GetOutput (int i)
        {
            return OutputLayer[i];
        }
        public float[] GetOutputs ()
        {
            return OutputLayer.Values;
        }
        #endregion

        #region Backprop
        // Performs backprop in the couple (inputs, outputs)
        // input size: size of first layer
        // targets size: size of last layer
        public void Backprop (float [] inputs, float [] targets, float learningRate)
        {
            // Forward propagation
            // (updates Values[])
            SetInputs(inputs);
            Update();

            // Initialise errors for the output layer
            for (int i = 0; i < OutputLayer.Size; i++)
                OutputLayer.Errors[i] = OutputLayer.Values[i] - targets[i];

            // Starts from the last year and backprop
            OutputLayer.Backprop(learningRate);

            // Loops layers in reverse
            //for (int i = Layers.Count -1; i >= 1; i--)
            //    Layers[i].Backprop(learningRate);

        }
        #endregion
    }
}