using UnityEngine;
using UnityEngine.UI;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Vector2 speed = Vector2.one;

    private void Start()
    {
        image.material = new Material(image.material);
    }

    private void Update()
    {
        image.material.mainTextureOffset += speed * Time.deltaTime;
    }
}
