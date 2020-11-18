using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [SerializeField]
    private UnityEngine.UI.Button _start;
    [SerializeField]
    private UnityEngine.UI.Button _continue;
    [SerializeField]
    private UnityEngine.UI.Button _settings;

    [SerializeField]
    private Image _continueImage;
    [SerializeField]
    private Sprite _disabled;
    [SerializeField]
    private Sprite _enabled;

    private Navigation _disabledStartNav = new Navigation();
    private Navigation _enabledStartNav = new Navigation();
    private Navigation _disabledSettingsNav = new Navigation();
    private Navigation _enabledSettingsNav = new Navigation();

    protected override void Start()
    {
        base.Start();
        SetUpNavigations();

        if (!GameManager.Instance.CanContinue())
        {
            _continueImage.sprite = _disabled;
            _start.navigation = _disabledStartNav;
            _settings.navigation = _disabledSettingsNav;
        }
        else
        {
            _continueImage.sprite = _enabled;
            _start.navigation = _enabledStartNav;
            _settings.navigation = _enabledSettingsNav;
        }
    }

    private void SetUpNavigations()
    {
        _disabledStartNav.mode = Navigation.Mode.Explicit;
        _disabledStartNav.selectOnUp = _start.navigation.selectOnUp;
        _disabledStartNav.selectOnDown = _settings;

        _enabledStartNav.mode = Navigation.Mode.Explicit;
        _enabledStartNav.selectOnUp = _start.navigation.selectOnUp;
        _enabledStartNav.selectOnDown = _continue;

        _disabledSettingsNav.mode = Navigation.Mode.Explicit;
        _disabledSettingsNav.selectOnUp = _start;
        _disabledSettingsNav.selectOnDown = _settings.navigation.selectOnDown;

        _enabledSettingsNav.mode = Navigation.Mode.Explicit;
        _enabledSettingsNav.selectOnUp = _continue;
        _enabledSettingsNav.selectOnDown = _settings.navigation.selectOnDown;
    }

    public void StartGame()
    {
        GameManager.Instance.HasStarted();
        GameManager.Instance.ResetLevel();
        ContinueGame();
    }

    public void ContinueGame()
    {
        //if (GameManager.Instance.GetLevelInGame() == 0)
        //{
        //    GameManager.Instance.ResetLevel();
        //}
        _sceneTransition.TransitionToGame();
    }

    public void Credits()
    {
        _sceneTransition.TransitionToCredits();
    }
}
