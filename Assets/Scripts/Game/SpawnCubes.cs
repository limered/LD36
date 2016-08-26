using UnityEngine;
using System;
using UniRx;
using System.Collections.Generic;

public class SpawnCubes : MonoBehaviour
{
    public float spawnInterval = 1f;
    public float extend = 1f;
    private float initialExtend;
    public int maxCubes = 100;
    private Vector3 currentPos;
    public GameObject cubePrefab;
    private readonly List<GameObject> cubes = new List<GameObject>();

    [SerializeField]
    private Position nextPos;

    enum Position { Left, Top, Right, Bottom }

    void Start()
    {
        if (cubePrefab == null) throw new Exception("no cube prefab assigned");

        initialExtend = extend;
        currentPos = transform.position;

        Observable.Interval(TimeSpan.FromSeconds(spawnInterval))
            .Subscribe(t => SpawnNextCube())
            .AddTo(this);
    }

    private void SpawnNextCube()
    {
        extend = Mathf.Repeat(extend - 0.1f, initialExtend);

        switch (nextPos)
        {
            case Position.Left:
                nextPos = Position.Right;
                transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                transform.position = currentPos + new Vector3(-extend, transform.position.y, 0f); break;
            case Position.Top:
                nextPos = Position.Bottom;
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                transform.position = currentPos + new Vector3(0f, transform.position.y, extend); break;
            case Position.Right:
                nextPos = Position.Top;
                transform.rotation = Quaternion.Euler(0f, -90f, 0f);
                transform.position = currentPos + new Vector3(extend, transform.position.y, 0f); break;
            case Position.Bottom:
                nextPos = Position.Left;
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                transform.position = currentPos + new Vector3(0f, transform.position.y, -extend); break;
        }

        cubes.Add(Instantiate(cubePrefab, transform.position, transform.rotation) as GameObject);

        while (cubes.Count > maxCubes)
        {
            var cube = cubes[0];
            cubes.Remove(cube);
            Destroy(cube);
        }
    }
}
