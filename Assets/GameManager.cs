using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private SnakeCircle _snakeCirclePref;
    [SerializeField] private Snake _snake;


    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _highScoreText;


    [SerializeField] private Color[] _circlesColors;

    [SerializeField] private GameObject _losePanel;
    [SerializeField] private Text _loseScoreText;
    [SerializeField] private Text _loseHighScoreText;

    private int _maxX;
    private int _maxY;

    private int _highScore;
    private int _score;

    private void Start()
    {
        Instance = this;

        SpawnCircle();

        if (PlayerPrefs.HasKey("Highscore"))
            _highScore = PlayerPrefs.GetInt("Highscore");

        _highScoreText.text = "Highscore: " + _highScore;

        _maxY = 9;
        _maxX = Mathf.RoundToInt(Camera.main.aspect * 10);

        if (_maxX < Camera.main.aspect * 10)
            _maxX -= 1;
        else
            _maxX -= 2;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetScore()
    {
        if (_snake.SnakeCircles != null)
            _score = _snake.SnakeCircles.Count;
        _scoreText.text = "Score: " + _score;

        if (_score > _highScore)
        {
            _highScore = _score;
            PlayerPrefs.SetInt("Highscore", _highScore);
            _highScoreText.text = "Highscore: " + _highScore;
        }
    }

    public void Lose()
    {
        _losePanel.SetActive(true);
        _loseScoreText.text = _scoreText.text;
        _loseHighScoreText.text = _highScoreText.text;
    }

    public void SpawnCircle()
    {
        bool hz = true;

        Vector3 position = Vector3.zero;

        while (hz)
        {
            position = new Vector3(Random.Range(-_maxX, _maxX), Random.Range(-_maxY, _maxY), 0);
            hz = false;

            if (_snake.SnakeCircles != null)
            {
                foreach (SnakeCircle circle in _snake.SnakeCircles)
                {
                    if (circle.transform.position == position)
                    {
                        hz = true;
                        break;
                    }
                }
            }
        }

        SnakeCircle snakeCircle = Instantiate(_snakeCirclePref, position, Quaternion.identity);

        snakeCircle.IsSnake = false;

        SetColor(snakeCircle);
    }

    private void SetColor(SnakeCircle circle)
    {
        if (circle.TryGetComponent<SpriteRenderer>(out SpriteRenderer sprite))
        {
            sprite.color = _circlesColors[Random.Range(0, _circlesColors.Length)];
        }
        else
        {
            Debug.LogWarning($"Не смог получить доступ к компоненту SpriteRenderer объекта {circle.name}");
        }
    }    
}
