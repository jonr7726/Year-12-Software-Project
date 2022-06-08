using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//moving object is a reversable body that can move with commands
public class MovingObject : ReversableBody {

    public float speed = 1;
    public List<Vector3> route;
    private float deltaT = 0.02f;
    public bool hasOrders;
    //public bool hasPressurePad;
    public Vector3 firstPosition;

    private PressurePad pressurePad; //for changing colour of it

    void Start() {
    	//deltaT = Time.deltaTime; //done here as deltatime changes meaning we get diffrent speeds of things moving
        if (route.Count != 0 & hasOrders) {
        	CalculateCommands();
        }
        firstPosition = transform.position;
    }

    override protected void OnClear() {
        //if (hasPressurePad) {
        if (pressurePad != null) {
            hasOrders = false;
            pressurePad.ChangeColour();
	    	print("clear!");
	    }
    }

    public void CalculateCommands(PressurePad p = null) { //will calculate movement from list of positions to move to
        pressurePad = p;
    	//print("making commands");
    	hasOrders = true;
    	float step = speed * deltaT;
    	Vector3 position = transform.position;
    	Quaternion rotation = transform.rotation;
        //if (hasPressurePad) {
        if (pressurePad != null) {
            this.commands.Add(new TimeCommand(position, rotation, clearOnForward:true));
    	}
    	int i = 0;
    	foreach (Vector3 vector in route) {
    		while (Vector3.Distance(position, vector) > 0.001f) {
    			position = Vector3.MoveTowards(position, vector, step);
    			this.commands.Add(new TimeCommand(position, rotation));
    			i++;
    		}
    	}
    	commandsIndex = commands.Count - i;
    	//this.commands.Insert(0, new TimeCommand(transform.position, transform.rotation));
    }
}
