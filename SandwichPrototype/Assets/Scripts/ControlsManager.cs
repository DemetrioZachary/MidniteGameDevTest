using UnityEngine;
using PrsdTech.SO.Events;

public class InputEventArgs : SOEventArgs
{
    public Vector3 position;
}

public class ControlsManager : MonoBehaviour
{
    [SerializeField] SOEvent inputBeganEvent;
    [SerializeField] SOEvent inputDragEvent;
    [SerializeField] SOEvent inputEndEvent;

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
        {
            Vector3 pos = Input.mousePosition;
            if (Input.GetMouseButtonDown(0))
#else
        if (Input.touches.Length > 0)
        {
            var touch = Input.GetTouch(0);
            Vector3 pos = touch.position;
            if (touch.phase == TouchPhase.Began)
#endif
            {
                inputBeganEvent.Invoke(new InputEventArgs { position = pos });
            }
#if UNITY_EDITOR
            else if (Input.GetMouseButtonUp(0))
#else
            else if (touch.phase == TouchPhase.Ended)
#endif
            {
                inputEndEvent.Invoke(new InputEventArgs { position = pos });
            }
            else
            {
                inputDragEvent.Invoke(new InputEventArgs { position = pos });
            }
        }
    }
}
