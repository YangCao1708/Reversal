using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AnimationTrigger : MonoBehaviour
{

    [SerializeField]
    private PlayableDirector _timeline;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.tag != "Grave")
        {
            _timeline.Play();
            Destroy(this.gameObject);
        }
    }

    public void TriggerAnimation()
    {
        _timeline.Play();
        Destroy(this);
    }
}
