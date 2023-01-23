using UnityEngine;

namespace Assets.Scripts.Units
{
    public class Checker: Unit
    {
        [SerializeField] private CheckerPoint _topPoint;
        [SerializeField] private CheckerPoint _rightPoint;
        [SerializeField] private CheckerPoint _bottomPoint;
        [SerializeField] private CheckerPoint _leftPoint;

        public CheckerPoint[] Points => new[] {_topPoint, _rightPoint, _bottomPoint, _leftPoint};
        public CheckerPoint TopPoint
        {
            get => _topPoint;
            set => _topPoint = value;
        }
        public CheckerPoint RightPoint
        {
            get => _rightPoint;
            set => _rightPoint = value;
        }
        public CheckerPoint BottomPoint
        {
            get => _bottomPoint;
            set => _bottomPoint = value;
        }
        public CheckerPoint LeftPoint
        {
            get => _leftPoint;
            set => _leftPoint = value;
        }
        
        public void SetPointsByRotation()
        {
            var topPoint = TopPoint;
            var rightPoint = RightPoint;
            var bottomPoint = BottomPoint;
            var leftPoint = LeftPoint;

            TopPoint = rightPoint;
            RightPoint = bottomPoint;
            BottomPoint = leftPoint;
            LeftPoint = topPoint;
        }
    }
}
