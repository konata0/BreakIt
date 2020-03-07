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
    void Start(){

        this.setLength(2);
        
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    // 设置长度
    private void setLength(int l){
        this.length = l;
        middle.transform.localScale = new Vector3(this.length, 1.0f, 1.0f);
        float offset = middle.GetComponent<Renderer>().bounds.extents.x - 0.01f;
        left.transform.localPosition = new Vector3(-offset, 0, 0);
        right.transform.localPosition = new Vector3(offset, 0, 0);
    }
}
