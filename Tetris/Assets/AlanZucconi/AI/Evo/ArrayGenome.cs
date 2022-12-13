using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AlanZucconi.AI.Evo
{
    // An array which can be used with the evolution algorithm
    [System.Serializable]
    public struct ArrayGenome : IGenome
    {
        public float[] Params;

        public ArrayGenome (int n)
        {
            Params = new float[n];
        }

        #region Evolution
        // Copies this genome
        public IGenome Copy()
        {
            ArrayGenome copy = new ArrayGenome();
            copy.Params = (float[]) Params.Clone();

            return copy;
        }

        // Picks a random element and mutates it
        public void Mutate()
        {
            float MAX_MUTATION = 0.1f;

            int i = Random.Range(0, Params.Length);


            if (Random.Range(0f, 1f) >= 0.1f)
            {
                // 90% chance of small change
                float value = Params[i] + Random.Range(-MAX_MUTATION, +MAX_MUTATION);
                Params[i] = Mathf.Clamp(value, -1f, +1f);
            } else
            {
                // 10% change of completely new value
                Params[i] = Random.Range(-1f, +1f);
            }
        }

        public void InitialiseRandom()
        {
            for (int i = 0; i < Params.Length; i++)
                Params[i] = Random.Range(-1f, +1f);
        }
        #endregion

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.Append("[");
            for (int i = 0; i < Params.Length; i++)
            {
                s.Append(Params[i]);
                s.Append(", ");
            }
            s.Append("]");

            return s.ToString();
        }
    }
}