using UnityEngine;

public class HitResult
{
    public bool Defeat = false;
    public Collider2D Collider = null;
    public HitType Type = HitType.None;
    public Vector2 Position = Vector2.zero;


    public HitResult(){}
    public HitResult(bool defeat = false, Collider2D collider = null, HitType type = HitType.None, Vector2 position = default(Vector2))
    {
        Defeat = defeat;
        Collider = collider;
        Type = type;
        Position = position;
    }
}