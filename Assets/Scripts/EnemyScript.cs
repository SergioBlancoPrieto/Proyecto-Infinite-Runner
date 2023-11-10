using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float runningSpeed;
    private Rigidbody2D _rigidbody2D;
    public LayerMask layerDown;
    public LayerMask layerFront;
    public float distanceDown;
    public float distanceFront;
    public Transform controllerFront;
    public Transform controllerDown;
    public bool infoDown;
    public bool infoFront;
    public bool lookingRight = true;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
	
    private void Update() 
    {
        _rigidbody2D.velocity = new Vector2(runningSpeed, _rigidbody2D.velocity.y);

        infoFront = Physics2D.Raycast(controllerFront.position, transform.right, distanceFront, layerFront);
        infoDown = Physics2D.Raycast(controllerDown.position, transform.up * -1, distanceDown, layerDown);

        if (infoFront || !infoDown)
        {
            Turn();
        }
    }

    private void Turn()
    {
        lookingRight = !lookingRight;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        runningSpeed *= -1;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(controllerDown.transform.position, controllerDown.transform.position + transform.up * -1 * distanceDown);
        Gizmos.DrawLine(controllerFront.transform.position, controllerFront.transform.position + transform.right * distanceFront);
    }
	
    /*private void FixedUpdate()
    {
        float currentRunningSpeed = runningSpeed;

        if (_triggerMovement.turnAround == true)
        {
            currentRunningSpeed = -runningSpeed;
            transform.eulerAngles = new Vector3(0, 180f, 0);
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
        }

        if (GameManager.sharedInstance.currentGameState == GameState.inTheGame)
        {
            if (_rigidbody2D.velocity.x < runningSpeed)
            {
                _rigidbody2D.velocity = new Vector2(currentRunningSpeed, _rigidbody2D.velocity.y);
            }
        }
    }*/
}
