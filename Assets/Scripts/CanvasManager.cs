using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class CanvasManager : MonoBehaviour
{
    //float healthPercentage = GameManager.instance.health / 10;

    [Header("Buttons")]
    public Button startButton;
    public Button quitButton;
    public Button returnToMenuButton;
    public Button controls;
    public Image healthBarImage;
    public Button back;

    [Header("Text")]
    public Text healthText;
    public Text scoreText;
    public Text maxHealthText;
    public Text winText;
    public Text loseText;

    [Header("Menus")]
    public GameObject controlMenu;
    public GameObject mainMenu;


    // Start is called before the first frame update
    void Start()
    {
        if (startButton)
        {
            startButton.onClick.AddListener(() => GameManager.instance.StartGame());
        }
        //healthBarImage = GetComponent<Image>();
        if (quitButton)
        {
            quitButton.onClick.AddListener(() => GameManager.instance.QuitGame());
        }
        if (returnToMenuButton)
        {
            returnToMenuButton.onClick.AddListener(() => GameManager.instance.ReturnToMenu());
        }
        if (controlMenu)
        {
            controls.onClick.AddListener(() => ShowControlsMenu());
        }
        if (back)
        {
            back.onClick.AddListener(() => ShowMainMenu());
        }
        if (healthText)
        {
            SetHealthText();
        }
        if (scoreText)
        {
            SetScoreText();
        }
        if (maxHealthText)
        {
            SetMaxHealthText();
        }
    }
    public void SetHealthText()
    {
        if (GameManager.instance)
        {
            healthText.text = GameManager.instance.health.ToString();
        }
        else
        {
            SetHealthText();
        }
    }
    public void SetScoreText()
    {
        if (GameManager.instance)
        {
            scoreText.text = GameManager.instance.score.ToString();           
        }
        else
        {
            SetScoreText();
        }
    }
    public void SetMaxHealthText()
    {
        if (GameManager.instance)
        {
            maxHealthText.text = GameManager.instance.maxHealth.ToString();
        }
        else
        {
            SetMaxHealthText();
        }
    }
    // Update is called once per frame
    void Update()
    {
        //healthText.text = GameManager.instance.health.ToString();

    }
    void ShowControlsMenu()
    {
        controlMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    void ShowMainMenu()
    {
        controlMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
