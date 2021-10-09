using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    [SerializeField] private Transform _leftWall;
    [SerializeField] private Transform _rightWall;
    [SerializeField] private Transform _topWall;
    [SerializeField] private Transform _bottomWall;

    private void Start()
    {
        float aspect = Camera.main.aspect * 10;

        _leftWall.transform.position = new Vector2(-aspect, 0);
        _rightWall.transform.position = new Vector2(aspect, 0);


        Vector2 localScale = new Vector2(aspect * 2, 1);
        _topWall.transform.localScale = localScale;
        _bottomWall.transform.localScale = localScale;
    }
}
