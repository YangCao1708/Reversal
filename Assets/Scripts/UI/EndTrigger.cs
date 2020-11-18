using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Playables;

public class EndTrigger : MonoBehaviour
{
    public PlayableDirector _endingTimeline;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _endingTimeline.Play();
            Destroy(this.gameObject);
        }
    }
}
