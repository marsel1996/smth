using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class RotatingFigure: Unit
    {
        private Figure _figure;

        void Start()
        {
            _figure = gameObject.GetComponent<Figure>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                var place = Place.GetPlace();
                var oldPosition = transform.position;

                transform.RotateAround(_figure.CenterPoint.position, new Vector3(0, 0, 1), 90);

                if (_figure.Squares.Any(s => s.Checker.LeftPoint.Checkable
                                             && s.Checker.LeftPoint.CheckLeftBorder())
                    || _figure.Squares.Any(s => s.Checker.RightPoint.Checkable
                                                && s.Checker.RightPoint.CheckRightBorder()))
                {
                    transform.RotateAround(_figure.CenterPoint.position, new Vector3(0, 0, 1), -90);
                    //var leftBorder = place.LeftBorder + (int)(_figure.GetWidth() / 2);
                    //oldPosition.Set((float)leftBorder, oldPosition.y, oldPosition.z);
                    //transform.position = oldPosition;
                }
                else
                {
                    _figure.SetPointsByRotation();
                }

                //if ()
                //{
                //    transform.RotateAround(_figure.CenterPoint.position, new Vector3(0, 0, 1), -90);
                //    //var rightBorder = place.RightBorder - (int)(_figure.GetWidth() / 2);
                //    //oldPosition.Set((float)rightBorder, oldPosition.y, oldPosition.z);
                //    //transform.position = oldPosition;
                //}
            }
        }
    }
}
