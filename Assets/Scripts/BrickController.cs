using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController : MonoBehaviour{

    public int type = 0;
    // Start is called before the first frame update
    void Start(){
        Renderer r = this.GetComponent<Renderer>();
        float width = r.bounds.extents.y * 2.0f;
        float height = r.bounds.extents.x * 2.0f;
        this.GetComponent<BoxCollider2D>().size = new Vector2(width, height);       
    }

    // Update is called once per frame
    void Update(){
        
    }
}
