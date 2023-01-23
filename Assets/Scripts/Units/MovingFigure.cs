using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class MovingFigure: Unit
    {
        private Figure _figure;
        void Start()
        {
            _figure = GetComponent<Figure>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (IsValidMove(MoveDirection.Left))
                {
                    transform.position += new Vector3(-1, 0, 0);
                }
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (IsValidMove(MoveDirection.Right))
                {
                    transform.position += new Vector3(1, 0, 0);
                }
            }

            if (GetComponent<FallingFigure>() == null)
            {
                Destroy(this);
            }
        }

        private bool IsValidMove(MoveDirection direction = MoveDirection.Down)
        {
            var checkLeft = _figure.Squares
                .Any(square => square.Checker.LeftPoint.Checkable &&
                               square.Checker.LeftPoint.CheckLeftBorder());

            var checkRight = _figure.Squares
                .Any(square => square.Checker.RightPoint.Checkable &&
                               square.Checker.RightPoint.CheckRightBorder());

            return !(direction == MoveDirection.Left && checkLeft)
                   && !(direction == MoveDirection.Right && checkRight);
        }
    }
}
