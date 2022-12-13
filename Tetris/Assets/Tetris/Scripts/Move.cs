using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlanZucconi.Tetris
{
    /** This struct contains the information about a move.
     */
    public struct Move
    {
        public Tetromino Tetromino;
        public Vector2Int Position;

        public Move (Tetromino tetromino, Vector2Int position)
        {
            Tetromino = tetromino;
            Position  = position;
        }
    }
}