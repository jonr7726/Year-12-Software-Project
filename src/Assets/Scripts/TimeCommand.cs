using UnityEngine;

//time command that contians information for the rotation, position, image, if object is alive, if object is cleared for a button to interact with it once it reaches it's position
public class TimeCommand {
	
	public Vector3 position;
	public Quaternion rotation;
	public Sprite sprite;
	public int killed; //int 1 = destory future, -1 = destroy past, 0 = null
	public bool clearOnForward;

	public TimeCommand (Vector3 position, Quaternion rotation, int killed=0, Sprite sprite=null, bool clearOnForward=false) {
		this.position = position;
		this.rotation = rotation;
		this.killed = killed;
		this.sprite = sprite;
		this.clearOnForward = clearOnForward;
	} 

	public void Kill(int killed) {
		this.killed = killed;
	}
}
