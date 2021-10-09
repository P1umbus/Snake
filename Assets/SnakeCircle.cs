using System.Collections.Generic;
using UnityEngine;

public class SnakeCircle : MonoBehaviour
{
    public Vector2 PastPosition;
    public bool IsSnake;

    public void Move(Vector2 position)
    {
        PastPosition = transform.position;

        transform.position = position;
    }
}
