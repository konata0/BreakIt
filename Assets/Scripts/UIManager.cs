using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour{

    public Animator startButton;
    public Animator exitButton;

    void Start(){
        Globle.readLevelData();
        Globle.levelIndex = 0;
        startButton.SetBool("hidden", false);
        exitButton.SetBool("hidden", false);
    }

    public void StartGame(){
        Globle.levelIndex = 0;
        SceneManager.LoadScene("Loading");
    }

    public void ExitGame(){
        Application.Quit();
    }

}
