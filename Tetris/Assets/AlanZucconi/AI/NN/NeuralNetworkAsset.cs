using System.Collections.Generic;
using UnityEngine;

using AlanZucconi.AI.Evo;

namespace AlanZucconi.AI.NN
{

    [CreateAssetMenu
    (
        fileName = "NeuralNewtorkData",
        menuName = "Neural Newtork"
    )]
    public class NeuralNetworkAsset: ScriptableObject
    {
        public NeuralNetworkData Data;
    }

    [System.Serializable]
    public class NeuralNetworkData : IGenome
    {
        // Layers
        //[HideInInspector]
        public List<LayerData> Layers;

        #region Evolution
        // Copies this genome
        public IGenome Copy()
        {
            NeuralNetworkData data = new NeuralNetworkData();
            data.Layers = new List<LayerData>();

            foreach (LayerData layer in Layers)
                data.Layers.Add(layer.Clone());

            return data;
        }

        // Picks a random element and mutates it
        public void Mutate()
        {
            float MAX_MUTATION = 0.1f;

            LayerData layer = Layers[Random.Range(0, Layers.Count)];
            if (Random.Range(0f, 1f) < 0.5f)
                layer.FlattenWeights[Random.Range(0, layer.FlattenWeights.Length)] += Random.Range(0f, MAX_MUTATION);
            else
                layer.Biases[Random.Range(0, layer.Biases.Length)] += Random.Range(0f, MAX_MUTATION);
        }

        public void InitialiseRandom ()
        {
            foreach (LayerData layer in Layers)
                layer.InitialiseRandom();
        }
        #endregion
    }

    [System.Serializable]
    public struct LayerData
    {
        // [,] not serializable, so we flat the weights to []
        // The lenghts of Weights before the flattening is
        // Weigts[FlattenWeightsLength/Biases.Length, Biases.Length]
        public float[] FlattenWeights;
        public float[] Biases;

        #region Evolution
        // Clones: used for Evolution
        public LayerData Clone ()
        {
            LayerData data = new LayerData();
            data.FlattenWeights = (float[]) FlattenWeights.Clone();
            data.Biases = (float[]) Biases.Clone();
            return data;
        }

        // Randomises all of the parameters
        public void InitialiseRandom ()
        {
            for (int i = 0; i < FlattenWeights.Length; i++)
                FlattenWeights[i] = Random.Range(-1f, +1f);
            for (int i = 0; i < Biases.Length; i++)
                Biases[i] = Random.Range(-1f, +1f);
        }
        #endregion
    }

    // Used to export and import LayerData
    public static class NeuralNetworkDataExtension //LayerExtensions
    {

        // Exports the Weights and Biases to a LayerData
        public static LayerData Export (this Layer layer)
        {
            LayerData data = new LayerData();
            //data.Weights = (float[,])layer.Weights.Clone();
            //data.Weights = layer.Weights.ToJaggedArray();
            data.FlattenWeights = layer.Weights.Flatten();
            data.Biases = (float[])layer.Biases.Clone();
            return data;
        }

        // Imports the Weights and Biases from a LayerData
        public static void Import (this Layer layer, LayerData data)
        {
            //layer.Weights = (float[,])data.Weights.Clone();
            //layer.Weights = data.Weights.ToMultiArray();
            layer.Weights = data.FlattenWeights.Unflatten
                (
                    data.FlattenWeights.Length / data.Biases.Length,
                    data.Biases.Length
                );
            layer.Biases = (float[])data.Biases.Clone();
        }




        // Export&Import from to Data
        //public static void Export(this NeuralNetwork network, NeuralNetworkData data)
        public static NeuralNetworkData Export (this NeuralNetwork network)
        {
            NeuralNetworkData data = new NeuralNetworkData();
            data.Layers = new List<LayerData>();

            // Skips input layer
            for (int i = 0 + 1; i < network.Layers.Count; i++)
                data.Layers.Add(network.Layers[i].Export());

            return data;
        }
        public static void Import (this NeuralNetwork network, NeuralNetworkData data)
        {
            // Skips input layer
            for (int i = 0 + 1; i < network.Layers.Count; i++)
                network.Layers[i].Import(data.Layers[i - 1]);
        }

        // Export&Import from Asset
        public static void Export(this NeuralNetwork network, NeuralNetworkAsset asset)
        {
            asset.Data = network.Export();

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(asset);
#endif
        }

        public static void Import (this NeuralNetwork network, NeuralNetworkAsset asset)
        {
            network.Import(asset.Data);
        }
    }
}