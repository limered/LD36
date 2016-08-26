using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class FingerTracker : ObservableTriggerBase
{
    private Subject<Touch> newFinger;
    private readonly Subject<Touch>[] activeFingers = new Subject<Touch>[5];

    public FingerTracker()
    {

    }

    void Start()
    {

    }

    void Update()
    {
        var touches = Input.touches;

        var fingerCount = 0;
        for (int i = 0; i < activeFingers.Length; i++)
        {
            if (Input.touchCount > i && touches[i].fingerId < activeFingers.Length)
            {
                fingerCount++;
                if (touches[i].phase == TouchPhase.Began)
                {
                    if (newFinger != null)
                    {
                        newFinger.OnNext(touches[i]);
                    }
                }
                if (activeFingers[touches[i].fingerId] != null)
                {
                    activeFingers[touches[i].fingerId].OnNext(touches[i]);
                    if (touches[i].phase >= TouchPhase.Ended)
                    {
                        activeFingers[touches[i].fingerId].OnCompleted();
                        activeFingers[touches[i].fingerId].Dispose();
                        activeFingers[touches[i].fingerId] = null;
                    }
                }
            }
        }
    }

    public IObservable<Touch> OnNewTouch()
    {
        return newFinger ?? (newFinger = new Subject<Touch>());
    }

    public IObservable<Touch> OnTrackFinger(int fingerId)
    {
        if (fingerId < 0 || fingerId >= activeFingers.Length) throw new ArgumentOutOfRangeException("fingerId must be between 0 and " + (activeFingers.Length - 1));

        if (activeFingers[fingerId] == null)
        {
            activeFingers[fingerId] = new Subject<Touch>();
        }

        return activeFingers[fingerId];
    }

    protected override void RaiseOnCompletedOnDestroy()
    {
        if (newFinger != null)
        {
            newFinger.OnCompleted();
            //do i need to do this??
            //newFinger.Dispose();
            //newFinger = null;
        }
    }
}
