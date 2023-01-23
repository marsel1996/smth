using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class CheckerPoint: Unit
    {
        [SerializeField] private bool _checkable;
        [SerializeField] private Square _square;

        public bool Checkable
        {
            get => _checkable;
            set => _checkable = value;
        }

        public bool CheckTop()
        {
            var place = Place.GetPlace();
            var points = place.StoppedSquares.Where(s => s != _square).Select(s => s.Checker.BottomPoint);

            return points.Any(x => MathHelpers.CrossPoints(x.transform, transform));
        }
        public bool CheckGround()
        {
            var place = Place.GetPlace();
            var currentY = transform.position.y;
            var points = place.StoppedSquares.Where(s => s != _square).Select(s => s.Checker.TopPoint).ToArray();

            var res = currentY <= place.BottomBorder
                      || points.Any(x => MathHelpers.CrossPoints(x.transform, transform));
            return res;
        }

        public bool CheckLeftBorder()
        {
            var place = Place.GetPlace();
            var currentX = transform.position.x;
            var points = place.StoppedSquares.Where(s => s != _square).Select(s => s.Checker.RightPoint);

            return currentX <= place.LeftBorder
                   || points.Any(x => MathHelpers.CrossPoints(x.transform, transform));
        }
        public bool CheckRightBorder()
        {
            var place = Place.GetPlace();
            var currentX = transform.position.x;
            var points = place.StoppedSquares.Where(s => s != _square).Select(s => s.Checker.LeftPoint);

            return currentX >= place.RightBorder
                   || points.Any(x => MathHelpers.CrossPoints(x.transform, transform));
        }
    }
}