using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

using AlanZucconi.Data;

namespace AlanZucconi.AI.Evo
{
    /*
     * All worlds linked are executed in parallel, and recycled.
     * They need to be linked manually.
     */
    public class EvolutionSystem<T> : MonoBehaviour
        where T : IGenome
    {
        public IGenomeFactory<T> Factory;
        // Population size = worlds.Count
        public List<IWorld<T>> Worlds;
        public List<T> Population;

        [Header("Manual initialisation")]
        public bool AddFirstGenome = false;
        // This one genome is added to the first generation
        public T FirstGenome;

        [Header("Settings")]
        [Min(1)]
        public int Generations;
        [Range(0f,1f)]
        public float SurvivalRate; // % of worlds that survive between generations

        [Min(1)]
        public int Mutations; // How many mutations

        [Min(1)]
        public int TestsPerGenome = 1;


        [Header("Results")]
        [LinePlot(LabelX = "Generations", LabelY = "Score")]
        public PlotData PlotData = new PlotData();

        [Button]
        public void StartEvolution ()
        {
            StartCoroutine(StartEvolutionCoroutine());
        }

        IEnumerator StartEvolutionCoroutine ()
        {
            // ======================
            // === INITIALISATION ===
            // ======================

            // FindObjectsOfType cannot retrieve interfaces
            // So we get all monobehaviours and filter for World<T>
            Worlds = FindObjectsOfType<MonoBehaviour>()
                .OfType<IWorld<T>>()
                .ToList();
            //Worlds = FindObjectsOfType<World<T>>()
        
            // Uses the first GenomeFactory<T> to instantiate the genomes
            Factory = FindObjectsOfType<MonoBehaviour>()
                .OfType<IGenomeFactory<T>>()
                .First();
            //Factory = Worlds[0] as GenomeFactory<T>; // Uses the first one as a factory

            // Waits one frame to make sure Awake() and Start() have been called
            yield return null;


            // ========================
            // === FIRST POPULATION ===
            // ========================
            if (AddFirstGenome)
            {
                // Adds the first genome
                Population.Add((T)FirstGenome.Copy());
                // Adds mutations of the first genome
                for (int i = 0 + 1; i < Worlds.Count; i++)
                {
                    T genome = (T)FirstGenome.Copy();
                    int mutations = Random.Range(0, Mutations);
                    for (int m = 0; m < mutations; m++)
                        genome.Mutate();

                    Population.Add(genome);
                }
            }
            else
            {
                // Initialises the random population
                for (int i = Population.Count(); i < Worlds.Count; i++)
                {
                    T genome = (T)Factory.Instantiate();
                    Population.Add(genome);
                }
            }
       

            // ======================
            // === EVOLUTION LOOP ===
            // ======================

            // Loops through the generations
            for (int generation = 0; generation < Generations; generation++)
            {
                Debug.Log("Generation: " + (generation+1));

                // -----------------------------------------
                // [TESTS]
                // Tests each world a numer of times
                // to make sure the score are reliable

                // Associates a list of scores to each world
                Dictionary<IWorld<T>, List<float>> scores = new Dictionary<IWorld<T>, List<float>>();
                foreach (IWorld<T> world in Worlds)
                    scores.Add(world, new List<float>());

                // Loops through all the necessary tests per genome
                for (int test = 0; test < TestsPerGenome; test++)
                {
                    Debug.Log("\tTest: " + (test+1));

                    // [SETUP]
                    // Initialises the worlds
                    //foreach (World<T> world in Worlds)
                    for (int i = 0; i < Worlds.Count; i++)
                    {
                        IWorld<T> world = Worlds[i];
                        T genome = Population[i];

                        world.ResetSimulation();
                        world.SetGenome(genome);
                        world.StartSimulation();
                    }

                    
                    // -----------------------------------------
                    // [SIMULATION]
                    // Waits for all worlds to be done
                    yield return new WaitUntil
                    (
                        () => Worlds.All(world => world.IsDone())
                    );

                    // -----------------------------------------
                    // Adds the score to the scores list
                    foreach (IWorld<T> world in Worlds)
                        scores[world].Add(world.GetScore());
                }
                // -----------------------------------------
                // [FITNESS]
                // Gets the top genomes
                List<T> topGenomes = Worlds
                    .Rank(world => scores[world].Average(), (int)(Worlds.Count * SurvivalRate))
                    .Select(world => world.GetGenome())
                    .ToList();

                //foreach (IWorld<T> world in Worlds)
                //    Debug.Log(world + "\t" + scores[world].Count() + "\t" + scores[world].Average());

                /*
                List<T> topGenomes = Worlds
                    .Rank(world => world.GetScore(), (int)(Worlds.Count * SurvivalRate))
                    .Select(world => world.GetGenome())
                    .ToList();
                */
                // Updates scores
                float maxScore = Worlds
                    .Select(world => scores[world].Average())
                    .Max();
                PlotData.Add
                (
                    new Vector2(generation,maxScore)
                );
                Debug.Log("\t=> Best score: " + maxScore);
                Debug.Log("\t=> Best genome: " + topGenomes[0]);

                // Mutations from the top genomes
                Population.Clear();
                // Adds the best one back
                Population.Add( (T) topGenomes[0].Copy() );
                
                // Adds the remaining ones and mutates them
                for (int i = 0+1; i < Worlds.Count; i ++)
                {
                    T genome = (T) topGenomes[Random.Range(0, topGenomes.Count)].Copy();
                    int mutations = Random.Range(0, Mutations);
                    for (int m = 0; m < mutations; m++)
                        genome.Mutate();

                    Population.Add(genome);
                }

                // Waits next frame before restarting
                yield return null;
            }
        }
    }
}