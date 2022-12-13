using UnityEngine;

using AlanZucconi.Tetris;

[CreateAssetMenu(fileName = "TetrisAI_Heuristic", menuName = "Tetris/Examples/TetrisAI_Heuristic")]
public class TetrisAI_Heuristic : TetrisAI
{
    // Loops through all the moves,
    // calculating their utility using a custom function (Heuristic)
    // and returns the index of the move with the highest heuristic
    public override int ChooseMove(Move[] moves)
    {
        return moves.IndexOfMax(move => Heuristic(move));
    }

    private float Heuristic(Move move)
    {
        // Simulates the effect of the move on the board
        TetrisState state = Tetris.SimulateMove(move);

        // Here your code to calculate the utility of this move
        // ...
        return 0;
    }
}
