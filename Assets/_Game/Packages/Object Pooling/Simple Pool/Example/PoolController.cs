using UnityEngine;

public class PoolController : MonoBehaviour
{
    [SerializeField] private Transform memberParent;
    [SerializeField] private Transform particleParent;

    [SerializeField] private PoolMember memberPrefab;
    [SerializeField] private ParticleSystem particlePrefab;

    private void Awake()
    {
        SimplePool.Preload(memberPrefab, 10, memberParent);
        ParticlePool.Preload(particlePrefab, 10, particleParent);
    }
}
