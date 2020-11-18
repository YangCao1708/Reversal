using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour
{
    [SerializeField]
    private Volume _volume;

    private ChromaticAberration _chrom;
    [SerializeField]
    private float _maxChrom;
    private float _targetChrom;


    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Instance.onLayerUp += IncreaseChromaticAberration;
        GameEvents.Instance.onLayerDown += DecreaseChromaticAberration;
        _volume.profile.TryGet<ChromaticAberration>(out _chrom);
        _targetChrom = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _chrom.intensity.value = Mathf.Lerp(_chrom.intensity.value, _targetChrom, 0.125f);
    }

    private void IncreaseChromaticAberration()
    {
        _targetChrom = _maxChrom;
    }

    private void DecreaseChromaticAberration()
    {
        _targetChrom = 0f;
    }
}
