using UnityEngine;
using System.Collections;

public class DeathController : MonoBehaviour {

    public Transform startPosition;

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.position = startPosition.position;
            other.gameObject.rigidbody2D.velocity = Vector2.zero;
        }
    }
}
