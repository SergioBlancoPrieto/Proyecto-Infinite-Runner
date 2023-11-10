/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float runningSpeed;
    private Rigidbody2D _rigidbody2D;
	//public LayerMask _layerDown;
	//public LayerMask _layerFront;
	public float distanceDown;
	public float distanceFront;
    /*public TriggerMovement _triggerMovementFront;
	public TriggerMovement _triggerMovementDown;
	public Transform _controllerFront;
	public Transform _controllerDown;
	public bool infoDown;
	public bool infoFront;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
	
	private void Update() 
	{
		_rigidbody2D.velocity = new Vector2(runningSpeed, _rigidbody2D.velocity.y);

		infoFront = Physics2D.Raycast(_controllerFront.position, transform.right, distanceFront, layerFront);
		infoDown = Physics2D.Raycast(_controllerDown.position, transform.up * -1, distanceDown, layerDown);

		if (infoFront || !infoDown)
		{
			Turn();
		}
	}

	private void Turn()
	{
        transform.eulerAngles = new Vector3(0, 180f, 0);
		runningSpeed *= -1;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(_controllerDown.transform.position, _controllerDown.transform.position + transform.up * -1 * distanceDown);
		Gizmos.DrawLine(_controllerFront.transform.position, _controllerFront.transform.position + transform.right * distanceFront);
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
