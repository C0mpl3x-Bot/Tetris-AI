using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;

using Isotope.Collections;

namespace AlanZucconi.Tetris
{
    public class TetrisState
    {

        public Vector2Int Size;
        public BitArray2D Board;
        //public bool[,] Board;





        public TetrisState(int x, int y)
        {
            Size = new Vector2Int(x, y);
            Board = new BitArray2D(x, y);
            //Board = new bool[x, y];
        }


        public void Clear()
        {
            Board.SetAll(false);
            //Array.Clear(Board, 0, Board.Length);
        }


        public int Width
        {
            get { return Board.Width; }
            //get { return Board.GetLength(0); }
        }
        public int Height
        {
            get { return Board.Height; }
            //get { return Board.GetLength(1); }
        }




        /// <summary>
        /// <para>Sets a cell in the board.</para>
        /// </summary>
        /// 
        /// <param name="x">The x coordinate of the cell.</param>
        /// <param name="y">The y coordinate of the cell.</param>
        /// <param name="state">The new state of the cell (default <see langword="true"/>).</param>
        /// 
        /// <seealso cref="Get(int, int)"/>
        public void Set(int x, int y, bool state = true)
        {
            Board[x, y] = state;
        }


        /// <summary>
        /// <para>Gets the state of a cell in the board</para>
        /// </summary>
        /// 
        /// <param name="x">The x coordinate of the cell.</param>
        /// <param name="y">The y coordinate of the cell.</param>
        /// 
        /// <returns>
        ///     <para><see langword="true"/> if the cell il full.</para>
        ///     <para><see langword="false"/> if the cell is empty.</para>
        ///     <para>All cells outside the boards return <see langword="true"/>,
        ///     because they are not free.</para>
        /// </returns>
        /// 
        /// <seealso cref="Set(int, int, bool)"/>
        public bool Get(int x, int y)
        {
            // Out of bounds => not empty ==> true
            if (x < 0 || x >= Size.x ||
                y < 0 || y >= Size.y)
                return true;

            return Board[x, y];
        }





        /// <summary>
        /// <para>Returns <see langword="true"/> if there is a piece at (<paramref name="x"/>,<paramref name="y"/>).</para>
        /// </summary>
        /// 
        /// <param name="x">The x coordinate of the cell.</param>
        /// <param name="y">The y coordinate of the cell.</param>
        /// 
        /// <returns>
        ///     <para><see langword="true"/> if the cell il full.</para>
        ///     <para><see langword="false"/> if the cell is empty.</para>
        ///     <para>All cells outside the boards return <see langword="true"/>,
        ///     because they are not free.</para>
        /// </returns>
        /// 
        /// <seealso cref="IsEmpty(int, int)"/>
        public bool IsFull(int x, int y)
        {
            return Get(x, y);
        }

        /// <summary>
        /// <para>Returns <see langword="true"/> if there is no piece at (<paramref name="x"/>,<paramref name="y"/>).</para>
        /// </summary>
        /// 
        /// <param name="x">The x coordinate of the cell.</param>
        /// <param name="y">The y coordinate of the cell.</param>
        /// 
        /// <returns>
        ///     <para><see langword="true"/> if the cell il empty.</para>
        ///     <para><see langword="false"/> if the cell is full.</para>
        ///     <para>All cells outside the boards return <see langword="false"/>,
        ///     because they are not free.</para>
        /// </returns>
        /// 
        /// <seealso cref="IsFull(int, int)"/>
        public bool IsEmpty(int x, int y)
        {
            return !Get(x, y);
        }



        #region FullRows
        /// <summary>
        /// <para>Returns <see langword="true"/> if the <paramref name="y"/>-th row is full.</para>
        /// </summary>
        /// 
        /// <param name="y">The y coordinate of the row. Row 0 starts at the bottom.</param>
        /// 
        /// <returns>
        ///     <para><see langword="true"/> if the row is full.</para>
        ///     <para><see langword="false"/> if the row is not full.</para>
        ///     <para>All rows outside the boards return <see langword="true"/>,
        ///     because they are not free.</para>
        /// </returns>
        /// 
        /// <seealso cref="FirstFullRow"/>
        /** Returns true if the i-th row is full.
         * Row 0 starts at the bottom.
         */
        public bool IsRowFull(int y)
        {
            //for (int x = 0; x < Board.GetLength(0); x ++)
            for (int x = 0; x < Size.x; x++)
                if (IsEmpty(x, y))
                    return false;
            return true;
        }

