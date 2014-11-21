using UnityEngine;
using System.Collections;

public class MoveableObjectController : MonoBehaviour {

    public float xDistance = 2.0f;
    public float yDistance = 0.0f;
    public float moveTime = 1.0f;

    private bool forward = true;
    private bool moving = false;

    private Vector2 startPosition = new Vector2();

    // Use this for initialization
    void Start ()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if(moving)
        {
            float newX = transform.position.x;
            float newY = transform.position.y;
            newX += (forward ? 1 : -1) * xDistance * Time.deltaTime / moveTime;
            newY += (forward ? 1 : -1) * yDistance * Time.deltaTime / moveTime;

            transform.position = new Vector2(newX, newY);
            if((newX <= startPosition.x && newY <= startPosition.y) ||
                (newX >= startPosition.x + xDistance && newY >= startPosition.y + yDistance))
            {
                moving = false;
                forward = !forward;
            }
        }
    }

    public void On()
    {
        moving = true;
    }

    public void Off()
    {
        moving = false;
    }

    public void Forward()
    {
        moving = true;
        forward = true;
    }

    public void Reverse()
    {
        moving = true;
        forward = false;
    }

}
