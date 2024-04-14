using System;
using System.Collections;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public event Action OnFirstParticleFinish;
    public event Action OnLastParticleFinish;
    public event Action OnParticleFinish;

    [SerializeField]
    private Transform target;
    [SerializeField] [Space]
    private int amount = 5;
    [SerializeField]
    private float interval = 0.2f;
    [SerializeField]
    private Transform myTransform;
    [SerializeField]
    private RewardParticle prefab;
    [SerializeField, ExposedScriptableObject]
    private ParticleConfig config;

    public int Amount => amount;
    public float Interval => interval;

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        RewardParticle particle;
        WaitForSeconds wait = new WaitForSeconds(interval);

        for (int i = 0; i < amount; i++)
        {
            particle = Instantiate(prefab, myTransform);
            particle.OnReachTarget += OnParticleFinish;

            if (i == 0)
            {
                particle.OnReachTarget += OnFirstParticleFinish;
            }
            else if (i == amount - 1)
            {
                particle.OnReachTarget += OnLastParticleFinish;
            }

            particle.Move(target.position);
            yield return wait;
        }
    }

    public void ClearSubscribers()
    {
        OnFirstParticleFinish = null;
        OnLastParticleFinish = null;
        OnParticleFinish = null;
    }
}
