using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//reversed player that mimicks their movement
public class Ghost : ReversableBody {

    public Protagonist player;

    override public void OnDead() {
    	GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = null;
    }

    override public void OnAlive() {
    	GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
