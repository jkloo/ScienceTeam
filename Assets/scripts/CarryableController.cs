using UnityEngine;
using System.Collections;

public class CarryableController : MonoBehaviour {

    private Animator anim;
    private bool carried = false;
    private Vector2 startPosition;
    private CameraFollow cameraFollow;
    private HandleMovingPlatforms movingPlatformHandler;

    void Awake()
    {
        anim = GetComponent<Animator>();
        movingPlatformHandler = GetComponent<HandleMovingPlatforms>();
        startPosition = transform.position;
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraFollow = camera.GetComponent<CameraFollow>();
    }

    public void SetCarriedStatus(bool status)
    {
        carried = status;
        collider2D.isTrigger = status;
        movingPlatformHandler.standingOn = null;
    }

    void FixedUpdate()
    {
        if(!InRange(transform.position.x, cameraFollow.minX, cameraFollow.maxX)
            || !InRange(transform.position.y, cameraFollow.minY - 5, cameraFollow.maxY + 5))
        {
            transform.position = startPosition;
            rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
            transform.rotation.Set(0.0f, 0.0f, 0.0f, 0.0f);
            rigidbody2D.angularVelocity = 0.0f;
        }
    }

    bool InRange(float v, float min, float max)
    {
        return v >= min && v <= max;
    }

}
