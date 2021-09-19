using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager _instance = null;
    CanvasManager currentCanvas;
    public bool end;
    public static GameManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }
    public float _maxHealth = 10;
    public float maxHealth
    {
        get { return _maxHealth; }
        set
        {
            currentCanvas = FindObjectOfType<CanvasManager>();

            _maxHealth = value;
            currentCanvas.SetMaxHealthText();
        }
    }
    float _health;
    public float health
    {
        get { return _health; }
        set
        {
            currentCanvas = FindObjectOfType<CanvasManager>();
            if (_health <= 0)
            {
                SceneManager.LoadScene("GameOver");
            }
            _health = value;
            if (_health > maxHealth)
            {
                _health = maxHealth;
            }
            currentCanvas.SetHealthText();
        }
    }
    int _score = 0;
    public int score
    {
        get { return _score; }
        set
        {
            currentCanvas = FindObjectOfType<CanvasManager>();

            _score = value;
            currentCanvas.SetScoreText();

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
    public void GameOver()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            Cursor.lockState = CursorLockMode.None;

            SceneManager.LoadScene("GameOver");
            Debug.Log("You died, press Esc key to go to Title Screen");
        }
    }

    public void StartGame()
    {
        _health = 10;
        _score = 0;
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Cursor.lockState = CursorLockMode.None;

    }
    public void Win()
    {
        //if (end == true)
        {
            Cursor.lockState = CursorLockMode.None;

            SceneManager.LoadScene("Win");

        }
    }
}
