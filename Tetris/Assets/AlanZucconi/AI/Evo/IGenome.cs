using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlanZucconi.AI.Evo
{
    // Creates a random genome
    public interface IGenomeFactory<T>
        where T : IGenome
    {
        T Instantiate();
    }

    public interface IGenome
    {
        // Creates a copy
        IGenome Copy();

        void Mutate();
    }
}