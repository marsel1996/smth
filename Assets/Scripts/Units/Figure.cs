using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Units;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Figure : Unit
{
    private Place _place;
    [SerializeField] private List<Square> _squares;
    [SerializeField] private Transform _centerPoint;
    public List<Square> Squares
    {
        get => _squares;
        set => _squares = value;
    }

    public Transform CenterPoint => _centerPoint;
    public Color Color { get; set; }
    public List<CheckerPoint> BottomCheckerPoints => Squares.Select(s => s.Checker.BottomPoint).ToList();
    public List<CheckerPoint> TopCheckerPoints => Squares.Select(s => s.Checker.TopPoint).ToList();
    public List<CheckerPoint> LeftCheckerPoints => Squares.Select(s => s.Checker.LeftPoint).ToList();
    public List<CheckerPoint> RightCheckerPoints => Squares.Select(s => s.Checker.RightPoint).ToList();

    public Figure(List<Square> squares)
    {
        _squares = squares;
    }
    void Start()
    {
        _place = FindObjectOfType<Place>();
    }
    public void StopMoving()
    {
        Destroy(GetComponent<FallingFigure>());
        Destroy(GetComponent<RotatingFigure>());
        UpdatePositionInfo(true);
        _place.UpdatePlace(this);
    }

    public void UpdatePositionInfo(bool afterStop = false)
    {
        if (!afterStop)
        {
            if (GetComponent<FallingFigure>() != null) return;
        }

        TopCheckerPoints.ForEach(x =>
        {
            if (x.Checkable && x.CheckTop())
                x.Checkable = false;
        });
        LeftCheckerPoints.ForEach(x =>
        {
            if (x.Checkable && x.CheckLeftBorder())
                x.Checkable = false;
        });
        RightCheckerPoints.ForEach(x =>
        {
            if (x.Checkable && x.CheckRightBorder())
                x.Checkable = false;
        });
        BottomCheckerPoints.ForEach(x =>
        {
            if (x.Checkable && x.CheckGround())
                x.Checkable = false;
        });
    }

    public void UpdateAllCheckers()
    {
        if (gameObject.name.Contains("Column") || gameObject.name.Contains("Часть"))
        {
            var a = 0;
        }
        TopCheckerPoints.ForEach(x => x.Checkable = !x.CheckTop());
        LeftCheckerPoints.ForEach(x => x.Checkable = !x.CheckLeftBorder());
        RightCheckerPoints.ForEach(x => x.Checkable = !x.CheckRightBorder());
        BottomCheckerPoints.ForEach(x => x.Checkable = !x.CheckGround()); 
    }

    public bool UpdateFalling()
    {
        if (gameObject.name.Contains("Часть"))
        {
            var a = 0;
        }
        if (!BottomCheckerPoints.Any(x => x.Checkable)) return false;

        var bottomChecked = BottomCheckerPoints.Any(x => x.Checkable && x.CheckGround());
        return !bottomChecked;
    }

    private bool AxisIsValid(double[] items)
    {
        if (items.Length == 0 || items.Length == 1) return true;
        for (int i = 1; i < items.Length; i++)
        {
            if (items[i - 1] + 1 == items[i])
            {

            }
            else
            {
                return false;
            }
        }

        return true;
    }

    //public void UpdateFigure()
    //{
    //    var axesX = Squares.Select(x => Math.Round(x.transform.position.x)).Distinct().ToList();

    //    axesX.ForEach(x =>
    //    {
    //        var squares = Squares.Where(s => Math.Round(s.transform.position.x) == x).ToList();
    //        if (squares.Count > 1)
    //        {
    //            var axesY = squares.Select(s => Math.Round(s.transform.position.y)).OrderBy(s => s).ToList();
    //            var currentY = axesY[0];

    //            if (AxisIsValid(axesY.ToArray())) return;
    //            for (var i = 1; i < axesY.Count; i++)
    //            {
    //                var y = axesY[i];
    //                var diff = 0;
    //                for (var pp = currentY; pp < y - 1; pp++)
    //                {
    //                    diff++;
    //                }

    //                if (diff != 0)
    //                {
    //                    var currentSquare = squares.Find(s => Math.Round(s.transform.position.y) == y);
    //                    var pos = currentSquare.transform.position;
    //                    pos.Set(pos.x, pos.y - diff, pos.z);
    //                    currentSquare.transform.position = pos;
    //                }

    //                currentY = y - diff;
    //            }
    //        }
    //    });
    //}

    public List<Figure> UpdateFigure()
    {
        var place = Place.GetPlace();

        var onlySquares = Squares
            .Where(square => !Squares.Any(squareSecond =>
                MathHelpers.CrossPoints(square.Checker.RightPoint.transform, squareSecond.Checker.LeftPoint.transform)
                || MathHelpers.CrossPoints(square.Checker.TopPoint.transform, squareSecond.Checker.BottomPoint.transform)
                || MathHelpers.CrossPoints(square.Checker.LeftPoint.transform, squareSecond.Checker.RightPoint.transform)
                || MathHelpers.CrossPoints(square.Checker.BottomPoint.transform, squareSecond.Checker.TopPoint.transform)))
            .ToList();

        Squares = Squares.Where(x => !onlySquares.Contains(x)).ToList();
        if (Squares.Count < 2)
        {
            place.StoppedFigures.Remove(this);
        }
        var result = new List<Figure>();
        foreach (var onlySquare in onlySquares)
        {
            var position = onlySquare.transform.position;

            var newGameObj = new GameObject($"Часть {onlySquare.Figure.name}");

            var newFigure = newGameObj.AddComponent<Figure>();
            var squareClone = Instantiate(onlySquare, Vector3.zero, Quaternion.identity, newFigure.transform);
            newFigure.Squares = new List<Square>() { squareClone };
            newGameObj.transform.position = position;
            newGameObj.transform.rotation = onlySquare.Figure.transform.rotation;
            onlySquare.Remove();

            //Instantiate(newGameObj, position, Quaternion.identity);
            result.Add(newFigure);
        }

        return result;
    }

    public void SetPointsByRotation()
    {
        Squares.ForEach(square => square.Checker.SetPointsByRotation());
    }
    public void RemoveSquare(Square square)
    {
        Squares.Remove(square);
    }
    public void RandomColor()
    {
        Color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        Squares.ForEach(x => x.SetColor(Color));
    }
}