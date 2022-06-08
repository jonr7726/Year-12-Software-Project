using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * ReversableBody is a superclass that entails all objects that are reversed when the player reverses time
 */
public class ReversableBody : MonoBehaviour {

	public bool isRewinding = false;
	public bool alive = true;
    public Sprite sprite; //image to display
    public bool existsAfterTime = true; //exists after out of commands
    public bool existsBeforeTime = true; //exists before commands start (for when time is reversed)
    public GameObject dependentBody = null; //object that it's existance is dependent on (ie. if that object is destroyed, so is this object)
    public bool defaultImage;

	public List<TimeCommand> commands = new List<TimeCommand>(); //list of commands for movement
	public int commandsIndex = 0;

    void Update() { //check for user q input and reverse time
        if (Input.GetKeyUp(KeyCode.Q)) {InvertRewind();}
    }

    void FixedUpdate() { //if game not paused, update position, image and call subclass overriden methods
        if (!WorldScript.paused) {
            if (isRewinding) {
                UnStep();
                Rewind();
                UpdateImage(-1);
            }
            else {
                Step();
                Move();
                UpdateImage(1);
            }
        }
    }

    virtual public void Step() {} //called every loop when moving forward in time, overridden by subcalsses
    virtual public void UnStep() {} //called every loop when moving backward in time, overridden by subclasses
    virtual public void UpdateImage(int rewind) {} //overridden by subclasses, used for guards who need their images updated when moving

    private void ProcessCommand() { //process command for object
        if (!(commandsIndex < commands.Count)) {commandsIndex = commands.Count - 1;print("error");}
    	transform.position = commands[commandsIndex].position; //move and rotate object as per command
        transform.rotation = commands[commandsIndex].rotation;
        
		if (alive & ((commands[commandsIndex].killed == 1 & !isRewinding) | (commands[commandsIndex].killed == -1 & isRewinding))) { //
			alive = false;
			OnDead();
		}
        else if (!alive & ((commands[commandsIndex].killed == 1 & isRewinding) | (commands[commandsIndex].killed == -1 & !isRewinding))) {
            alive = true;
            OnAlive();
        }
        if (commands[commandsIndex].sprite != null & alive) {
            GetComponent<SpriteRenderer>().sprite = commands[commandsIndex].sprite;
        }
        if (commands[commandsIndex].clearOnForward) {
            print(commandsIndex);
            OnClear();
            if (!isRewinding) {
                commands.Clear();
                OnClear();
                commandsIndex = 0;
            }
        }
    }

    virtual protected void OnClear() {} //called when commands are completed when rewinding (used for doors)

    private void Rewind() { //moves object (back in time), if command, process that, otherwise, if it is to be displayed before command starts, add a command for it being still
        if (commandsIndex >= 0) {
	    	ProcessCommand();
	    	commandsIndex --;
	    }
	    else {
            if(defaultImage) {
                commands.Insert(0, new TimeCommand(transform.position, transform.rotation, sprite:sprite));
                if (alive) {GetComponent<SpriteRenderer>().sprite = sprite;}
            }
            else {commands.Insert(0, new TimeCommand(transform.position, transform.rotation));}
            if (!existsBeforeTime & alive) {
                OnDead();
                alive = false;
                commands[commandsIndex+1].Kill(isRewinding? -1 : 1);
            }
	    }
    }

    private void Move() { //moves object (forward in time), if command, process that, otherwise, if it is to be displayed after command ends, add a command for it being still
    	if (commandsIndex < commands.Count) {
    		ProcessCommand();
    		commandsIndex ++;
    	}
    	else {
            if(defaultImage) {commands.Add(
                new TimeCommand(transform.position, transform.rotation, sprite:sprite));
                if (alive) {GetComponent<SpriteRenderer>().sprite = sprite;}
            }
            else {commands.Add(new TimeCommand(transform.position, transform.rotation));}
            if (!existsAfterTime & alive) {
                OnDead();
                alive = false;
                commands[commandsIndex].Kill(isRewinding? -1 : 1);
            }
	    	commandsIndex ++;
	    }
    }

    public void StartRewind() { //rewinds object
    	isRewinding = true;
    	commandsIndex --;
    }

    public void StopRewind() { //stops rewinding object
    	isRewinding = false;
    	commandsIndex ++;
    }

    public void InvertRewind() { //inverts rewinding of object
    	if (isRewinding) {
    		StopRewind();
       	}
       	else {
       		StartRewind();
       	}
    }

    public void Kill() { //kills object 
        int i = isRewinding? 3 : -3; //3 and -3 are a threshold to determine when to kill the object (due to issues with command system)
        OnDead();
        if ((commandsIndex + i < 0 & !isRewinding) | (commandsIndex + i >= commands.Count & isRewinding)) {
            Destroy(this.gameObject); //lived for too short to actualy have enough commands to put deaths in -stops errors that may arise
        }
        else {
            commands[commandsIndex + i].Kill(isRewinding? -1 : 1);
            alive = false;
        }
    }

    virtual public void OnDead() { //called when object is killed
        GetComponent<SpriteRenderer>().sprite = null; //stops displaying image
    }

    virtual public void OnAlive() { //when object is 'unkilled' (for reversing)
        if (dependentBody) { //destroy object if dependent object is destroyed
            if(!dependentBody.GetComponent<Turret>().alive) {
                Destroy(this.gameObject);
            }
        }
        else { //otherwise display object again
            GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
