using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AlanZucconi.AI.Evo;

namespace AlanZucconi.AI.NN.Test
{
    public class Function : MonoBehaviour,
        IWorld<NeuralNetworkData>,
        IGenomeFactory<NeuralNetworkData>
    {

        [Header("Function")]
        public AnimationCurve Curve;
        public float StepX = 0.01f;


        [Header("Neural Network")]
        public NeuralNetwork Network;
        private NeuralNetworkData NetworkData;

        [Button(Editor=true)]
        public void Start()
        {
            Network = new NeuralNetwork();
            Network.AddLayer(1);
            Network.AddLayer(10, Activation.Tanh);
            Network.AddLayer(10, Activation.Tanh);
            //Network.AddLayer(10, Activation.Tanh);
            //Network.AddLayer(20, Activation.Tanh);
            //Network.AddLayer(30, Activation.Tanh);
            Network.AddLayer(1, Activation.Tanh);
        }


        #region Evolution
        public NeuralNetworkData Instantiate()
        {
            NeuralNetworkData data = Network.Export();
            data.InitialiseRandom();

            return data;
        }

        public void SetGenome(NeuralNetworkData data)
        {
            NetworkData = data;
            Network.Import(data);
        }
        public NeuralNetworkData GetGenome()
        {
            return NetworkData;
        }

        public void ResetSimulation()
        {
            
        }
        public void StartSimulation()
        {
            
        }

        public bool IsDone()
        {
            // Done in one frame!
            return true;
        }

        public float GetScore()
        {
            // score is rsme
            float score = 0;

            for (float x = 0; x <= 1f; x += StepX)
            {
                float y = Curve.Evaluate(x);

                Network.SetInput(0, x);
                Network.Update();
                float y_nn = Network.GetOutput(0);

                score += Mathf.Pow(y - y_nn, 2f);
            }

            return 1f/Mathf.Sqrt(score / (1f / StepX));
        }
        #endregion

        private void OnDrawGizmos()
        {
            // Draw curve
            Vector3 prev = new Vector3(0f, Curve.Evaluate(0f));
            for (float x = 0; x <= 1f; x += StepX)
            {
                Vector3 next = new Vector3(x, Curve.Evaluate(x));
                Debug.DrawLine(transform.position+prev, transform.position+next, Color.green);
                prev = next;
            }

            if (Network == null)
                return;

            // Neural network
            prev = new Vector3(0f, 0f);
            for (float x = 0; x <= 1f; x += StepX)
            {
                Network.SetInput(0, x);
                Network.Update();
                float y = Network.GetOutput(0);

                
                Vector3 next = new Vector3(x, y);
                if (x != 0)
                    Debug.DrawLine(transform.position+prev, transform.position + next, Color.red);
                prev = next;
            }
        }
    }
}