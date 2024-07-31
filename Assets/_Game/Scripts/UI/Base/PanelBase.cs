using UnityEngine;

public abstract class PanelBase : MonoBehaviour
{
    [SerializeField] protected Canvas canvas;

    public void EnableCanvas(bool enabled)
    {
        if (canvas == null) return;

        canvas.enabled = enabled;
    }
}
