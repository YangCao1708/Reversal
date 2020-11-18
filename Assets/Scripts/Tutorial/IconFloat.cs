using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconFloat : MonoBehaviour
{
    [SerializeField]
    private string _easeType;
    [SerializeField]
    private float _time;
    [SerializeField]
    private float _floatDis;

    private float _alpha;
    private float _targetAlpha;
    private SpriteRenderer _icon;

    void Awake()
    {
        iTween.MoveBy(gameObject, iTween.Hash("y", _floatDis, "Time", _time, "easeType", _easeType, "loopType", "pingPong"));
        _icon = this.GetComponent<SpriteRenderer>();
        _targetAlpha = 0;
        _alpha = _targetAlpha;
        _icon.color = new Color(1f, 1f, 1f, _alpha);
    }

    private void Update()
    {
        _alpha = Mathf.Lerp(_alpha, _targetAlpha, 0.2f);
        _icon.color = new Color(1f, 1f, 1f, _alpha);
    }

    public void SetTransparency(float a)
    {
        _targetAlpha = a;
    }

    public void SetTransparencyDelayed(float a)
    {
        StartCoroutine(Delay(a));
    }

    IEnumerator Delay(float a)
    {
        yield return new WaitForSecondsRealtime(1f);
        _targetAlpha = a;
    }
}
