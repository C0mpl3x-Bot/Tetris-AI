using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AlanZucconi.Tetris
{
    // https://codeincomplete.com/articles/javascript-tetris/
    public class Tetromino
    {
        public bool[,] Area;

        public Tetromino(bool[,] area)
        {
            Area = area;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder("", Area.Length);
            for (int y = 0; y < Area.GetLength(1); y++)
            {
                for (int x = 0; x < Area.GetLength(0); x++)
                    s.Append(Area[x,y] ? 'X' : '-' );
                s.AppendLine();
            }

            return s.ToString();
        }


        public int Width
        {
            get { return Area.GetLength(0); }
        }
        public int Height
        {
            get { return Area.GetLength(1); }
        }



        public static Tetromino I0 = new Tetromino
        (
            new bool[,]
            {
                { true },
                { true },
                { true },
                { true }
            }
        );
        public static Tetromino I1 = new Tetromino
        (
            new bool[,]
            {
                { true,  true,  true,  true  },
            }
        );



        public static Tetromino J0 = new Tetromino
        (
            new bool[,]
            {
                { false, true },
                { false, true },
                { true,  true },
            }
        );
        public static Tetromino J1 = new Tetromino
        (
            new bool[,]
            {
                { true,  false, false },
                { true,  true,  true  },

            }
        );
        public static Tetromino J2 = new Tetromino
        (
            new bool[,]
            {
                { true,  true  },
                { true,  false },
                { true,  false },
            }
        );
        public static Tetromino J3 = new Tetromino
        (
            new bool[,]
            {
                { true,  true,  true },
                { false, false, true },
            }
        );



        public static Tetromino L0 = new Tetromino
        (
            new bool[,]
            {
                { true,  false },
                { true,  false },
                { true,  true  },

            }
        );
        public static Tetromino L1 = new Tetromino
        (
            new bool[,]
            {
                { true,  true,  true  },
                { true,  false, false },
            }
        );
        public static Tetromino L2 = new Tetromino
        (
            new bool[,]
            {
                { true,  true },
                { false, true },
                { false, true },
            }
        );
        public static Tetromino L3 = new Tetromino
        (
            new bool[,]
            {
                { false, false, true },
                { true,  true,  true },
            }
        );



        public static Tetromino O0 = new Tetromino
        (
            new bool[,]
            {
                { true,  true },
                { true,  true },
            }
        );



        public static Tetromino S0 = new Tetromino
        (
            new bool[,]
            {
                { false, true,  true  },
                { true,  true,  false },
            }
        );
        public static Tetromino S1 = new Tetromino
        (
            new bool[,]
            {
                { true,  false },
                { true,  true  },
                { false, true  },
            }
        );



        public static Tetromino T0 = new Tetromino
        (
            new bool[,]
            {
                { false, true,  false },
                { true,  true,  true  },
            }
        );
        public static Tetromino T1 = new Tetromino
        (
            new bool[,]
            {
                { true,  false },
                { true,  true  },
                { true,  false },
            }
        );
        public static Tetromino T2 = new Tetromino
        (
            new bool[,]
            {
                { true,  true,  true  },
                { false, true,  false },
            }
        );
        public static Tetromino T3 = new Tetromino
        (
            new bool[,]
            {
                { false, true },
                { true,  true },
                { false, true },
            }
        );



        public static Tetromino Z0 = new Tetromino
        (
            new bool[,]
            {
                { true,  true,  false },
                { false, true,  true  },
            }
        );
        public static Tetromino Z1 = new Tetromino
        (
            new bool[,]
            {
                { false, true  },
                { true,  true  },
                { true,  false },
            }
        );



        // Fixed tetrominos
        public static Tetromino[] Is = new Tetromino[] { I0, I1 };
        public static Tetromino[] Js = new Tetromino[] { J0, J1, J2, J3 };
        public static Tetromino[] Ls = new Tetromino[] { L0, L1, L2, L3 };
        public static Tetromino[] Os = new Tetromino[] { O0 };
        public static Tetromino[] Ss = new Tetromino[] { S0, S1 };
        public static Tetromino[] Ts = new Tetromino[] { T0, T1, T2, T3 };
        public static Tetromino[] Zs = new Tetromino[] { Z0, Z1 };

        // All the groups
        public static Tetromino[][] TetrominoGroups =
            new Tetromino[][]
            {
                Is, Js, Ls, Os, Ss, Ts, Zs
            };

        // All the variants
        // This array should not be used to pick a random tetromino,
        // because it would yield unbalanced results
        // (i.e.: there are more rotations for J than Z)
        public static Tetromino[] All = new Tetromino[]
        {
            I0, I1,
            J0, J1, J2, J3,
            L0, L1, L2, L3,
            O0,
            S0, S1,
            T0, T1, T2, T3,
            Z0, Z1
        };
    }
}