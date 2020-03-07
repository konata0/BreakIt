using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour{
    public Sprite[] backgroundList;
    public Sprite[] brickList;
    public GameObject ball;
    public GameObject brick;
    private float brickWidth;
    private float brickHeight;
    // Start is called before the first frame update
    private List<List<int>> levelData = null;
    private int levelBackground = 0;
    private GameObject background;
    private int ballNumber = 0;
    private int brickNumber = 0;
    void Start(){

        Globle.readLevelData();

        // 获取关卡数据
        this.levelData = Globle.getLevelData();
        this.levelBackground = (int)Mathf.Clamp(Globle.getLevelBackground(), 0, backgroundList.Length - 1);

        // 设置背景
        this.background = GameObject.Find("Background");
        this.background.GetComponent<SpriteRenderer>().sprite = backgroundList[levelBackground];

        // 创建弹球
        GameObject newBall = (GameObject)Instantiate(ball, new Vector3(0, 0, 0), Quaternion.identity);
        BallController newBallController = newBall.GetComponent<BallController>();
        newBallController.speed = new Vector3(0, -3.6f, 0);
        this.ballNumber = 1;

        // 创建砖块
        GameObject tempBrick = (GameObject)Instantiate(brick, new Vector3(0, 0, 0), Quaternion.identity);   
        Renderer r = tempBrick.GetComponent<Renderer>();
        this.brickHeight = r.bounds.extents.y * 2.0f;
        this.brickWidth = r.bounds.extents.x * 2.0f;
        Destroy(tempBrick);
        this.brickNumber = 0;
        for(int row = 0; row <= this.levelData.Count - 1; row++){
            for(int col = 0; col <= this.levelData[row].Count - 1; col++){
                int type = this.getBrickType(row, col);
                if(type >= 0){
                    this.createBrick(type, row, col);
                }
            }
        }
             
    }

    // Update is called once per frame
    void Update(){


    }

    // 获取砖块类型
    private int getBrickType(int row, int col){
        if(row >= levelData.Count || row < 0){
            return -1;
        }
        if(col >= levelData[row].Count || col < 0){
            return -1;
        }
        int re = levelData[row][col];
        re = (re < -1)? -1: re;
        re = (re > brickList.Length - 1)? brickList.Length - 1: re;
        return re;
    }

    // 生成砖块
    private void createBrick(int type, int row, int col){
        float x = (((float)col) - ((float)Globle.col - 1.0f) / 2.0f) * this.brickWidth + Globle.brickCenter.x;
        float y = (((float)Globle.row - 1.0f) / 2.0f - ((float)row)) * this.brickHeight + Globle.brickCenter.y;
        GameObject newBrick = (GameObject)Instantiate(brick, new Vector3(x, y, 0), Quaternion.identity);   
        BrickController brickController = newBrick.GetComponent<BrickController>();
        brickController.type = type;
        this.brickNumber += 1;
    }

    private void OnTriggerEnter2D(Collider2D collision){
        
    }
}
