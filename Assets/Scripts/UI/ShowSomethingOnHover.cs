using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class ShowSomethingOnHover : MonoBehaviour
{
    public GameObject showOnHover;

    void Start()
    {
        var button = GetComponent<Button>();
        button.OnPointerEnterAsObservable()
            .Select(x => true)
            .Merge(
                button.OnPointerExitAsObservable()
                .Select(x => false)
            )
            .Subscribe(showOnHover.SetActive).AddTo(this);
    }
}
