using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager _instance = null;
    CanvasManager currentCanvas;
     
    public static GameManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }
    public float maxHealth = 10;

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
            SceneManager.LoadScene("GameOver");
            Debug.Log("You died, press Esc key to go to Title Screen");
        }
    }

    public void StartGame()
    {
        _health = 10;
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
