using UnityEngine;

using AlanZucconi.Tetris;

[CreateAssetMenu(fileName = "TetrisAI_Random", menuName = "Tetris/Examples/TetrisAI_Random")]
public class TetrisAI_Random : TetrisAI
{
    // Performs random actions
    public override int ChooseMove(Move[] moves)
    {
        return Random.Range(0, moves.Length);
    }
}
