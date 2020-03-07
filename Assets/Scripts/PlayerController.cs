using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject left;
    public GameObject right;
    public GameObject middle;

    private int length = 2;
    private float playerMoveBorderX;
    private Vector3 initPosition;
    private Vector3 mousePosition;
    private Vector3 newPosition;
    void Start(){
        initPosition = new Vector3(0, -2.3f, 0);
        this.setLength(2);
        
        
    }

    // Update is called once per frame
    void Update(){
        // 移动
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition = new Vector3(Mathf.Clamp(mousePosition.x, -playerMoveBorderX, playerMoveBorderX), initPosition.y, 0);
        this.GetComponent<Rigidbody2D>().MovePosition(newPosition);
    }

    // 设置长度
    private void setLength(int l){
        // 设置长度
        this.length = l;
        middle.transform.localScale = new Vector3(this.length, 1.0f, 1.0f);
        // 获取实际长度
        Globle.playerLength = middle.GetComponent<Renderer>().bounds.extents.x - 0.01f;
        // 设置左右两端位置
        left.transform.localPosition = new Vector3(-Globle.playerLength, 0, 0);
        right.transform.localPosition = new Vector3(Globle.playerLength, 0, 0);
        // 设置移动边界
        float screenX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        this.playerMoveBorderX = screenX - middle.GetComponent<Renderer>().bounds.extents.x;
    }
}
