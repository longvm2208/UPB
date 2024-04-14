using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Particle Config")]
public class ParticleConfig : ScriptableObject
{
    public int resolution = 10;
    public float moveDuration = 1.2f;
    public FloatRange centerWeight = new FloatRange(0.4f, 0.5f);
    [Tooltip("Canvas Screen Space - Camera: 1 - 10\n" +
        "Canvas Screen Space - Overlay: 100 - 1000")]
    public FloatRange downWeight = new FloatRange(800f, 1000f);
    public AnimationCurve moveAnimationCurve;
}
