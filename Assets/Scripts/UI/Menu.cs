using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Menu : MonoBehaviour
{

    [SerializeField]
    protected GameObject _mainMenuUI;
    [SerializeField]
    protected GameObject _settingsUI;

    [SerializeField]
    protected UnityEngine.UI.Button _topButtonMain;
    [SerializeField]
    protected Slider _topButtonSettings;
    [SerializeField]
    protected UnityEngine.UI.Button _topButtonInstruction;

    [SerializeField]
    private Slider _BGMSlider;
    [SerializeField]
    private Slider _SFXSlider;

    [SerializeField]
    protected SceneTransition _sceneTransition;

    protected UnityEngine.UI.Button _previousButton;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        _topButtonMain.Select();
        _previousButton = _topButtonMain;
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_settingsUI.activeSelf)
            {
                GoBack();
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (_mainMenuUI.activeSelf)
            {
                _topButtonMain.Select();
            }
            else if (_settingsUI.activeSelf)
            {
                _topButtonSettings.Select();
            }
        }
    }

    public void GoToSettings()
    {
        _mainMenuUI.SetActive(false);
        _settingsUI.SetActive(true);
        _BGMSlider.value = SoundManager.Instance.GetBGMVolume();
        _SFXSlider.value = SoundManager.Instance.GetSFXVolume();
        _previousButton = EventSystem.current.currentSelectedGameObject.GetComponent<UnityEngine.UI.Button>();
        EventSystem.current.SetSelectedGameObject(null);
        _topButtonSettings.Select();
    }

    protected virtual void GoBack()
    {
        _mainMenuUI.SetActive(true);
        _settingsUI.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        _previousButton.Select();
    }

    public void HoverOnButton()
    {
        SoundManager.Instance.PlaySFX("UIHover");
    }

    public void SelectButton()
    {
        //_previousButton = EventSystem.current.currentSelectedGameObject.GetComponent<UnityEngine.UI.Button>();
        //EventSystem.current.SetSelectedGameObject(null);
        //_previousButton.Select();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }

    public void SetBGMVolume(float volume)
    {
        SoundManager.Instance.SetBGMVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        SoundManager.Instance.SetSFXVolume(volume);
    }

}
