using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class FallingFigure : Unit
    {
        private float previousTime;
        public float failTime = 0.5f;
        private Figure _figure;

        void Start()
        {
            _figure = GetComponent<Figure>();
        }

        void Update()
        {
            var failTimeRange = Input.GetKey(KeyCode.DownArrow) ? failTime / 10 : failTime;
            if (Time.time - previousTime > failTimeRange)
            {
                if (IsValidMove())
                {
                    transform.position += new Vector3(0, -1, 0);
                    previousTime = Time.time;
                }
                else
                {
                    _figure.StopMoving();
                }
            }
        }
        public bool IsValidMove()
        {
            var isGrounded = _figure.Squares.Any(s => s.Checker.BottomPoint.Checkable && s.Checker.BottomPoint.CheckGround());

            return !isGrounded;
        }
    }
}