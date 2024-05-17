using DG.Tweening;
using System;
using UnityEngine;

public class Particle : MonoBehaviour
{
    [SerializeField] private Transform myTransform;

    private PrefabInstancePool<Particle> pool;

    public event Action OnReachTarget;

    public Transform MyTransform => myTransform;

    private void OnValidate()
    {
        myTransform = GetComponent<Transform>();
    }

    public Particle Spawn(Transform parent)
    {
        var instance = pool.GetInstance(this);
        instance.pool = pool;
        instance.myTransform.SetParent(parent);
        return instance;
    }

    public void Despawn()
    {
        pool.Recycle(this);
    }

    public void Move(ParticleConfig config, Vector3[] path)
    {
        myTransform.position = path[0];
        myTransform.DOPath(path, config.moveDuration, PathType.CatmullRom)
        .SetEase(config.moveAnimationCurve).OnComplete(() =>
        {
            OnReachTarget?.Invoke();
            OnReachTarget = null;
            Despawn();
        });
    }
}
