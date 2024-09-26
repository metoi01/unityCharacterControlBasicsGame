using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static Action OnMouseClicked;
    public static Action<int> ArrowClicked;

    private int _arrow;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseClicked?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _arrow = -1; 
            ArrowClicked?.Invoke(_arrow);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _arrow = 1;
            ArrowClicked?.Invoke(_arrow);
        }
    }
}
