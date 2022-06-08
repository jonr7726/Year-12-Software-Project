using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//guard is a turret that can move, it has a command chain and will change images to show movement as they walk
public class Guard : Turret {

	private List<List<Sprite>> textures;
	private int state; //0 = idle, 1 = walk, 2 = shoot
	private int sequence;

	public float speed = 1;
    public List<Vector3> route;
    private float deltaT = 0.02f;

    void Start() {
    	sequence = 0;
    	state = 0;
    	List<Sprite> idle = new List<Sprite>();
    	for (int i = 0; i <= 19; i++) {
    		idle.Add((Sprite)Resources.LoadAll("Textures/rifle/idle/survivor-idle_rifle_"+i, typeof(Sprite))[0]);
    	}
    	List<Sprite> walk = new List<Sprite>();
    	for (int i = 0; i <= 19; i++) {
    		walk.Add((Sprite)Resources.LoadAll("Textures/rifle/move/survivor-move_rifle_"+i, typeof(Sprite))[0]);
    	}
        List<Sprite> shoot = new List<Sprite>();
        for (int i = 0; i <= 2; i++) {
            shoot.Add((Sprite)Resources.LoadAll("Textures/rifle/shoot/survivor-shoot_rifle_"+i, typeof(Sprite))[0]);
        }
    	textures = new List<List<Sprite>>() {idle, walk, shoot};

    	GetComponent<SpriteRenderer>().sprite = textures[state][sequence];

        //deltaT = Time.deltaTime; //done here as deltatime changes meaning we get diffrent speeds of things moving
        CalculateCommands();
        defaultImage = true;
    }

    override public void UpdateImage(int rewind) {
    	LookAtPlayer();
    	if (state == 2) {
    		GetComponent<SpriteRenderer>().sprite = textures[state][sequence];
	    	sequence += rewind;
	        if (sequence >= textures[state].Count) {
	        	state = 1;
	            sequence = 0;
	        }
	        else if (sequence < 0) {
	        	state = 1;
	            sequence = textures[state].Count;
	        }
	    }
    }

    public void CalculateCommands() { //calculates their movement from commands
    	float step = speed * deltaT;
    	Vector3 position = transform.position;
    	Quaternion rotation = transform.rotation;
    	int i = 0;
    	state = 1;
    	foreach (Vector3 vector in route) {
    		while (Vector3.Distance(position, vector) > 0.001f) {
    			sequence ++;
	            if (sequence >= textures[state].Count) {
	                sequence = 0;
	            }
    			position = Vector3.MoveTowards(position, vector, step);
    			this.commands.Add(new TimeCommand(position, rotation, sprite:textures[state][sequence]));
    			i++;
    		}
    	}
    	commandsIndex = commands.Count - i;
    	this.commands.Insert(0, new TimeCommand(transform.position, transform.rotation, sprite:textures[0][0]));
    }
}
