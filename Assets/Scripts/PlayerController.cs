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
    private BoxCollider2D playerCollider;
    private Renderer middleRenderer;
    private Rigidbody2D playerRigidbody;
    private float height;
    private float width;
    void Start(){

        initPosition = new Vector3(0, -2.3f, 0);

        this.playerCollider = this.GetComponent<BoxCollider2D>();
        this.middleRenderer = this.middle.GetComponent<Renderer>();
        this.playerRigidbody = this.GetComponent<Rigidbody2D>();
        this.height = this.middleRenderer.bounds.extents.y * 2.0f;
        this.width = this.middleRenderer.bounds.extents.x * 2.0f;

        this.setLength(4);
        
        
    }

    // Update is called once per frame
    void Update(){
        // 移动
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition = new Vector3(Mathf.Clamp(mousePosition.x, -playerMoveBorderX, playerMoveBorderX), initPosition.y, 0);
        this.playerRigidbody.MovePosition(newPosition);
    }

    // 设置长度
    private void setLength(int l){
        // 设置长度
        this.length = l;
        middle.transform.localScale = new Vector3(this.length, 1.0f, 1.0f);
        // 获取实际长度
        Globle.playerLength = this.middleRenderer.bounds.extents.x - 0.01f;
        this.height = this.middleRenderer.bounds.extents.y * 2.0f;
        this.width = this.middleRenderer.bounds.extents.x * 2.0f;
        // 设置左右两端位置
        left.transform.localPosition = new Vector3(-Globle.playerLength, 0, 0);
        right.transform.localPosition = new Vector3(Globle.playerLength, 0, 0);
        // 设置移动边界
        float screenX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        this.playerMoveBorderX = screenX - this.middleRenderer.bounds.extents.x;
        // 设置碰撞体大小
        this.playerCollider.size = new Vector2(this.width, this.height);

    }
}
