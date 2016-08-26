using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBinding : MonoBehaviour
{
    public Button StartButton;
    public Button SettingsButton;
    private MenuNavigation menuNavigation;


    void Start()
    {
        menuNavigation = IoC.Resolve<MenuNavigation>();

        StartButton.OnClickAsObservable().Subscribe(x => StartGame()).AddTo(this);
        SettingsButton.OnClickAsObservable().Subscribe(x => ShowSettings()).AddTo(this);
    }

    private void StartGame()
    {
        menuNavigation.StartGame();
    }

    private void ShowSettings()
    {
        IoC.Resolve<MenuNavigation>().NavigateToSettingsMenu();
    }
}
