using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Particle Config")]
public class ParticleConfig : ScriptableObject
{
    public bool isRandom = true;
    public int amount = 5;
    public int resolution = 10;
    public float interval = 0.2f;
    public float moveDuration = 1.2f;
    public Vector2 random = new Vector2(100f, 100f);
    public Particle particlePrefab;
    public AnimationCurve moveAnimationCurve;

    public Vector3[] GetPath(Vector3 start, Vector3 control, Vector3 end)
    {
        if (isRandom)
        {
            float x = Random.Range(-random.x, random.x);
            float y = Random.Range(-random.y, random.y);
            control += new Vector3(x, y);
        }

        return PathUtils.QuadraticBezier(resolution, start, control, end);
    }

    public Vector3[] GetBasePath(Vector3 start, Vector3 control, Vector3 end)
    {
        return PathUtils.QuadraticBezier(resolution, start, control, end);
    }
}
