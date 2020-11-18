using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

    [SerializeField]
    private int _level;
    [SerializeField]
    private Transform _camPos;
    [SerializeField]
    private float _speed;

    private BoxCollider2D _managerBox;
    private bool _inside; // if player is inside
    private Transform _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _managerBox = GetComponent<BoxCollider2D>();
        if (GameManager.Instance.GetCurrentLevel() == _level)
        {
            MoveCameraFast();
        }
    }

    // Update is called once per frame
    void Update()
    {
        ManageBoundary();
        MoveCamera();
    }

    void ManageBoundary()
    {
        if (_managerBox.bounds.min.x < _player.position.x && _player.position.x < _managerBox.bounds.max.x &&
            _managerBox.bounds.min.y < _player.position.y && _player.position.y < _managerBox.bounds.max.y)
        {
            if (!_inside)
            {
                GameManager.Instance.NewRespawnPosition(this);
            }
            _inside = true;
        }
        else
        {
            _inside = false;
        }
    }

    void MoveCamera()
    {
        if (_inside)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, _camPos.position, Time.deltaTime * _speed);
        }
    }

    void MoveCameraFast()
    {
        Camera.main.transform.position = _camPos.position;
    }

    public int CurrentLevel()
    {
        return _level;
    }

}
