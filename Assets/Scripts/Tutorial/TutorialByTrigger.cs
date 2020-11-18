using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialByTrigger : Tutorial
{
    [SerializeField]
    private BoxCollider2D _collider;

    private bool _hasBeenTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && _icon != null && !_hasBeenTriggered)
        {
            StartCoroutine(ShowTutorial());
        }
    }

    public override void Destroy()
    {
        base.Destroy();
        if (_collider != null)
        {
            Destroy(_collider);
        }
    }

    IEnumerator ShowTutorial()
    {
        _hasBeenTriggered = true;
        yield return new WaitForSecondsRealtime(5f);
        if (_icon != null)
        {
            _icon.gameObject.SetActive(true);
            _icon.SetTransparency(1f);
        }
    }
}
