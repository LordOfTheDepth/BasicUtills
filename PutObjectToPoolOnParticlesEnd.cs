using UnityEngine;

public class PutObjectToPoolOnParticlesEnd : MonoBehaviour
{
    private void Awake()
    {
        var system = GetComponent<ParticleSystem>().main;
        system.stopAction = ParticleSystemStopAction.Callback;
    }
    private void OnParticleSystemStopped()
    {
        Pooler.Put(gameObject);
    }
}
