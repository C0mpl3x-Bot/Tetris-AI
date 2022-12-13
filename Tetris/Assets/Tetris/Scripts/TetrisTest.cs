using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

namespace AlanZucconi.Tetris
{
    public class TetrisTest : MonoBehaviour
    {

        public TetrisGame Game;

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("== I ==");
            foreach (Tetromino t in Tetromino.Is)
                Debug.Log(t);

            Debug.Log("== J ==");
            foreach (Tetromino t in Tetromino.Js)
                Debug.Log(t);

            Debug.Log("== L ==");
            foreach (Tetromino t in Tetromino.Ls)
                Debug.Log(t);

            Debug.Log("== O ==");
            foreach (Tetromino t in Tetromino.Os)
                Debug.Log(t);

            Debug.Log("== S ==");
            foreach (Tetromino t in Tetromino.Ss)
                Debug.Log(t);

            Debug.Log("== T ==");
            foreach (Tetromino t in Tetromino.Ts)
                Debug.Log(t);

            Debug.Log("== Z ==");
            foreach (Tetromino t in Tetromino.Zs)
                Debug.Log(t);
        }

        [Header("Test Tetromino Placement")]
        public Vector2Int Position;
        [Range(0,9)]
        public int Skip = 0;

        // Update is called once per frame
        void Update()
        {
            Game.State.Clear();
            Tetromino t = Tetromino.Z1;

            Game.State.Place(t, Position.x, Position.y);

            //Debug.Log(Game.State.FindValidPlacements(t).Count());
            //Debug.Log(Game.State.FindValidHorizontalPlacements(t).Count());


            //Vector2Int p = Game.State
            Move move = Game.State
                //.FindValidPlacements(t)
                .FindValidPlacements(t)
                .Skip(Skip)
                .First();
            Game.State.Place(move);

            Game.DrawBoard();
        }
    }
}