using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    public List<SnakeCircle> SnakeCircles => _snakeCircles;

    [SerializeField] private float _timeBtwMove;
    [SerializeField] private SnakeCircle _snakeTailPref;

    private float _elapsedTime;

    private Vector2 _direction = Vector2.up;

    private List<SnakeCircle> _snakeCircles;

    private Vector2 _pastPosition;

    private bool _bugFix;

    private bool _isAlive = true;  

    private void Start()
    {
        _snakeCircles = new List<SnakeCircle>();
    }

    private void Update()
    {
        if (_isAlive == true)
        {
            _elapsedTime += Time.deltaTime;

            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                SetDirection();

            if (_elapsedTime >= _timeBtwMove)
            {
                _elapsedTime = 0;

                _pastPosition = transform.position;

                transform.Translate(_direction);

                TailMovement();

                _bugFix = true;
            }

            if (Input.GetMouseButtonDown(0))
                AddCircle();
        }
    }
    private void SetDirection()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (_bugFix == true)
        {
            if (horizontal > 0 && _direction.x == 0)
            {
                _direction = Vector2.right;
            }
            else if (horizontal < 0 && _direction.x == 0)
            {
                _direction = Vector2.left;
            }
            else if (vertical > 0 && _direction.y == 0)
            {
                _direction = Vector2.up;
            }
            else if (vertical < 0 && _direction.y == 0)
            {
                _direction = Vector2.down;
            }

            _bugFix = false;
        }
    }

    private void TailMovement()
    {
        for (int i = 0; i < _snakeCircles.Count; i++)
        {
            if (i == 0)
                _snakeCircles[i].Move(_pastPosition);
            else
                _snakeCircles[i].Move(_snakeCircles[i - 1].PastPosition);
        }
    }

    private void AddCircle() //
    {
        if (_pastPosition == null)
        {
            Debug.LogError("Не могу заспавнить circle без _pastPosition");
            return;
        }

        Vector2 position;

        if (_snakeCircles.Count == 0)
            position = _pastPosition;
        else
            position = _snakeCircles[_snakeCircles.Count - 1].PastPosition;

        SnakeCircle circle;

        if (transform.parent != null)
            circle = Instantiate(_snakeTailPref, position, Quaternion.identity, transform.parent.transform);
        else
            circle = Instantiate(_snakeTailPref, position, Quaternion.identity);

        _snakeCircles.Add(circle);

        circle.IsSnake = true;

        GameManager.Instance.SetScore();        
    }

    private void AddCircle(SnakeCircle circle) 
    {
        Vector2 position;

        if (_snakeCircles.Count == 0)
            position = _pastPosition;
        else
            position = _snakeCircles[_snakeCircles.Count - 1].PastPosition;

        circle.transform.position = position;

        _snakeCircles.Add(circle);

        if (transform.parent != null)
            circle.transform.SetParent(transform.parent);

        circle.IsSnake = true;

        GameManager.Instance.SetScore();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<SnakeCircle>(out SnakeCircle circle))
        {
            if (circle.IsSnake == false)
            {
                AddCircle(circle);

                GameManager.Instance.SpawnCircle();
            }
            else
            {
                Death();
            }
        }
        else
        {
            Death();
        }
    }

    private void Death()
    {
        _isAlive = false;

        GameManager.Instance.Lose();

        Debug.Log("You Death!");
    }
}