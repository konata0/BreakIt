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
        if(this.transform.position.x > ballMoveBorderX){
            this.speed.x = -Mathf.Abs(this.speed.x);
        }
        if(this.transform.position.x < -ballMoveBorderX){
            this.speed.x = Mathf.Abs(this.speed.x);
        }
        if(this.transform.position.y > ballMoveBorderY){
            this.speed.y = -Mathf.Abs(this.speed.y);
        }
        if(this.transform.position.y < -ballMoveBorderY){
            this.speed.y = Mathf.Abs(this.speed.y);
        }
        this.transform.Translate(speed * Time.deltaTime, Space.World);     
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag.Equals("Player")){
            float t = (this.transform.position.x - collision.gameObject.transform.position.x)/Globle.playerLength;
            t = (t > 0)? Mathf.Clamp(t * t, -1.0f, 1.0f): Mathf.Clamp(-t * t, -1.0f, 1.0f);
            float v = this.speed.magnitude;
            this.speed = (1.0f - t) * BallController.leftReflect + (t + 1.0f) * BallController.rightReflect;
            this.speed.Normalize();
            this.speed *= v;
        }     
    }
}
