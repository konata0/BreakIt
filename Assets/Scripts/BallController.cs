using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour{

    public Vector3 speed;
    private float screenX;
    private float screenY;
    private float ballMoveBorderX;
    private float ballMoveBorderY;
    private static Vector3 leftReflect = new Vector3(-1.0f, 0.3f, 0);
    private static Vector3 rightReflect = new Vector3(1.0f, 0.3f, 0);
    // Start is called before the first frame update
    void Start(){
        this.screenX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        this.screenY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
        this.ballMoveBorderX = this.screenX - this.GetComponent<Renderer>().bounds.extents.x;
        this.ballMoveBorderY = this.screenY - this.GetComponent<Renderer>().bounds.extents.y;        
    }

    // Update is called once per frame
    void Update(){
        if(this.transform.position.x > this.ballMoveBorderX){
            this.speed.x = -Mathf.Abs(this.speed.x);
        }
        if(this.transform.position.x < -this.ballMoveBorderX){
            this.speed.x = Mathf.Abs(this.speed.x);
        }
        if(this.transform.position.y > this.ballMoveBorderY){
            this.speed.y = -Mathf.Abs(this.speed.y);
        }
        if(this.transform.position.y < -(this.screenY * 2 - this.ballMoveBorderY)){
            SendMessageUpwards("dropBall", this.gameObject, SendMessageOptions.DontRequireReceiver);
        }
        this.transform.Translate(this.speed * Time.deltaTime, Space.World);     
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag.Equals("Player")){
            float t = (this.transform.position.x - collision.gameObject.transform.position.x)/Globle.playerLength;
            float v = this.speed.magnitude;
            this.speed = this.getReflectSpeed(t, v);
        }else if(collision.gameObject.tag.Equals("Brick")){
            float x = this.transform.position.x - collision.gameObject.transform.position.x;
            float y = this.transform.position.y - collision.gameObject.transform.position.y;
            float bx = Globle.brickWidth/2.0f;
            float by = Globle.brickHeight/2.0f;
            float l1 = x - bx + by;
            float l2 = -x - bx + by;
            float l3 = x + bx - by;
            float l4 = -x + bx - by;
            // 0123：上下左右
            int t = -1;
            if((y >= l1)&&(y >= l2)&&(y >= 0)){
                t = 0;
            }else if((y <= l3)&&(y <= l4)&&(y <= 0)){
                t = 1;
            }else if((y >= l3)&&(y <= l2)&&(x <= -bx + by)){
                t = 2;
            }else if((y <= l1)&&(y >= l4)&&(x >= bx - by)){
                t = 3;
            }else{
                t = -1;
            }
            switch(t){
                case 0:{
                    this.speed.y = Mathf.Abs(this.speed.y);
                    break;
                }
                case 1:{
                    this.speed.y = -Mathf.Abs(this.speed.y);
                    break;
                }
                case 2:{
                    this.speed.x = -Mathf.Abs(this.speed.x);
                    break;
                }
                case 3:{
                    this.speed.x = Mathf.Abs(this.speed.x);
                    break;
                }
                default:{
                    break;
                }
            }  
        }   
    }

    public Vector3 getReflectSpeed(float t, float v){
        t = (t > 0)? Mathf.Clamp(t * t, -1.0f, 1.0f): Mathf.Clamp(-t * t, -1.0f, 1.0f);
        Vector3 newSpeed = (1.0f - t) * BallController.leftReflect + (t + 1.0f) * BallController.rightReflect;
        newSpeed.Normalize();
        return newSpeed * v;
    }

    public void changeSpeed(float scale){
        float v = Mathf.Clamp(this.speed.magnitude * scale, 1.8f, 7.2f);
        this.speed.Normalize();
        this.speed *= v;
    }
}
