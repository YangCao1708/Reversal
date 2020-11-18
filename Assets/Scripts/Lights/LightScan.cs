using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LightScan : MonoBehaviour
{
    [SerializeField]
    private float[] _zRotations;

    [SerializeField]
    private float _timeGap;

    [SerializeField]
    private float _rotSpeed;

    private int _targetIndex;
    private float _targetZRot;

    private GameObject _lightBeam;

    private void Start()
    {
        _targetIndex = 1;
        _targetZRot = _zRotations[_targetIndex];
        _lightBeam = transform.Find("LightOrigin").gameObject;
        _lightBeam.SetActive(false);
        StartCoroutine(Rotate());
    }

    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, _targetZRot)), Time.deltaTime * _rotSpeed);
    }

    IEnumerator Rotate()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(_timeGap);
            _targetIndex = (_targetIndex + 1) % _zRotations.Length;
            _targetZRot = _zRotations[_targetIndex];
        }
    }

    public void TurnOn()
    {
        _lightBeam.SetActive(true);
    }

    public void TurnOff()
    {
        _lightBeam.SetActive(false);
    }
}
