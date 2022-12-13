using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using UnityEngine.Tilemaps;

namespace AlanZucconi.Tetris
{
    public class TetrisGame : MonoBehaviour
    {
        [Header("Game")]
        [EditorOnly]
        public Vector2Int Size = new Vector2Int(10, 20);
        [Range(0, 5)]
        public float Delay = 1; // seconds

        [Space]
        [ReadOnly]
        public bool Running = false;
        [ReadOnly]
        public int Turn = 0;

        //[Space]
        // TODO: implement
        //public bool PauseOnDeath = false;


        [Header("Rendering")]
        [EditorOnly]
        public Tilemap Tilemap;
        [EditorOnly]
        public Tile BlockTile;
        public bool Rendering = true;



        



        public TetrisState State;


        public enum SequenceType
        {
            Random, // A random seed for the tetromino sequence
            Seeded  // An input seed for the tetromino sequence
        }

        [Header("Randomness")]
        // If true, the tetromino sequence is random
        public SequenceType Sequence = SequenceType.Random;
        // Used to randomise the sequence of tetrominos
        [ShowIf("Sequence", SequenceType.Seeded)]
        public int TetrominoSalt;


        [Header("AI")]
        public TetrisAI TetrisAI = null;
        //public DerivedType TetrisAIType = new DerivedType(typeof(TetrisAI));
        //private TetrisAI TetrisAI = null;

        void Awake()
        {
            // Allows the tile to change colour
            BlockTile.flags = TileFlags.None;
        }


        void Start()
        {
            State = new TetrisState(Size.x, Size.y);


            DrawBoardBorder();
        }



        #region GameLoop
        [Button(Editor=false)]
        public void StartGame ()
        {
            StartCoroutine(GameLoop_Coroutine());
        }
        public IEnumerator GameLoop_Coroutine()
        {
            // Only one running
            if (Running)
                yield break;

            // Instantiates the AI
            //TetrisAI = TetrisAIType.Instantiate<TetrisAI>();
            //TetrisAI.Tetris = this;
            InitialiseAI();


            // If the tetromino sequence is random,
            // we initialise the tetromino salt randomly
            // Otherwise, we skip this step
            // and leave its valeue unchange
            if (Sequence == SequenceType.Random)
                TetrominoSalt = Random.Range(int.MinValue, int.MaxValue);

            Turn = 0;

            Running = true;

            State.Clear();

            //for (int x = 0; x < Size.x; x++)
            //    State.Set(x, 0);

            // Game loop
            while (Running)
            {
                Turn++;


                Move[] moves = AvailableMoves();

                // No moves available: game over
                if (moves == null)
                {
                    StopGame();
                    yield break;
                }


                int i = TetrisAI.ChooseMove(moves);
                Move move = moves[i];
                State.Place(move.Tetromino, move.Position.x, move.Position.y);

                DrawBoard();
                ColorTetromino(move.Tetromino, move.Position, Color.red);
                //yield return new WaitForSeconds(Delay / 2f);
                yield return new WaitForSeconds(Delay);
                ColorTetromino(move.Tetromino, move.Position, Color.white);
                //yield return new WaitForSeconds(Delay / 2f);


                // Deletes full rows
                int y;
                while ((y = State.FirstFullRow()) != -1)
                {
                    ColourRow(y, Color.red);
                    yield return new WaitForSeconds(Delay / 2f);

                    State.ShiftDown(y);

                    ColourRow(y, Color.white);
                    DrawBoard();
                    yield return new WaitForSeconds(Delay / 2f);
                }
            }
            Running = false;
        }

        [Button(Editor = false)]
        public void StopGame ()
        {
            Running = false;
        }
        #endregion


        #region AI
        public void SetAI (TetrisAI ai)
        {
            TetrisAI = ai;
        }
        public void InitialiseAI()
        {
            TetrisAI.Tetris = this;
            TetrisAI.Initialise();
        }
        #endregion




        /** Simulates a move.
         * Given a move, it returns a copy of the current tetris state
         * with the move already performed.
         * 
         * This is useful to simulate a move without messing the current board.
         */
        public TetrisState SimulateMove (Move move, bool compact = true)
        {
            return State.SimulateMove(move, compact);
        }




        #region MovesAndTetromino
        /** Gets a list of available moves.
         * Returns null if there are no avaialble moves.
         */
        public Move[] AvailableMoves ()
        {
            Tetromino[] tetrominos = GetCurrentTetrominos();

            return State
                .FindValidPlacements(tetrominos)
                .ToArrayOrNull();
        }
        /** Returns the current tetromino groups.
         * This is an array that includes the tetromino, and all of its rotations.
         * 
         * The current tetromino is random.
         * To ake sure this is predictable, it depends on the current turn
         * and a random number generated at the start of the game.
         */
        public Tetromino[] GetCurrentTetrominos(int turn)
        {
            // Seed based on the current turn (so is predictable)
            int seed = turn ^ TetrominoSalt;
            System.Random rnd = new System.Random(seed);

            int i = rnd.Next(Tetromino.TetrominoGroups.Length);
            return Tetromino.TetrominoGroups[i];
        }
        public Tetromino[] GetCurrentTetrominos()
        {
            return GetCurrentTetrominos(Turn);
        }
        #endregion




        #region Rendering
        // Draws the border of the tetris board
        private void DrawBoardBorder ()
        {
            if (!Rendering)
                return;

            // Tetris board starts at (0,0)
            // and end and (Size.x-1, Size.y-1)

            // Horizontal
            for (int x = 0-1; x < Size.x+1; x ++)
            {
                // Bottom
                Tilemap.SetTile(new Vector3Int(x, 0 - 1, 0), BlockTile);
                Tilemap.SetColor(new Vector3Int(x, 0 - 1, 0), Color.grey);

                // Top
                Tilemap.SetTile(new Vector3Int(x, Size.y, 0), BlockTile);
                Tilemap.SetColor(new Vector3Int(x, Size.y, 0), Color.grey);
            }

            // Vertical
            for (int y = 0 - 1; y < Size.y + 1; y++)
            {
                // Left
                Tilemap.SetTile(new Vector3Int(0 -1, y, 0), BlockTile);
                Tilemap.SetColor(new Vector3Int(0 - 1, y, 0), Color.grey);

                // Top
                Tilemap.SetTile(new Vector3Int(Size.x, y, 0), BlockTile);
                Tilemap.SetColor(new Vector3Int(Size.x, y, 0), Color.grey);
            }
        }



        public void DrawBoard ()
        {
            if (!Rendering)
                return;

            for (int x = 0; x < State.Width; x++)
                for (int y = 0; y < State.Height; y++)
                    Tilemap.SetTile
                    (
                        new Vector3Int(x, y, 0),
                        State.IsFull(x, y)
                            ? BlockTile
                            : null
                    );
        }

        /** Colours a row. */
        private void ColourRow (int y, Color color)
        {
            if (!Rendering)
                return;

            for (int x = 0; x < Size.x; x++)
                Tilemap.SetColor(new Vector3Int(x, y, 0), color);
        }


        /** Colours a tetromino.
         */
        private void ColorTetromino(Tetromino tetromino, Vector2Int position, Color color)
        {
            if (!Rendering)
                return;

            for (int tx = 0; tx < tetromino.Width; tx++)
                for (int ty = 0; ty < tetromino.Height; ty++)
                    if (tetromino.Area[tx, ty])
                        Tilemap.SetColor(new Vector3Int(position.x + tx, position.y + ty, 0), color);
        }
        #endregion
    }
}