using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialByActivation : Tutorial
{

    [SerializeField]
    private Vector3 _offset;

    private void Update()
    {
        this.transform.position = _player.position + _offset;
    }

    //private void OnDestroy()
    //{
    //    Destroy(_icon.gameObject);
    //}
}
