using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour{

    public int type = 0;
    public Sprite[] ItemList;
    // Start is called before the first frame update
    private Vector3 speed = new Vector3(0, -2.0f, 0);  
    void Start(){
        this.type = (this.type < 0)? 0: this.type;
        this.type = (this.type > ItemList.Length - 1)? ItemList.Length - 1: this.type;
        this.GetComponent<SpriteRenderer>().sprite = ItemList[type];        
    }

    // Update is called once per frame
    void Update(){
        this.transform.Translate(this.speed * Time.deltaTime, Space.World); 
    }
}
