using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    private List<Respawn> _respawns;
    private Respawn _curRespawn;
    [SerializeField]
    private int _level = 0;
    private int _levelInGame = 1;
    private Vector3 _respawnPosition;

    private float _timeStarted = 0;
    private bool _isTiming = false;

    private bool _hasStarted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.sceneLoaded += OnSceneLoaded;
        _respawns = new List<Respawn>();
        StartGame(_level);
    }

    public void NewRespawnPosition(Respawn r)
    {
        if (_level == 4 && r.CurrentLevel() == 4)
        {
            return;
        }

        int previousLevel = _level;

        _level = r.CurrentLevel();
        
        _curRespawn = _respawns[_level - 1];
        _respawnPosition = _curRespawn.transform.position;

        GameEvents.Instance.NewLevel();

        if (previousLevel == 0)
        {
            _isTiming = true;
            _timeStarted = Time.time;
        }
        else
        {
            if (previousLevel < _level)
            {
                if (_isTiming)
                {
                    float timeDiff = Time.time - _timeStarted;
                    MetricManagerScript.instance.LogString("Time spent solving level " + previousLevel.ToString(), timeDiff.ToString());
                }
                _timeStarted = Time.time;
                _isTiming = true;
            }
            else if (previousLevel > _level)
            {
                _isTiming = false;
            }

            SoundManager.Instance.Restart();
        }
    }

    public void EnterLevel4()
    {
        int previousLevel = _level;

        _level = 4;
        _curRespawn = _respawns[_level - 2];
        _respawnPosition = _curRespawn.transform.position;
        GameEvents.Instance.NewLevel();

        if (_isTiming)
        {
            float timeDiff = Time.time - _timeStarted;
            MetricManagerScript.instance.LogString("Time spent solving level " + previousLevel.ToString(), timeDiff.ToString());
        }
        _isTiming = false;
        SoundManager.Instance.Silence();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame(int level)
    {
        if (level == 0)
        {
            _level = 0;
            SceneManager.LoadScene(0);
            return;
        }
        else
        {
            HasStarted();
        }
        _level = level;

        if (Physics2D.gravity.y > 0)
        {
            Physics2D.gravity = new Vector2(0, -9.81f);
        }

        SceneManager.LoadScene(1);
    }

    public Vector3 GetRespawnPosition()
    {
        return _respawnPosition;
    }

    public int GetCurrentLevel()
    {
        return _level;
    }

    public void SaveLevel()
    {
        if (_level == 4)
        {
            _levelInGame = 3;
        }
        else
        {
            _levelInGame = _level;
        }
    }

    public void ResetLevel()
    {
        _levelInGame = 1;
    }

    public int GetLevelInGame()
    {
        return _levelInGame;
    }

    public void GetBackToMenu()
    {
        _level = 0;
    }

    public void HasStarted()
    {
        _hasStarted = true;
    }

    public bool CanContinue()
    {
        return _hasStarted;
    }

    private void PlayIntro()
    {
        PlayableDirector intro = GameObject.Find("IntroTimeline").GetComponent<PlayableDirector>();
        intro.Play();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "zza3")
        {
            // starts timing
            _timeStarted = Time.time;
            _isTiming = true;

            _respawns.Clear();
            int searchRespawnIndex = 1;
            GameObject go = GameObject.Find("/Respawns/RespawnLvl" + searchRespawnIndex.ToString());
            while (go != null)
            {
                _respawns.Add(go.GetComponent<Respawn>());
                searchRespawnIndex++;
                go = GameObject.Find("/Respawns/RespawnLvl" + searchRespawnIndex.ToString());
            }

            // sets current level and player respawn position
            _curRespawn = _respawns[_level - 1];
            _respawnPosition = _curRespawn.transform.position;

            if (_level == 1)
            {
                PlayIntro();
            }

        }
    }
}
