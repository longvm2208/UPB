using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform controlPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField, ExposedScriptableObject]
    private ParticleConfig config;

    private Transform myTransform;

    private static WaitForSeconds wait;

    public event Action OnFirstParticleFinish;
    public event Action OnLastParticleFinish;
    public event Action OnParticleFinish;

    private void Awake()
    {
        myTransform = transform;
        wait = new WaitForSeconds(config.interval);
    }

    [Button]
    public void Spawn()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        for (int i = 0; i < config.amount; i++)
        {
            var particle = config.particlePrefab.Spawn(myTransform);
            particle.OnReachTarget += OnParticleFinish;

            if (i == 0)
            {
                particle.OnReachTarget += OnFirstParticleFinish;
            }
            else if (i == config.amount - 1)
            {
                particle.OnReachTarget += OnLastParticleFinish;
            }

            var path = config.GetPath(startPoint.position, controlPoint.position, endPoint.position);
            particle.Move(config, path);

            yield return wait;
        }
    }

    public void ClearSubscribers()
    {
        OnFirstParticleFinish = null;
        OnLastParticleFinish = null;
        OnParticleFinish = null;
    }

    private void OnDrawGizmos()
    {
        GizmosUtils.DrawPath(config.GetBasePath(startPoint.position, controlPoint.position, endPoint.position));
    }
}
