using UnityEngine;
using AlanZucconi.Tetris;
using AlanZucconi.AI.Evo;
[CreateAssetMenu
(
fileName = "TetrisAI_oguve001",
menuName = "Tetris/2021-22/TetrisAI_oguve001"
)]
public class TetrisAI_oguve001 : TetrisAI
{
    //values the evolution algorithm assigns are saved in these variables
    //used during the evolution algorithm
    public float x;
    public float c;
    public float v;
    public float b;
    public float n;


    //counts number of empty cells on the board 
    public float EmptyCells(Move move)
    {
        int emptyCells = 0;
        int min = Tetris.State.GetMaxColumnHeight();
        // Simulates the effect of the move on the board
        TetrisState state = Tetris.SimulateMove(move);
        for (int x = 0; x < Tetris.Size.x; x++)
        {
            if (min > state.GetColumnHeight(x))
            {
                min = state.GetColumnHeight(x);
            }

            for (int y = 0; y < state.GetColumnHeight(x); y++)
            {
                if (state.IsEmpty(x, y))
                {
                    emptyCells++;
                }
            }
        }
        return emptyCells;
    }

    //finds bumpiness by finding the difference between column height before move simulation and after move simulation
    public float bumpiness(Move move)
    {
        int min = Tetris.State.GetMaxColumnHeight();
        // Simulates the effect of the move on the board
        TetrisState state = Tetris.SimulateMove(move);
        int bumpiness = state.GetMaxColumnHeight() - min;
        return bumpiness;
    }

    //calculates and returns maximum height
    public float maxHeight(Move move)
    {
        int maxHeight = 0;
        int max = 0;
        // Simulates the effect of the move on the board
        TetrisState state = Tetris.SimulateMove(move);
        for (int x = 0; x < Tetris.Size.x; x++)
        {
            if (state.GetColumnHeight(x) > max)
            {
                max = state.GetColumnHeight(x);
            }
            int height = state.GetColumnHeight(x);
            if (height > maxHeight)
            {
                maxHeight = height;
            }
        }
        maxHeight = maxHeight - Tetris.State.GetMaxColumnHeight();
        return maxHeight;

    }

    //calculates and returns number of holes the tetris blocks have created
    public float Holes(Move move)
    {
        int holes = 0;
        // Simulates the effect of the move on the board
        TetrisState state = Tetris.SimulateMove(move);
        for (int j = 0; j < Tetris.Size.x; j++)
        {
            for (int y = 0; y < state.GetColumnHeight(j); y++)
            {

                if (state.IsEmpty(j, y))
                {
                    holes++;
                }
            }
        }
        return holes;
    }


    //calculates and returns number of spaces between the piece that is being placed indicating that it is causing a pillar
    public float pillar(Move move)
    {
        // Simulates the effect of the move on the board
        TetrisState state = Tetris.SimulateMove(move);
        int counter = 0;

        for (int x = 0; x < Tetris.Size.x; x++)
        {
            for (int i = Tetris.Size.y - 1; i >= 0; i--)
            {
                if (state.IsFull(x, i))
                    break;

                if (state.IsFull(x - 1, i) && state.IsFull(x + 1, i))
                {
                    counter++;
                }

            }
        }
        return counter;
    }

    //calculate cost of each move
    public float Heuristic(Move move)
    {
        //call all the functions 
        float Pillar = pillar(move);
        float holes = Holes(move);
        float emptyCells = EmptyCells(move);
        float MaxHeight = maxHeight(move);
        float Bumpiness = bumpiness(move);

        //values the evolution algorithm made are used here to calculate the cost.
        //float cost = (holes * (float)x) + (float)c * emptyCells + ((float)v * MaxHeight) + Bumpiness * (float)b + Pillar * (float)n;


        //calculate cost of the move using the factors returns and the multipliers
        float cost = (holes * (float)18.3995) + (float)12.411025 * emptyCells + ((float)12.597177 * MaxHeight) + Bumpiness * (float)0 + Pillar * (float)10.2577379;

        //return the cost of the move
        return cost;
    }
    public override int ChooseMove(Move[] moves)
    {
        //perform the move with the lowest cost
        return moves.IndexOfMin(move => Heuristic(move));
    }
}
