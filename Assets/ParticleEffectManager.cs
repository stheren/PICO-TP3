using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectManager : MonoBehaviour
{
    public GameObject particleEmitterPrefab;
    private GameObject instance;

    // Start is called before the first frame update
    public void Activate()
    {
        instance = Instantiate(particleEmitterPrefab, transform.position, transform.rotation);
    }

    // Update is called once per frame
    public void Desactivate()
    {
        Destroy(instance);
    }
}
