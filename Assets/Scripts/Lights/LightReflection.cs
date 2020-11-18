using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LightReflection : MonoBehaviour
{
    [SerializeField]
    private int _maxReflectionCount; // 5
    [SerializeField]
    private float _maxStepDistance; // 200
    [SerializeField]
    private LayerMask _whatIsSurface; // TODO: how to make a box both interactable and reflective?

    private LineRenderer _line;
    private LightTrigger _lightTrig;

    [SerializeField]
    private GameObject _endParticles;

    void Awake()
    {
        _line = GetComponent<LineRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        RedrawnLine();
    }

    private void OnDisable()
    {
        if (_lightTrig != null)
        {
            _lightTrig.RemoveOneLight(this);
        }
    }

    void RedrawnLine()
    {
        _line.positionCount = 1;
        _line.SetPosition(0, transform.position);

        // check if the origin is inside a box.
        if (Physics2D.OverlapCircle(transform.position, 0.01f, _whatIsSurface))
        {
            if (_lightTrig != null)
            {
                _lightTrig.RemoveOneLight(this);
            }
            _endParticles.SetActive(false);
            return;
        }
        DrawLineSeg(this.transform.position, this.transform.right, _maxReflectionCount);
    }

    void DrawLineSeg(Vector2 position, Vector2 direction, int reflectionsRemain)
    {
        if (reflectionsRemain == 0)
        {
            if (_lightTrig != null)
            {
                _lightTrig.RemoveOneLight(this);
            }
            MoveEndParticle(position, -direction);
            return;
        }

        position += direction.normalized * 0.01f;
        RaycastHit2D hit = Physics2D.Raycast(position, direction, _maxStepDistance, _whatIsSurface);
        if (hit.collider)
        {
            direction = Vector2.Reflect(direction, hit.normal);
            position = hit.point;
        }
        else
        {
            position += direction * _maxStepDistance;
        }

        _line.positionCount++;
        int lineSegIndex = _maxReflectionCount - reflectionsRemain + 1;
        _line.SetPosition(lineSegIndex, position);
        reflectionsRemain--;
        if (hit.collider && !hit.collider.tag.Contains("Reflective"))
        {
            reflectionsRemain = 0;
            MoveEndParticle(position, -Vector2.Reflect(direction, hit.normal));
            if (hit.collider.tag.Contains("LightTrigger"))
            {
                // open door;
                _lightTrig = hit.transform.GetComponent<LightTrigger>();
                if (_lightTrig == null)
                {
                    Debug.Log("missing lightTrig");
                }
                else
                {
                    _lightTrig.AddOneLight(this);
                }
            }
            else if(_lightTrig != null)
            {
                _lightTrig.RemoveOneLight(this);
            }
            return;
        }
        

        DrawLineSeg(position, direction, reflectionsRemain);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position+transform.right);
        DrawGizomsLine(transform.position, transform.right, _maxReflectionCount);
    }

    void DrawGizomsLine(Vector2 position, Vector2 direction, int reflectionsRemain)
    {
        if (reflectionsRemain == 0)
        {
            return;
        }

        Vector2 startingPos = position + direction.normalized * 0.01f;
        RaycastHit2D hit = Physics2D.Raycast(startingPos, direction, _maxStepDistance, _whatIsSurface);
        if (hit.collider)
        {
            direction = Vector2.Reflect(direction, hit.normal);
            position = hit.point;
        }
        else
        {
            position += direction * _maxStepDistance;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startingPos, position);
        reflectionsRemain--;

        DrawGizomsLine(position, direction, reflectionsRemain);
    }

    private void MoveEndParticle(Vector2 position, Vector2 direction)
    {
        _endParticles.SetActive(true);
        _endParticles.transform.position = position;
        _endParticles.transform.rotation = Quaternion.LookRotation(direction, Vector3.forward);
    }
}