using UniRx;
using UnityEngine;

public class ControlHintsBinding : MonoBehaviour
{
    private MenuNavigation menuNavigation;

    void Start()
    {
        menuNavigation = IoC.Resolve<MenuNavigation>();

        Observable.EveryUpdate()
            .Where(x => KeyCode.R.IsPressed())
            .Take(1)
            .Subscribe(x => menuNavigation.StartGame())
            .AddTo(this);
    }
}