        /// <summary>
        /// <para>Returns the index of the first full row.
        /// Or -1 if there are none.</para>
        /// </summary>
        /// 
        /// <seealso cref="IsRowFull(int)"/>
        public int FirstFullRow()
        {
            for (int y = 0; y < Size.y; y++)
                if (IsRowFull(y))
                    return y;

            return -1;
        }



        /** Shift the board down a row.
         * This replaces the selected row.
         * The top row is set to empty.
         * 
         * Once called, this method is executed even if the selected row is not full.
         */
        public void ShiftDown(int startY)
        {
            // Shifts down
            //for (int y = 0; y < Board.GetLength(1) - 1; y ++)
            //    for (int x = 0; x < Board.GetLength(0); x ++)
            for (int y = startY; y < Size.y - 1; y++)
                for (int x = 0; x < Size.x; x++)
                    Set(x, y, Get(x, y + 1));

            // Clears top row
            //for (int x = 0; x < Board.GetLength(0); x++)
            for (int x = 0; x < Size.x; x++)
                Set(x, Size.y - 1, false);
        }


        /** Deletes all full rows.
         * Returns the number of rows that were cleared.
         */
        public int ClearAllFullRows()
        {
            int cleared = 0;
            int y;
            while ((y = FirstFullRow()) != -1)
            {
                ShiftDown(y);
                cleared++;
            }
            return cleared;
        }
        #endregion


        /// <summary>
        /// <para>Returns a clone/copy of this tetris state object.</para>
        /// </summary>
        /// 
        /// <seealso cref="IsRowFull(int)"/>
        /// 
        /** Creates a copy of this object. */
        public TetrisState Clone()
        {
            TetrisState state = new TetrisState(Size.x, Size.y);
            state.Board = Board.Clone();
            //Array.Copy(Board, state.Board, Size.x * Size.y);

            return state;
        }





        #region TetrominoPlacement
        /** Return true if the tetromino can be placed at (x,y).
         */
        public bool IsValidPlacement(Tetromino tetromino, int x, int y)
        {
            for (int tx = 0; tx < tetromino.Width; tx++)
                for (int ty = 0; ty < tetromino.Height; ty++)
                {
                    // If the tetromino has a block,
                    // the board must be empty
                    if (tetromino.Area[tx, ty] &&
                        Get(x + tx, y + ty))
                        return false;
                }

            return true;
        }

        /** Iterates over all possible placements of the given tetromino
         * on the top row.
         * This method is used as a first step to calculate all valid final placements.
         */
        private IEnumerable<Vector2Int> FindValidTopPlacements(Tetromino tetromino)
        {
            for (int x = 0; x < Size.x; x++)
            {
                // Places the tetromino at the top of the board
                int y = Size.y - 0 - tetromino.Height;
                if (IsValidPlacement(tetromino, x, y))
                    yield return new Vector2Int(x, y);
            }
        }

        /** Given a tetromino, generates all (x, y) final positions in which
         * it can go.
         * 
         * This method should not be used directly,
         * as it can enumerate duplicate positions.
         * The results should be filtered using Distinct().
         */
        private IEnumerable<Vector2Int> FindValidBottomPlacements(Tetromino tetromino)
        {
            // First: finds all possible horizontal placements
            foreach (Vector2Int p in FindValidTopPlacements(tetromino))
            {
                //yield return p;

                // Moves the tetromino down until it hits something
                // It goes to -1, so it is guaranteed that the last iteration hits
                //for (int y = Size.y -1 - tetromino.Height; y >= 0; y--)
                for (int y = p.y; y >= 0 - 1; y--)
                {
                    if (!IsValidPlacement(tetromino, p.x, y))
                    {
                        // If this placement is invalid,
                        // it means we hit something
                        // So we return the previous value (y+1)
                        // which is always working
                        yield return new Vector2Int(p.x, y + 1);
                        /*
                        // Now that we know a possible move,
                        // we also need to see if we can move it left/right.
                        // This is because there might be some interlocking
                        // moves that can only be done like this.
                        if (IsValidPlacement(tetromino, p.x - 1, y + 1))
                            yield return new Vector2Int(p.x - 1, y + 1);
                        if (IsValidPlacement(tetromino, p.x + 1, y + 1))
                            yield return new Vector2Int(p.x + 1, y + 1);
                        // Doing things this way means that there might be duplicated
                        // moves returned by this method.
                        // They need to be filtered out.
                        */
                        break; // Collision ==> stops here!
                    }
                }
            }
        }


