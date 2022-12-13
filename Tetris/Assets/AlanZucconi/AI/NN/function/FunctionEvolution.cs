using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

using AlanZucconi.AI.Evo;

namespace AlanZucconi.AI.NN.Test
{
    public class FunctionEvolution : EvolutionSystem<NeuralNetworkData>
    {
        /*
        IEnumerator Start()
        {
            Worlds = FindObjectsOfType<Function>()
                // List<RocketGame> -> List<World<NeuralNetworkData>
                .Select(world => world as World<NeuralNetworkData>)
                .ToList();

            //foreach (World<NeuralNetworkData> world in Worlds)
            //    (world as Function).Start();

            // Uses the first rocketgame to instantiate the genomes
            Factory = Worlds[0] as GenomeFactory<NeuralNetworkData>; // Uses the first one as a factory

            yield return null;

            StartEvolution();
        }
        */
    }
}