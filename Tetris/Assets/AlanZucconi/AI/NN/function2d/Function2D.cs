using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AlanZucconi.AI.Evo;

namespace AlanZucconi.AI.NN.Test
{
    [ExecuteInEditMode]
    public class Function2D : MonoBehaviour,
        IWorld<NeuralNetworkData>,
        IGenomeFactory<NeuralNetworkData>
    {

        [Header("Function")]
        public float StepX = 0.1f;
        public float StepY = 0.1f;

        [Header("Debug")]
        public int xSize = 10;
        public int ySize = 10;


        [Header("Neural Network")]
        public NeuralNetwork Network;
        private NeuralNetworkData NetworkData;

        //[Button(Editor = true)]
        public void Start()
        {
            Network = new NeuralNetwork();
            Network.AddLayer(2);
            Network.AddLayer(10, Activation.Tanh);
            Network.AddLayer(10, Activation.Tanh);
            Network.AddLayer(10, Activation.Tanh);
            //Network.AddLayer(100);
            //Network.AddLayer(100);

            //Network.AddLayer(10, Activation.Tanh);
            //Network.AddLayer(10, Activation.Tanh);
            //Network.AddLayer(10, Activation.Tanh);
            //Network.AddLayer(10, Activation.Tanh);
            //Network.AddLayer(10, Activation.Tanh);

            //Network.AddLayer(10, Activation.Tanh);
            //Network.AddLayer(10, Activation.Tanh);
            //Network.AddLayer(100, Activation.Tanh);
            //Network.AddLayer(50, Activation.Tanh);
            //Network.AddLayer(50, Activation.Tanh);
            //Network.AddLayer(50, Activation.Tanh);
            //Network.AddLayer(10, Activation.Tanh);
            //Network.AddLayer(20, Activation.Tanh);
            //Network.AddLayer(30, Activation.Tanh);
            Network.AddLayer(1, Activation.Tanh);


            GenerateMesh();
            GenerateMeshNetwork();
        }


        private float Map(float x, float y)
        {
            //sin(x*3)+cos(8*x*y)
            return Mathf.Sin(x * 3f) / 3f + Mathf.Cos(x * y * 8f) / 3f
                //- Mathf.Sqrt(x*2f)/10f
                +
                Mathf.Sin(x / (y + 1))/4f;

                ;
            //return Gaussian2D(x, y, 0.5f, 0.5f, 0.1f, 0.1f) / 20f;
        }

        // https://stackoverflow.com/questions/7687679/how-to-generate-2d-gaussian-with-python
        private float Gaussian2D(float x, float y, float mx, float my, float sx, float sy)
        {
            return (1f / (2f * Mathf.PI * sx * sy)) * Mathf.Exp(-(Mathf.Pow(x - mx, 2f) / (2f * sx * sx) + Mathf.Pow(y - my, 2f) / (2f * sy * sy)));
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
            // score is 1/rsme
            float score = 0;

            for (float x = 0; x <= 1f; x += StepX)
            {
                for (float y = 0; y <= 1f; y += StepY)
                {
                    //float x = Curve.Evaluate(x);
                    float z = Map(x, y);

                    Network.SetInput(0, x);
                    Network.SetInput(1, y);
                    Network.Update();
                    float z_nn = Network.GetOutput(0);

                    score += Mathf.Pow(z - z_nn, 2f);
                }
            }

            return 1f / Mathf.Sqrt(score / (1f / (StepX * StepY)));
        }
        #endregion

        // https://catlikecoding.com/unity/tutorials/procedural-grid/
        private Mesh Mesh = null;
        private Vector3[] Vertices = null;
        //Mesh GenerateMesh (int xSize, int ySize, System.Func<float, float, float> map)
        void GenerateMesh()
        {
            //if (Mesh == null)
            //{
            Mesh = new Mesh();
            Vertices = new Vector3[(xSize + 1) * (ySize + 1)];
            //}

            UpdateMesh();

            int[] triangles = new int[xSize * ySize * 6];
            for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
            {
                for (int x = 0; x < xSize; x++, ti += 6, vi++)
                {
                    triangles[ti] = vi;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                    triangles[ti + 5] = vi + xSize + 2;
                }
            }
            Mesh.triangles = triangles;
        }

        void UpdateMesh()
        {
            for (int i = 0, y = 0; y <= ySize; y++)
            {
                for (int x = 0; x <= xSize; x++, i++)
                {
                    float xx = x / (float)xSize;
                    float yy = y / (float)ySize;

                    //Vertices[i] = new Vector3(x, y);
                    Vertices[i] = new Vector3(xx, Map(xx, yy), yy);
                }
            }
            Mesh.vertices = Vertices;

            Mesh.RecalculateBounds();
            Mesh.RecalculateNormals();
        }





        private Mesh MeshNetwork = null;
        private Vector3[] VerticesNetwork = null;
        void GenerateMeshNetwork()
        {
            //if (Mesh == null)
            //{
            MeshNetwork = new Mesh();
            VerticesNetwork = new Vector3[(xSize + 1) * (ySize + 1)];
            //}

            UpdateMeshNetwork();

            int[] triangles = new int[xSize * ySize * 6];
            for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
            {
                for (int x = 0; x < xSize; x++, ti += 6, vi++)
                {
                    triangles[ti] = vi;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                    triangles[ti + 5] = vi + xSize + 2;
                }
            }
            MeshNetwork.triangles = triangles;
        }

        void UpdateMeshNetwork()
        {
            for (int i = 0, y = 0; y <= ySize; y++)
            {
                for (int x = 0; x <= xSize; x++, i++)
                {
                    float xx = x / (float)xSize;
                    float yy = y / (float)ySize;

                    //Vertices[i] = new Vector3(x, y);
                    //VerticesNetwork[i] = new Vector3(xx, Map(xx, yy), yy);

                    float yy_nn = 0;
                    if (Network != null)
                    {
                        Network.SetInput(0, xx);
                        Network.SetInput(1, yy);
                        Network.Update();
                        yy_nn = Network.GetOutput(0);
                    }

                    VerticesNetwork[i] = new Vector3(xx, yy_nn, yy);
                }
            }
            MeshNetwork.vertices = VerticesNetwork;

            MeshNetwork.RecalculateBounds();
            MeshNetwork.RecalculateNormals();
        }


        private void OnDrawGizmos()
        {
            if (Mesh == null)
                GenerateMesh();

            UpdateMesh();
            Gizmos.color = Color.green;
            Gizmos.DrawWireMesh(Mesh, transform.position);

            /*
            // Draw curve
            for (float x = 0; x <= 1f; x += DebugX)
            {
                for (float y = 0; y <= 1f; y += DebugY)
                {
                    Vector3 point = new Vector3(x, Map(x, y), y);
                    Vector3 point_x = new Vector3(x-DebugX, Map(x- DebugX, y), y);
                    Vector3 point_y = new Vector3(x, Map(x, y- DebugY), y-DebugY);

                    // point_X to point
                    Debug.DrawLine(transform.position + point_x, transform.position + point, Color.green);
                    Debug.DrawLine(transform.position + point_y, transform.position + point, Color.green);

                    //Debug.DrawLine(transform.position + prev, transform.position + next, Color.green);
                }
            }*/


            if (Network == null)
                return;

            if (MeshNetwork == null)
                GenerateMeshNetwork();

            UpdateMeshNetwork();
            Gizmos.color = Color.blue;
            Gizmos.DrawWireMesh(MeshNetwork, transform.position);
        }


        #region Backrpop
        public int TrainSize = 10000;
        public float LearningRate = 0.01f;
        [Button(Editor = true)]
        public void Backprop ()
        {
            Start();

            float[] inputs = new float[2];
            float[] outputs = new float[1];
            for (int i = 0; i < TrainSize; i ++)
            {
                float x = Random.Range(0f, 1f);
                float y = Random.Range(0f, 1f);
                float z = Map(x, y);

                inputs[0] = x;
                inputs[1] = y;
                outputs[0] = z;
                Network.Backprop(inputs, outputs, LearningRate);
            }
        }
        #endregion
    }
}