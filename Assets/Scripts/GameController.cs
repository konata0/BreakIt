using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour{
    public Sprite[] spriteList;
    // Start is called before the first frame update
    private List<List<int>> levelData = null;
    private int levelBackground = 0;
    private GameObject background;
    void Start(){
        Globle.readLevelData();
        // 获取关卡数据
        levelData = Globle.getLevelData();
        levelBackground = Mathf.Clamp(Globle.getLevelBackground(), 0, spriteList.Length - 1);
        // 设置背景
        background = GameObject.Find("Background");
        background.GetComponent<SpriteRenderer>().sprite = spriteList[levelBackground];


        
    }

    // Update is called once per frame
    void Update(){
        
    }
}
