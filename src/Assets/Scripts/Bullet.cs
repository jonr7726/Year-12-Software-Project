using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//bullet is created by turrets and the player when they shoot, it will kill the player or turret on impact
public class Bullet : ReversableBody {

    public Vector3 velocity;
    public float speed;
    public Vector3 endPoint = new Vector3(0,0,10);

    public int polarity; //-1 = reversed, 1 = forward, 0 = both ,2 = neither ->if say reversed, once the time becomes reversed the bullet will cease to exist once not alive -allowing a different course of action to be taken by enemies

    void Start() {
    	GetComponent<SpriteRenderer>().sprite = sprite;
        existsBeforeTime = false;
        velocity = new Vector3(velocity.x * speed, velocity.y * speed);
    }

    override public void Step() {
    	if (alive) {
	    	if (endPoint != new Vector3(0,0,10) & Vector3.Distance(transform.position, endPoint) < 0.15f) {
	    		Kill();
	    		//commandsIndex ++;
	    	}
	    	else {
	        	transform.Translate(velocity);
	        }
	    }
    }

    private void OnTriggerEnter2D(Collider2D other) {
    	if (alive) {
	    	if (other.tag == "Player") {
	    		other.GetComponent<Protagonist>().Kill();
	    		Kill();
	    	}
	    	else if (other.tag == "Ghost") {
	        	other.GetComponent<Ghost>().player.Kill();
	        	Kill();
	        }
	    	else if (other.tag == "Enemy") {
		        	other.GetComponent<ReversableBody>().Kill();
		        	Kill();
	        }
	        else if (other.tag == "Wall") {
	        	Kill();
	        }
	    }
    }

    override public void OnDead() {
    	GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = null;
        if(polarity == 2) {
        	Destroy(this.gameObject);
        }
    }

    override public void OnAlive() {
    	if (dependentBody) {
            if(!dependentBody.GetComponent<Turret>().alive) {
                Destroy(this.gameObject);
            }
        }
    	if((polarity == 1 & !isRewinding) | (polarity == -1 & isRewinding)) {
        	Destroy(this.gameObject);
        }
        else {
	    	GetComponent<CircleCollider2D>().enabled = true;
	        GetComponent<SpriteRenderer>().sprite = sprite;
	    }
    }
}
