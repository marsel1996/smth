using Assets.Scripts.Units;
using UnityEngine;

public class CheckerPointSignal : Unit
{
    private Animation _animation;
    [SerializeField] private CheckerPoint _point;
    private SpriteRenderer _sprite;
    private bool _animationPlayed = true;
    [SerializeField] private bool _enable = false;

    void Start()
    {
        _animation = GetComponent<Animation>();
        _sprite = GetComponent<SpriteRenderer>();
        ChangeState();
    }

    // Update is called once per frame
    void Update()
    {
        if (_enable)
        {
            ChangeState();
        }
    }

    void ChangeState()
    {
        if (_point.Checkable)
        {
            if (_animationPlayed) return;
            _sprite.enabled = true;
            _animationPlayed = true;
            _animation.Play();
        }
        else
        {
            if (!_animationPlayed) return;
            _sprite.enabled = false;
            _animationPlayed = false;
            _animation.Stop();
        }
    }
}
