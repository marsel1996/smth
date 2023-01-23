using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Units
{
    public static class CustomArray<T>
    {
        public static T[] GetColumn(T[,] matrix, int columnNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(0))
                .Select(x => matrix[x, columnNumber])
                .ToArray();
        }

        public static T[] GetRow(T[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                .Select(x => matrix[rowNumber, x])
                .ToArray();
        }
    }
    public class Place : Unit
    {
        private static Place _instance = null;
        public static Place GetPlace()
        {
            if (_instance == null) FindObjectOfType<Place>();
            return _instance;
        }

        [SerializeField]
        private Spawner _spawner;
        [SerializeField] private Text _text;
        public int Height => (int)Math.Ceiling(transform.localScale.y);
        public int Width => (int)Math.Ceiling(transform.localScale.x);
        public int LeftBorder => Width / 2 * -1;
        public int RightBorder => Width / 2;
        public int BottomBorder => Height / 2 * -1;
        public List<Figure> StoppedFigures { get; set; }
        public List<Square> StoppedSquares => StoppedFigures.SelectMany(x => x.Squares).ToList();
        public Square[,] StoppedSquaresGrid { get; set; }
        public Square[] UpdateFigure { get; set; }

        void Start()
        {
            StoppedFigures = new List<Figure>();
            StoppedSquaresGrid = new Square[Height + 1, Width + 1];
            _instance = FindObjectOfType<Place>();
        }

        void FixedUpdate()
        {
            if (FindObjectOfType<FallingFigure>() == null)
            {
                _spawner?.Spawn();
            }
        }

        public void UpdatePlace(Figure figure)
        {
            StoppedFigures.Add(figure);
            UpdateStoppedFigureIntoGrid();
            CheckAndDeleteFullRows();
        }

        void UpdateStoppedFigureIntoGrid()
        {
            StoppedSquaresGrid = new Square[Height + 1, Width + 1];
            StoppedSquares.ForEach(s =>
            {
                var x = (int)MathF.Round(s.transform.position.x) - LeftBorder;
                var y = (int)MathF.Round(s.transform.position.y) - BottomBorder;

                StoppedSquaresGrid[y, x] = s;
            });
            //var st = "";
            //for (int i = 0; i < Height + 1; i++)
            //{
            //    var row = CustomArray<Square>.GetRow(StoppedSquaresGrid, i).ToList();
            //    for (int j = 0; j < row.Count; j++)
            //    {
            //        if (row[j] != null)
            //        {
            //            st += $"\t i = {i}, j = {j} name = {row[j].name}";
            //        }
            //    }
            //}

            //_text.text = st;
        }

        void CheckAndDeleteFullRows()//Figure figure)
        {
            var rows = Enumerable.Range(0, Height + 1)
                .Select(y =>
                {
                    var row = CustomArray<Square>.GetRow(StoppedSquaresGrid, y).ToList();
                    if (row.Any(x => x == null)) return null;

                    return row;
                })
                .Where(x => x != null)
                .ToList();

            if (rows.Count > 0)
            {
                var forUpdate = rows.SelectMany(x => x).ToList();
                var figuresForUpdate = forUpdate.Select(x => x.Figure).Distinct().ToList();

                var maxY = forUpdate.Max(x => x.Checker.TopPoint.transform.position.y);

                forUpdate.ForEach(x => x.Remove());
                var figuresForSplit = DeleteEmptyFigure(figuresForUpdate);

                var figuresNew = figuresForSplit.SelectMany(x => x.UpdateFigure());

                foreach (var figure in figuresNew)
                {
                    StoppedFigures.Add(figure);
                }

                StoppedFigures
                    .Where(x => x.Squares.Any(s => s.Checker.TopPoint.transform.position.y > maxY))
                    .ToList()
                    .ForEach(x => x.transform.position += new Vector3(0, -rows.Count, 0));

                StoppedFigures.ForEach(x => x.UpdateAllCheckers());

                //var updatedFigure = StoppedFigures.Where(x => x.UpdateFalling()).ToList();
                //StoppedFigures = StoppedFigures.ToList();
                //updatedFigure.ForEach(x => x.AddComponent<FallingFigure>());
            }
        }

        public void AddStoppedFigure(Figure figure)
        {
            StoppedFigures.Add(figure);
        }

        private List<Figure> DeleteEmptyFigure(List<Figure> figuresForUpdate)
        {
            var emptyFigures = figuresForUpdate.Where(x => x.Squares.Count == 0).ToList();
            StoppedFigures = StoppedFigures.Where(x => !emptyFigures.Contains(x)).ToList();

            return figuresForUpdate.Where(x => !emptyFigures.Contains(x)).ToList();
        }
    }
}