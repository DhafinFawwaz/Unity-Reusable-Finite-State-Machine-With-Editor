using UnityEngine;

[System.Serializable]
public class HitRequest
{
    public float Damage;
    public float Knockback = 0;
    public Vector3 Direction = Vector3.zero;
    public float StunDuration = 0;
    public Element Element = Element.Normal;

    public HitRequest(){}
    // Everything
    public HitRequest(float damage, float knockback, Vector3 direction, Element element = Element.Normal, float stunDuration = 0.1f)
    {
        Damage = damage;
        Knockback = knockback;
        Direction = direction;
        Element = element;
        StunDuration = stunDuration;
    }
}
