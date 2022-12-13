using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AlanZucconi.Data;

namespace AlanZucconi.Tetris
{
    public abstract class TetrisAI : ScriptableObject
    {
        [Header("Student Data")]
        public string StudentLogin = "yourlogin";
        public string StudentName = "FirstName LastName";
        public string StudentEmail = "youremail@gold.ac.uk";


        // Will be initialised by the Automation tool
        [Header("Statistics")]
        [Space]
        [ReadOnly]
        public float MedianScore = 0;
        [ReadOnly]
        public float AverageScore = 0;

        [Header("Results")]
        //[LinePlot(LabelX = "test", LabelY = "points")]
        //[ScatterPlot(LabelX = "test", LabelY = "points")]
        //[HistogramPlot(Bins=15, LabelX = "points", LabelY = "count")]
        [HistogramPlot(LabelX = "points", LabelY = "count")]
        public PlotData PlotData = new PlotData();


        



        [HideInInspector]
        public TetrisGame Tetris;

        

        public abstract int ChooseMove(Move[] moves);

        // Can be used to initialisation
        public virtual void Initialise() { }
    }
}