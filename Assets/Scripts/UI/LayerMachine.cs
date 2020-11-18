using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LayerMachine : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;

    [SerializeField]
    private RectTransform _pivot;
    [SerializeField]
    private Image _gravityLayer;
    [SerializeField]
    private Image _iconOff;
    [SerializeField]
    private Image _iconOnGravity;

    private Quaternion _targetRotation;
    private Image _curLayer;
    private Image _curIconOn;

    private bool _layerUp = false;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Instance.onLayerUp += LayerOn;
        GameEvents.Instance.onLayerDown += LayerOff;
        GameEvents.Instance.onLayerSwitch += LayerSwitch;
        SceneManager.sceneLoaded += OnSceneLoaded;

        _pivot = GameObject.Find("Pivot").GetComponent<RectTransform>();
        _iconOff = GameObject.Find("IconOffGravity").GetComponent<Image>();
        _iconOnGravity = GameObject.Find("IconOnGravity").GetComponent<Image>();

        _targetRotation = Quaternion.Euler(0, 0, -179f);
        _curLayer = _gravityLayer;
        _curLayer.enabled = false;
        _curIconOn = _iconOnGravity;
    }

    // Update is called once per frame
    void Update()
    {
        _pivot.rotation = Quaternion.Lerp(_pivot.rotation, _targetRotation, Time.deltaTime * _speed);
        if (_pivot.rotation == Quaternion.Euler(0, 0, 179f))
        {
            _curLayer.enabled = false;
        }
        else if (_layerUp && _pivot.rotation == Quaternion.Euler(0, 0, 0))
        {
            _layerUp = false;
            GameEvents.Instance.LayerSet();
        }
    }

    private void LayerOn()
    {
        SoundManager.Instance.PlaySFX("PageFlip");
        SoundManager.Instance.PlaySFX("Gear");
        if (_iconOff)
        {
            _iconOff.enabled = false;
        }
        else
        {
            _iconOff = GameObject.Find("IconOff").GetComponent<Image>();
            _iconOff.enabled = false;
        }
        if (_curIconOn)
        {
            _curIconOn.enabled = true;
        }
        else
        {
            _iconOnGravity = GameObject.Find("IconOnGravity").GetComponent<Image>();
            _curIconOn = _iconOnGravity;
            _curIconOn.enabled = true;
        }
        
        if (_pivot)
        {
            _pivot.rotation = Quaternion.Euler(0, 0, -179f);
        }
        else
        {
            _pivot = GameObject.Find("Pivot").GetComponent<RectTransform>();
            _pivot.rotation = Quaternion.Euler(0, 0, -179f);
        }
        _curLayer.enabled = true;
        _targetRotation = Quaternion.Euler(0, 0, 0);
        _layerUp = true;
    }

    private void LayerOff()
    {
        SoundManager.Instance.PlaySFX("PageFlip");
        SoundManager.Instance.PlaySFX("Gear");
        _targetRotation = Quaternion.Euler(0, 0, 179f);
        if (_curIconOn)
        {
            _curIconOn.enabled = false;
        }
        else
        {
            _iconOnGravity = GameObject.Find("IconOnGravity").GetComponent<Image>();
            _curIconOn = _iconOnGravity;
            _curIconOn.enabled = false;
        }
        if (_iconOff)
        {
            _iconOff.enabled = true;
        }
        else
        {
            _iconOff = GameObject.Find("IconOff").GetComponent<Image>();
            _iconOff.enabled = true;
        }
    }

    private void LayerSwitch()
    {
        // switch current layer and current icon on
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            _pivot = GameObject.Find("Pivot").GetComponent<RectTransform>();
            _iconOff = GameObject.Find("IconOffGravity").GetComponent<Image>();
            _iconOnGravity = GameObject.Find("IconOnGravity").GetComponent<Image>();
            if (GameManager.Instance.GetCurrentLevel() > 1)
            {
                _iconOff.enabled = true;
                _iconOff.color = new Color(_iconOff.color.r, _iconOff.color.g, _iconOff.color.b, 1);
            }
        }
    }
}
