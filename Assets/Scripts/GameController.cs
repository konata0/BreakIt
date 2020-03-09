using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour{
    public Sprite[] backgroundList;
    public Sprite[] brickList;
    public GameObject ball;
    public GameObject brick;
    public GameObject redExplodeEffect;
    public GameObject blueExplodeEffect;
    public GameObject yellowExplodeEffect;
    public GameObject item;
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
        GameObject newBall = (GameObject)Instantiate(ball, new Vector3(0, 0.6f, 0), Quaternion.identity);
        BallController newBallController = newBall.GetComponent<BallController>();
        newBall.transform.SetParent(this.transform);
        newBallController.speed = new Vector3(0, -3.6f, 0);
        this.ballNumber = 1;

        // 创建砖块
        GameObject tempBrick = (GameObject)Instantiate(brick, new Vector3(0, 0, 0), Quaternion.identity);   
        Renderer r = tempBrick.GetComponent<Renderer>();
        Globle.brickHeight = r.bounds.extents.y * 2.0f;
        Globle.brickWidth = r.bounds.extents.x * 2.0f;
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
        float x = (((float)col) - ((float)Globle.col - 1.0f) / 2.0f) * Globle.brickWidth + Globle.brickCenter.x;
        float y = (((float)Globle.row - 1.0f) / 2.0f - ((float)row)) * Globle.brickHeight + Globle.brickCenter.y;
        GameObject newBrick = (GameObject)Instantiate(brick, new Vector3(x, y, 0), Quaternion.identity);   
        newBrick.transform.SetParent(this.transform);
        BrickController brickController = newBrick.GetComponent<BrickController>();
        brickController.type = type;
        brickController.row = row;
        brickController.col = col;
        if(type != 7){
            this.brickNumber += 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        
    }

    // 击碎砖块
    private void breakBrick(GameObject brick){
        if(Random.Range(0, 1.0f) < Globle.itemDropRate){
            GameObject newItem = (GameObject)Instantiate(item, brick.transform.position, Quaternion.identity);
            int type = 0;
            float random = Random.Range(0, 5.0f);
            if(random > 1.0f){
                type = 1;
            }
            if(random > 2.0f){
                type = 2;
            }
            if(random > 3.0f){
                type = 3;
            }
            if(random > 4.0f){
                type = 4;
            }
            newItem.GetComponent<ItemController>().type = type;
        }
        Destroy(brick);
        this.brickNumber -= 1;
    }

    // Blue爆炸
    private void strikeBlue(int index){
        int col = index % Globle.col;
        int row = (index - col) / Globle.col;
        float x = (((float)col) - ((float)Globle.col - 1.0f) / 2.0f) * Globle.brickWidth + Globle.brickCenter.x;
        float y = (((float)Globle.row - 1.0f) / 2.0f - ((float)row)) * Globle.brickHeight + Globle.brickCenter.y;
        GameObject newEffect = (GameObject)Instantiate(blueExplodeEffect, new Vector3(x, y, 0), Quaternion.identity);   
        Destroy(newEffect, 1.2f);
    }

    // Red爆炸
    private void strikeRed(int index){
        int col = index % Globle.col;
        int row = (index - col) / Globle.col;
        float x = (((float)col) - ((float)Globle.col - 1.0f) / 2.0f) * Globle.brickWidth + Globle.brickCenter.x;
        float y = (((float)Globle.row - 1.0f) / 2.0f - ((float)row)) * Globle.brickHeight + Globle.brickCenter.y;
        GameObject newEffect = (GameObject)Instantiate(redExplodeEffect, new Vector3(x, y, 0), Quaternion.identity);   
        Destroy(newEffect, 1.2f);
        BroadcastMessage("redExplode", index, SendMessageOptions.DontRequireReceiver);
    }

    // Yellow爆炸
    private void strikeYellow(int index){
        int col = index % Globle.col;
        int row = (index - col) / Globle.col;
        float x = (((float)col) - ((float)Globle.col - 1.0f) / 2.0f) * Globle.brickWidth + Globle.brickCenter.x;
        float y = (((float)Globle.row - 1.0f) / 2.0f - ((float)row)) * Globle.brickHeight + Globle.brickCenter.y;
        GameObject newEffect = (GameObject)Instantiate(yellowExplodeEffect, new Vector3(x, y, 0), Quaternion.identity);   
        Destroy(newEffect, 1.2f);
        BroadcastMessage("yellowExpode", index, SendMessageOptions.DontRequireReceiver);
    }

    // 复制Ball
    private void doubleBall(GameObject orignalBall){
        GameObject newBall = (GameObject)Instantiate(ball, orignalBall.transform.position, Quaternion.identity);
        BallController newBallController = newBall.GetComponent<BallController>();
        newBall.transform.SetParent(this.transform);
        float v = orignalBall.GetComponent<BallController>().speed.magnitude;
        newBallController.speed = newBallController.getReflectSpeed(Random.Range(-1.0f, 1.0f), v);
        this.ballNumber += 1;
    }

    // 球到达下边界
    private void dropBall(GameObject ballToDestroy){
        Destroy(ballToDestroy);
        this.ballNumber -= 1;
    }

    // 改变球速
    private void changeBallSpeed(float scale){
        BroadcastMessage("changeSpeed", scale, SendMessageOptions.DontRequireReceiver);
    }

    
}
