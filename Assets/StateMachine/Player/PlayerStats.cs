using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    PlayerCore _core;
    public float MoveSpeed = 10;
    void Awake()
    {
        _core = GetComponent<PlayerCore>();
    }
}
