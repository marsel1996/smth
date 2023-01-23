using UnityEngine;

namespace Assets.Scripts.Units
{
    public class Spawner: Unit
    {
        [SerializeField]
        private GameObject[] _figures;
        public Figure Spawn()
        {
            var index = Random.Range(0, _figures.Length);
            if (index == _figures.Length) index = _figures.Length - 1;

            var startPosition = transform.position;

            var figure = _figures[index];
            if (figure.name == "Column" || figure.name == "Kube")
            {
                startPosition.Set(startPosition.x + 0.5f, startPosition.y + 0.5f, startPosition.z);
            }
            
            var unit = Instantiate(figure, startPosition, Quaternion.identity);
            unit.AddComponent<RotatingFigure>();
            unit.AddComponent<FallingFigure>();
            unit.AddComponent<MovingFigure>();

            var figureObj = figure.GetComponent<Figure>();
            figureObj.RandomColor();

            return figureObj;
        }
    }
}
