using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager _instance = null;

    public static GameManager instance
    {
        get { return instance; }
        set { _instance = value; }
    }
    //public int health = 10;

    int _health;
    public int health
    {
        get { return _health; }
        set
        {
            //currentCanvas = FindObjectOfType<CanvasManager>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
