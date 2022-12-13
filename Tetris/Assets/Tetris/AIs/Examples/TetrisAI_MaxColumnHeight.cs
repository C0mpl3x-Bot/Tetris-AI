using UnityEngine;

using AlanZucconi.Tetris;

[CreateAssetMenu(fileName = "TetrisAI_MaxColumnHeight", menuName = "Tetris/Examples/TetrisAI_MaxColumnHeight")]
public class TetrisAI_MaxColumnHeight : TetrisAI
{
    // Loops through all the moves,
    // calculating the height of the tallest column for each one,
    // and returns the index of the move with the lowest tallest column
    public override int ChooseMove(Move[] moves)
    {
        return moves.IndexOfMin(move => MaxColumnHeight(move));
    }

    private float MaxColumnHeight(Move move)
    {
        // Simulates the effect of the move on the board
        TetrisState state = Tetris.SimulateMove(move);

        // Calculates the height of the tallest column
        return state.GetMaxColumnHeight();
    }
}
