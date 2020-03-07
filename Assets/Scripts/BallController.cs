using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour{

    public Vector3 speed;
    private float screenX;
    private float screenY;
    private float ballMoveBorderX;
    private float ballMoveBorderY;
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
}
