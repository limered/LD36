using UniRx;
using UniRx.Triggers;
using UnityEngine;

public static class ShowDebugTextMesh
{
    public static TextMesh AddHoverText(this GameObject gameObject, Color color, float topOffset = 1f)
    {
        var mainCam = GameObject.FindGameObjectWithTag("MainCamera");

        var hover = Object.Instantiate(EmptyPrefabCache);
        hover.transform.localRotation = Quaternion.identity;
        hover.transform.localScale = Vector3.one;
        hover.transform.position = gameObject.transform.position;

        var text = hover.AddComponent<TextMesh>();
        text.alignment = TextAlignment.Center;
        text.anchor = TextAnchor.MiddleCenter;
        text.color = color;
        
        gameObject.OnDestroyAsObservable().Subscribe(x => Object.Destroy(hover)).AddTo(gameObject);
        Observable.EveryLateUpdate()
            .Subscribe(x =>
            {
                hover.transform.position = gameObject.transform.position + new Vector3(0, topOffset, 0);
                if (mainCam)
                {
                    hover.transform.rotation = mainCam.transform.rotation;
                }
            })
            .AddTo(gameObject);

        return text;
    }

    private static GameObject emptyPrefabCache;
    private static GameObject EmptyPrefabCache
    {
        get
        {
            if (emptyPrefabCache == null)
                emptyPrefabCache = Resources.Load<GameObject>("Prefabs/Empty");

            return emptyPrefabCache;
        }
    }
}
