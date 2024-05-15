using Sirenix.OdinInspector;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Transform worldTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Camera canvasCamera;

    private void Update()
    {
        rectTransform.anchoredPosition = VectorUtils.WorldPositionToAnchorPosition(worldTransform.position, rectTransform.parent as RectTransform, canvas, canvasCamera);
    }
}
