using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{   
    private int delay = 300;
    private AsyncOperation  asyncOperation;
    // Start is called before the first frame update
    void Start(){
        this.delay = Globle.loadingDelay;
        StartCoroutine("LoadScene");
    }

    IEnumerator LoadScene(){
        asyncOperation = SceneManager.LoadSceneAsync("Game");
        asyncOperation.allowSceneActivation = false;
        yield return asyncOperation;
    }

    // Update is called once per frame
    void Update(){
        if(this.asyncOperation == null){
            return;
        }
        if (asyncOperation.progress >= 0.89f){
            this.delay -= 1;
            if(this.delay <= 0){
                asyncOperation.allowSceneActivation = true;
            }
        }   
    }
}
