using UnityEngine;
using AlanZucconi.Tetris;
using AlanZucconi.AI.Evo;


public class TetrisWorld_oguve001 :
    MonoBehaviour,
    IWorld<ArrayGenome>,
    IGenomeFactory<ArrayGenome>
{
    public TetrisGame Tetris;
    public TetrisAI_oguve001 AI;
    private ArrayGenome Genome;

    public void SetGenome(ArrayGenome genome)
    {
        AI = ScriptableObject.CreateInstance<TetrisAI_oguve001>();
        AI.x = genome.Params[0] * 10f + 10f;
        AI.c = genome.Params[1] * 10f + 10f;
        AI.v = genome.Params[2] * 10f + 10f;
        AI.b = genome.Params[3] * 10f + 10f;
        AI.n = genome.Params[4] * 10f + 10f;

        Tetris.TetrisAI = AI;
        Genome = genome;

    }

    public ArrayGenome GetGenome()
    {
        return Genome;
    }

    public void StartSimulation()
    {
        Tetris.StartGame();
    }

    public bool IsDone()
    {
        return !Tetris.Running;
    }

    public float GetScore()
    {
        return Tetris.Turn;
    }

    public new ArrayGenome Instantiate()
    {
        ArrayGenome genome = new ArrayGenome(5);
        genome.InitialiseRandom();
        return genome;
    }

    public void ResetSimulation()
    {
        AI = ScriptableObject.CreateInstance<TetrisAI_oguve001>();
        Tetris.TetrisAI = AI;
    }
}