        /** Given a tetromino, generates all (x, y) final positions in which
         * it can go.
         * 
         * This method does not returns duplicates.
         */
        public IEnumerable<Move> FindValidPlacements(Tetromino tetromino)
        {
            // Because FindValidBottomPlacements can return duplicates,
            // we use Distinct() to filter them out.
            return FindValidBottomPlacements(tetromino)
                .Distinct()
                .Select(position => new Move(tetromino, position))
                ;
        }


        /** Given a tetromino list, it returns all valid final placements.
         */
        //public IEnumerable<Vector2Int> FindValidPlacements (Tetromino[] tetrominos)
        public IEnumerable<Move> FindValidPlacements(Tetromino[] tetrominos)
        {
            //return tetrominos
            //    // Flattens [list of [list of valid moves]] to [list of valid moves]
            //    .SelectMany(tetromino => FindValidPlacements(tetromino))
            //    ;

            foreach (Tetromino tetromino in tetrominos)
                foreach (Move move in FindValidPlacements(tetromino))
                    yield return move;
        }
        #endregion


        /** Places a tetromino on the board.
         * If the placement overlaps with cells which are already fulls,
         * an exception is returned.
         * 
         * This is to force students to deal with the error,
         * in case they do something wrong!
         */
        public void Place(Tetromino tetromino, int x, int y)
        {
            for (int tx = 0; tx < tetromino.Width; tx++)
                for (int ty = 0; ty < tetromino.Height; ty++)
                {
                    // If the tetromino has a block,
                    // sets it on the board
                    if (tetromino.Area[tx, ty])
                    {
                        // Board cell must be empty!
                        if (IsFull(x + tx, y + ty))
                            throw new Exception("Tetromino placement out of board!");

                        Set(x + tx, y + ty);
                    }
                }
        }

        public void Place(Move move)
        {
            Place(move.Tetromino, move.Position.x, move.Position.y);
        }


        /** Simulates a move.
         * Given a move, it returns a copy of the current tetris state
         * with the move already performed.
         * 
         * This is useful to simulate a move without messing the current board.
         * 
         * If compact is true, it clears the rows that are full.
         */
        public TetrisState SimulateMove(Move move, bool compact = true)
        {
            TetrisState state = Clone();
            state.Place(move);
            if (compact)
                state.ClearAllFullRows();
            return state;
        }


        #region Heuristics
        // These methods are used for heuristics
        // but are not used directly in the code
        //
        // For these reasons, they often raise exceptions
        // when used outside the board area.
        ///
        /** 
         */

        // <see langword="true"/>
        // <seealso cref="TurnRight"/>


        /// <summary>
        /// <para>Calculates the height of the <paramref name="x"/>-th column.</para>
        /// </summary>
        /// 
        /// <seealso cref="IsRowFull(int)"/>
        public int GetColumnHeight(int x)
        {
            int y = Size.y - 1;
            while (IsEmpty(x, y))
                y--;

            return y;
        }

        /** Gets the maximum height on the board.
         */
        public int GetMaxColumnHeight()
        {
            return Enumerable
                .Range(0, Size.x)
                .Select(x => GetColumnHeight(x))
                .Max();
        }


        /** Returns true if the x-th column is empty.
         */
        public bool IsColumnEmpty(int x)
        {
            return Enumerable
                .Range(0, Size.y)
                .All(y => IsEmpty(x, y));
        }

        #endregion
    }
}