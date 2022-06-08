using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//stationary enemy, handels shooting bullets at player in forward and reversed time, as well as animations for shooting
public class Turret : ReversableBody {

	//public bool reversed = false;
	private List<List<Sprite>> textures;
	private int state; //0 = idle, 1 = walk, 2 = shoot
	private int sequence;

    private int shootTimer = 0;
    private int shootTimerMax = 150;

    void Start() {
    	sequence = 0;
    	state = 0;
    	List<Sprite> idle = new List<Sprite>();
    	for (int i = 0; i <= 19; i++) {
    		idle.Add((Sprite)Resources.LoadAll("Textures/rifle/idle/survivor-idle_rifle_"+i, typeof(Sprite))[0]);
    	}
    	List<Sprite> walk = new List<Sprite>();
    	//dont bother loading
        List<Sprite> shoot = new List<Sprite>();
        for (int i = 0; i <= 2; i++) {
            shoot.Add((Sprite)Resources.LoadAll("Textures/rifle/shoot/survivor-shoot_rifle_"+i, typeof(Sprite))[0]);
        }
    	textures = new List<List<Sprite>>() {idle, walk, shoot};

    	GetComponent<SpriteRenderer>().sprite = textures[state][sequence];

        //deltaT = Time.deltaTime; //done here as deltatime changes meaning we get diffrent speeds of things moving
        defaultImage = true;
        /*route = new List<Vector3>() {new Vector3(2,2), new Vector3(-2,2), new Vector3(0,0)};
        CalculateCommands();*/
        //if (reversed) {shootTimer = 50;}
    }

    private GameObject FindClosest(Protagonist player) { //will only return player (may implement targeting ghosts)
		GameObject closest = player.gameObject;
		float distance = Vector3.Distance(closest.transform.position, this.transform.position);

		/*foreach (Ghost ghost in FindObjectsOfType<Ghost>()) {
			if (ghost.alive) {
				float newDis = Vector3.Distance(ghost.gameObject.transform.position, this.transform.position);
				if (distance > newDis) {
					distance = newDis;
					closest = ghost.gameObject;
				}
			}
		}*/
		return closest;
    }

    virtual public void ChangeImage() {
    	state = 2;
    	sequence = isRewinding? 2 : 0;
    }

    override public void UpdateImage(int rewind) { //updates image fro shooting
    	LookAtPlayer();
    	if (state == 2) {
    		GetComponent<SpriteRenderer>().sprite = textures[state][sequence];
	    	sequence += rewind;
	        if (sequence >= textures[state].Count) {
	        	state = 0;
	            sequence = 0;
	        }
	        else if (sequence < 0) {
	        	state = 0;
	            sequence = textures[state].Count;
	        }
	    }
    }

    protected void LookAtPlayer() {
    	Vector3 target = FindClosest(FindObjectsOfType<Protagonist>()[0]).gameObject.transform.position;
    	transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg);
    }

    override public void Step() { //go forward in time
    	if(alive) {
	    	if (shootTimer >= shootTimerMax) {
	    		shootTimer = 0;
	    		//for all players
	    			//if player in line of sight
	    				//shoot at player
	    				//break
	    		Protagonist player = FindObjectsOfType<Protagonist>()[0];
	    		GameObject closest = FindClosest(player);

	    		Vector3 posNormalized = Protagonist.Normalize(closest.transform.position - this.transform.position);
	            GameObject b = Instantiate(player.bullet, transform.position+new Vector3(posNormalized.x * (0.7f), posNormalized.y * (0.7f)), Quaternion.identity);
	    		b.GetComponent<Bullet>().velocity = posNormalized;
	    		b.GetComponent<Bullet>().polarity = 2; //1
	    		b.GetComponent<Bullet>().dependentBody = this.gameObject;
	    		ChangeImage();
	    	}
	    	else {
	    		shootTimer ++;
	    	}
	    }
    }

    override public void UnStep() { //rewind
    	if(alive) {
	    	if (shootTimer <= 0) { //it can now shoot reversed bullets -we would have to calculate the time until the bullet would return and if thats >= shootTimer *deltaTime
	    		shootTimer = shootTimerMax;
	    		Protagonist player = FindObjectsOfType<Protagonist>()[0];
	    		GameObject closest = FindClosest(player);

	    		RaycastHit2D hit = Physics2D.Raycast(this.transform.position, (closest.transform.position - this.transform.position).normalized, Mathf.Infinity, Physics2D.DefaultRaycastLayers, -2, -1);

	    		Vector3 posNormalized = Protagonist.Normalize(this.transform.position - closest.transform.position);
	            posNormalized.z = 0;
	            Vector3 inPosNormalized = Protagonist.Normalize(closest.transform.position - this.transform.position);
	            inPosNormalized.z = 0;
	            GameObject b = Instantiate(player.bullet, new Vector3(hit.point.x + posNormalized.x * (0.2f), hit.point.y + posNormalized.y * (0.2f)), Quaternion.identity);
	    		b.GetComponent<Bullet>().velocity = posNormalized;
	    		b.GetComponent<Bullet>().polarity = 2; //-1
	    		b.GetComponent<Bullet>().dependentBody = this.gameObject;
	    		b.GetComponent<Bullet>().endPoint = this.transform.position + new Vector3(inPosNormalized.x * (0.7f), inPosNormalized.y * (0.7f));
	    		ChangeImage();
	    	}
	    	else {
	    		shootTimer --;
	    	}
	    }
    }

    override public void OnDead() {
    	GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = null;
		FindObjectsOfType<Protagonist>()[0].AddKill();
	}

    override public void OnAlive() {
    	GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
