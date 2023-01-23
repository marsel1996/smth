using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public virtual void Remove()
    {
        Destroy(gameObject);
    }
}

public static class MathHelpers
{
    public static bool CrossPoints(Transform p1, Transform p2)
    {
        return FloatEquals(p1.position.x, p2.position.x)
               && FloatEquals(p1.position.y, p2.position.y);
    }
    public static bool FloatEquals(float a, float b)
    {
        return Mathf.Abs(a - b) < 0.01f;
    }

}