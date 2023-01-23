using Assets.Scripts.Units;
using UnityEngine;

public class Square : Unit
{
    [SerializeField] private Checker _checker;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Figure _figure;

    public Checker Checker => _checker;
    public Figure Figure => _figure;
    public SpriteRenderer Sprite => _sprite;

    void Start()
    {
        _figure = GetComponentInParent<Figure>();
    }
    
    public void SetColor(Color color)
    {
        Sprite.color = color;
    }

    public override void Remove()
    {
        Figure.RemoveSquare(this);
        base.Remove();
    }
}