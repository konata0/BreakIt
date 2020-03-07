using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour{
    public Sprite[] spriteList;
    public GameObject ball;
    // Start is called before the first frame update
    private List<List<int>> levelData = null;
    private int levelBackground = 0;
    private GameObject background;
    private List<GameObject> ballList;
    void Start(){
        Globle.readLevelData();
        // 获取关卡数据
        levelData = Globle.getLevelData();
        levelBackground = Mathf.Clamp(Globle.getLevelBackground(), 0, spriteList.Length - 1);
        // 设置背景
        background = GameObject.Find("Background");
        background.GetComponent<SpriteRenderer>().sprite = spriteList[levelBackground];

        // 创建弹球
        GameObject newBall = (GameObject)Instantiate(ball, new Vector3(0, 0, 0), Quaternion.identity);
        BallController newBallController = newBall.GetComponent<BallController>();
        newBallController.speed = new Vector3(0, -3.0f, 0);
        ballList = new List<GameObject>();
        ballList.Add(newBall);
        
    }

    // Update is called once per frame
    void Update(){
        
    }
}
