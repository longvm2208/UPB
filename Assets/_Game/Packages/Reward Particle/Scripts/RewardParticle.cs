using DG.Tweening;
using System;
using UnityEngine;

public class RewardParticle : MonoBehaviour
{
    public event Action OnReachTarget;

    [SerializeField]
    private Transform myTransform;
    [SerializeField, ExposedScriptableObject]
    private ParticleConfig config;

    public void Move(Vector3 end)
    {
        float centerWeight = config.centerWeight.GetRandom();
        float downWeight = config.downWeight.GetRandom();
        Vector3[] path = PathUtils.SankCurveXY(myTransform.position, end, config.resolution, downWeight, centerWeight);
        myTransform.DOPath(path, config.moveDuration, PathType.CatmullRom)
        .SetEase(config.moveAnimationCurve).OnComplete(() =>
        {
            OnReachTarget?.Invoke();
            OnReachTarget = null;
            Destroy(gameObject);
        });
    }
}
