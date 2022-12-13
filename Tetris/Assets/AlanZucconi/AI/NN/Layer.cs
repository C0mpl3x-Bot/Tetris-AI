using System;

namespace AlanZucconi.AI.NN
{
    [Serializable]
    public class Layer
    {
        public int Size;

        public Layer Previous = null;
        public Layer Next = null;

        public float[] Values; //

        public float[,] Weights; // size: [parent.size, this.size]
        public float[] Biases;   // size: [this.size]

        public Activation Activation;

        // For backpropagation
        public float[] Errors; // size: [this.size] (deltas in backpropagation)

        public Layer(Layer parent, int size, Activation activation)
        {
            Previous = parent;
            Activation = activation;
            Size = size;

            Build();
        }

        private void Build()
        {
            // Build
            Values = new float[Size];
            Errors = new float[Size];

            // No parent: this is an input layer
            if (Previous == null)
                return;

            // Weights
            Weights = new float[Previous.Size, Size];
            Biases = new float[Size];

            // Random initialisation
            for (int i = 0; i < Size; i++)
            {
                // Biases
                Biases[i] = UnityEngine.Random.Range(-1f, +1f);

                // Weights
                for (int pi = 0; pi < Previous.Size; pi++)
                    Weights[pi, i] = UnityEngine.Random.Range(-1f, +1f);
            }
        }

        // Calculates the output values
        public void Update()
        {
            // Input layers are updated externally
            if (Previous == null)
                return;

            for (int i = 0; i < Size; i++)
            {
                // Reset
                Values[i] = 0 + Biases[i];
                // y = w * x + b
                for (int pi = 0; pi < Previous.Size; pi++)
                    Values[i] += Previous[pi] * Weights[pi, i];

                // Activation
                // y = f(w * x + b)
                Values[i] = Activation.F(Values[i]);
            }
        }


        public void Backprop(float learningRate)
        {
            // Loops through the neurons
            for (int i = 0; i < Size; i++)
            {
                // Loops through all of the outgoing connections
                // from the next layer
                float backpropError = 0;
                if (Next != null)
                    // Error for hidden layers
                    for (int ni = 0; ni < Next.Size; ni++)
                        backpropError = Next.Weights[i, ni] * Next.Errors[ni];
                else
                    // Errors for output layer
                    backpropError = Errors[i];
                // for this to work, Errors[i] of the last layer must be set to
                // Value[i] - Target[i]

                Errors[i] = Activation.D(Values[i]) * backpropError;
            }

            // Backprop stops at the input layer
            if (Previous == null)
                return;


            // Backprop to previous layers
            Previous.Backprop(learningRate);

            // Now that all Errors[] have been updated,
            // we can finally change the Weights[,] and Biases[]
            // 
            // We could have not not done that earlier,
            // because Previous needs the original Weights[,]
            // => they need to be updated at the every end!
            for (int i = 0; i < Size; i++)
            {
                // Loops through all of the incoming connections
                // from the previous layer
                for (int pi = 0; pi < Previous.Size; pi++)
                    Weights[pi, i] -= learningRate * Previous.Values[pi] * Errors[i];

                // Backprop for biases
                Biases[i] -= learningRate * 1f * Errors[i];
            }
        }



        // Gets neuron value
        public float this[int i]
        {
            get { return Values[i]; }
            set { Values[i] = value; }
        }


        
    }
}