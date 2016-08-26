using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Cube : MonoBehaviour
{
    private static float campos;
    private static Vector3 startPos, endPos;
    private static IDisposable dis;
    private static float timeLeft;
    private const float moveTime = 3f;

	void Start ()
	{
	    this.OnCollisionEnterAsObservable().Take(1).Subscribe(x => AligCamera()).AddTo(this);
	}

    private void AligCamera()
    {
        Debug.Log("aligning camera");
        if (dis != null)
        {
            dis.Dispose();
            dis = null;
        }

        //move camera
        var camera = GameObject.FindGameObjectWithTag("MainCamera");
        startPos = camera.transform.position;
        endPos = new Vector3(camera.transform.position.x, transform.position.y+5f, camera.transform.position.z);
        timeLeft = moveTime;
        dis = Observable.EveryFixedUpdate().Subscribe(x=>MoveCamera(camera));

        //move spawner
        var spawner = GameObject.FindGameObjectWithTag("CubeSpawner");
        spawner.transform.position = new Vector3(spawner.transform.position.x, endPos.y+5f, spawner.transform.position.z);
    }

    private void MoveCamera(GameObject cam)
    {
        timeLeft = Mathf.Clamp(timeLeft - Time.fixedDeltaTime, 0f, float.PositiveInfinity);
        cam.transform.position = Vector3.Lerp(startPos, endPos, (moveTime-timeLeft)/ moveTime);

        if (timeLeft<=0f)
        {
            dis.Dispose();
            dis = null;
        }
    }
}
