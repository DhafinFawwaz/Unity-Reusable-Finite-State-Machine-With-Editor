using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDebugger : MonoBehaviour
{
    [SerializeField] Core _core;
    [SerializeField] Vector2 _offset = new Vector2(0, 50);
    Vector2 _dimension = new Vector2(100, 20); 
    Camera cam;
    [SerializeField] int _fontSize = 15;
    [SerializeField] Color _textColor = Color.red;
    void Awake()
    {
        cam = Camera.main;
        _core ??= GetComponent<Core>();

    }
    void OnGUI()
    {
        Vector2 labelPosition = cam.WorldToScreenPoint(_core.transform.position);
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = _fontSize;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = _textColor;
        
        GUI.Label(
            new Rect(
                labelPosition.x - _dimension.x/2 + _offset.x, 
                Display.main.systemHeight-labelPosition.y - _dimension.y/2 - _offset.y, 
                _dimension.x, 
                _dimension.y
            ),
            _core.GetCurrentState(),
            style
        );
    }
}
