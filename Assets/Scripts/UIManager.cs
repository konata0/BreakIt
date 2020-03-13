using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour{

    void Start(){
        Globle.readLevelData();
        Globle.levelIndex = 0;
    }

    public void StartGame(){
        SceneManager.LoadScene("Game");
    }

    public void ExitGame(){
        Application.Quit();
    }

}
