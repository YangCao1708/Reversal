using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tutorial : MonoBehaviour
{

    [SerializeField]
    protected IconFloat _icon;
    protected Transform _player;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _player = FindObjectOfType<CharacterController>().transform;
    }

    public virtual void Destroy()
    {
        if (_icon != null)
        {
            Destroy(_icon.gameObject);
        }
    }
}
