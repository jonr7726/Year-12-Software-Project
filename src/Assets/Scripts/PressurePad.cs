using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//pressure pad that will be activated by player or guard or ghost
public class PressurePad : MonoBehaviour {

	public bool activated = false;
	public MovingObject movingObject;
	//public List<Vector3> route;
	public Color activatedColor;
	private Color originalColor;

	public void ChangeColour() { //needs to be called from the door when it goes back to its orininal position
		if (activatedColor != new Color()) {
			GetComponent<SpriteRenderer>().color = originalColor;
		}
	}

    private void OnTriggerEnter2D(Collider2D other) {
    	if (movingObject & activated) {
    		if (!movingObject.hasOrders | (movingObject.isRewinding & movingObject.gameObject.transform.position == movingObject.firstPosition)) {
    			activated = false;
    		}
    	}
    	if ((other.tag == "Enemy" | other.tag == "Player" | other.tag == "Ghost") & !activated) {
    		activated = true;
    		if (movingObject) {
    			movingObject.commands.Clear();
    			movingObject.CalculateCommands();
    		}
    		if (activatedColor != new Color()) {
    			originalColor = GetComponent<SpriteRenderer>().color;
                GetComponent<SpriteRenderer>().color = activatedColor;
            }
        }
    }
}
