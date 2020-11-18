using UnityEngine;
using UnityEngine.EventSystems;


public class PauseMenu : Menu
{
    [SerializeField]
    private GameObject _instructionUI;

    private bool _paused = false;
    private CharacterController _player;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _player = FindObjectOfType<CharacterController>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_paused)
            {
                if (_settingsUI.activeSelf || _instructionUI.activeSelf)
                {
                    GoBack();
                }
                else
                {
                    Resume();
                }
            }
            else
            {
                Pause();
            }
        }
        else if (Input.GetMouseButton(0) && _paused)
        {
            if (_mainMenuUI.activeSelf)
            {
                _topButtonMain.Select();
            }
            else if (_settingsUI.activeSelf)
            {
                _topButtonSettings.Select();
            }
            else if (_instructionUI.activeSelf)
            {
                _topButtonInstruction.Select();
            }
        }
    }

    public void Resume()
    {
        _mainMenuUI.SetActive(false);
        _settingsUI.SetActive(false);
        Time.timeScale = 1f;
        _paused = false;
        _player.enabled = true;
        AbilityManager.Instance.enabled = true;
        SoundManager.Instance.Resume();
    }

    void Pause()
    {
        _mainMenuUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        _topButtonMain.Select();
        Time.timeScale = 0f;
        _paused = true;
        _player.enabled = false;
        AbilityManager.Instance.enabled = false;
        SoundManager.Instance.Pause();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        GameManager.Instance.SaveLevel();
        _sceneTransition.TransitionToGame();
    }

    public void MainMenu()
    {
        Resume();
        _sceneTransition.TransitionToMain();
    }

    private void GoToInstructions()
    {
        _instructionUI.SetActive(true);
        _previousButton = EventSystem.current.currentSelectedGameObject.GetComponent<UnityEngine.UI.Button>();
        EventSystem.current.SetSelectedGameObject(null);
        _topButtonInstruction.Select();
    }

    protected virtual void GoBack()
    {
        base.GoBack();
        _instructionUI.SetActive(false);
    }
}
