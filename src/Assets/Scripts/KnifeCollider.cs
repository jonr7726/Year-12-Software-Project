using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//collider for knife, will kill enemies
public class KnifeCollider : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D other) {
    	if (other.tag == "Enemy") {
        	GetComponentInParent<Protagonist>().knifeCollision.Add(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
    	if (other.tag == "Enemy") {
        	GetComponentInParent<Protagonist>().knifeCollision.Remove(other.gameObject);
        }
    }
}
