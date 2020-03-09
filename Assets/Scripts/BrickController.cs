using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController : MonoBehaviour{

    public int type = 0;
    public int row;
    public int col;
    public Sprite[] spriteList;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start(){
        this.type = (this.type < 0)? 0: this.type;
        this.type = (this.type > spriteList.Length - 1)? spriteList.Length - 1: this.type;
        this.spriteRenderer  = GetComponent<SpriteRenderer>();
        this.spriteRenderer.sprite = spriteList[type];
        Renderer r = this.GetComponent<Renderer>();
        float width = r.bounds.extents.x * 2.0f;
        float height = r.bounds.extents.y * 2.0f;
        this.GetComponent<BoxCollider2D>().size = new Vector2(width, height);    
    }

    // Update is called once per frame
    void Update(){

    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag.Equals("Ball")){
            this.strike();
        }
    }

    private void strike(){
        switch(this.type){
            // Blue 0
            case 0:{
                this.type = -1;
                SendMessageUpwards("breakBrick", this.gameObject, SendMessageOptions.DontRequireReceiver);
                SendMessageUpwards("strikeBlue", this.row * Globle.col + this.col, SendMessageOptions.DontRequireReceiver);
                break;
            }
            // Blue 1
            case 1:{
                this.type = 0;
                this.spriteRenderer.sprite = spriteList[type];
                break;
            }
            // Blue 2
            case 2:{
                this.type = 1;
                this.spriteRenderer.sprite = spriteList[type];
                break;
            }
            // Blue 3
            case 3:{
                this.type = 2;
                this.spriteRenderer.sprite = spriteList[type];
                break;
            }
            // Red
            case 4:{
                this.type = -1;
                SendMessageUpwards("breakBrick", this.gameObject, SendMessageOptions.DontRequireReceiver);
                SendMessageUpwards("strikeRed", this.row * Globle.col + this.col, SendMessageOptions.DontRequireReceiver);
                break;
            }
            // Yellow
            case 5:{
                this.type = -1;
                SendMessageUpwards("breakBrick", this.gameObject, SendMessageOptions.DontRequireReceiver);
                SendMessageUpwards("strikeYellow", this.row * Globle.col + this.col, SendMessageOptions.DontRequireReceiver);
                break;
            }
            // Green
            case 6:{
                this.type = -1;
                SendMessageUpwards("breakBrick", this.gameObject, SendMessageOptions.DontRequireReceiver);
                break;
            }
            // Gray
            case 7:{
                break;
            }
            default:{
                break;
            }
        }
    }

    private void redExplode(int index){
        int redCol = index % Globle.col;
        int redRow = (index - redCol) / Globle.col;
        int dis = Mathf.Abs(this.row - redRow) + Mathf.Abs(this.col - redCol);
        if( dis == 1){
            this.strike();
        }
    }

    private void yellowExpode(int index){
        int yellowCol = index % Globle.col;
        int yellowRow = (index - yellowCol) / Globle.col;
        bool flag1 = (yellowCol == this.col);
        bool flag2 = (yellowRow == this.row);
        if((flag1 || flag2)&&(!(flag1&&flag2))){
            this.strike();
        }
    }

}
