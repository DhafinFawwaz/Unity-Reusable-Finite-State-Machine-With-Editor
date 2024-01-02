using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerCore _core;
    void Awake()
    {
        _core = GetComponent<PlayerCore>();
    }

    void Update()
    {
        transform.Translate(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * 
            _core.Stats.MoveSpeed * Time.deltaTime
        );
    }
}
