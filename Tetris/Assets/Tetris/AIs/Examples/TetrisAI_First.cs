using UnityEngine;

using AlanZucconi.Tetris;


[CreateAssetMenu(fileName = "TetrisAI_First", menuName = "Tetris/Examples/TetrisAI_First")]
public class TetrisAI_First : TetrisAI
{
    // Performs the first available action
    public override int ChooseMove(Move[] moves)
    {
        return 0;
    }
}
