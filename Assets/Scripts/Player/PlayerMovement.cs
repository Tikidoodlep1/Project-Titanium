using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField]
	private float MAX_SPEED = 0.10f;
	[SerializeField]
	private float MAX_MOMENTUM = 1.0f;
	[SerializeField]
	private float MOMENTUM_STEP = 0.02f;

	private int tempTestCounter = 0;
	private InputAction moveAction;
	//Vector2 for x and y acceleration
	//private Vector3 speed = new Vector3(0f, 0f, 0f);
	//Vector2 for momentum to create the slidy movement feel
	private Vector3 momentum = new Vector3(0f, 0f, 0f);

	void Awake()
    {
		//InputSystem is the new Unity System for handling input and controls. Defined in Project Settings
		moveAction = InputSystem.actions.FindAction("Move");
    }

	// Update is called 50 times per second, or every 0.2 seconds by default
	private void FixedUpdate()
	{
		Vector2 moveValue = moveAction.ReadValue<Vector2>();
		float xAdjust = moveValue.x;
		float yAdjust = moveValue.y;

		if(xAdjust != 0 || yAdjust != 0) {
			//Note that xAdjust and yAdjust are normalized values - they're either -1, 0, or 1.
			this.momentum.x += xAdjust * MOMENTUM_STEP;
			this.momentum.y += yAdjust * MOMENTUM_STEP;
			//Clamp speed to ensure we don't get too zoomy
			this.momentum.x = math.clamp(this.momentum.x, -MAX_MOMENTUM, MAX_MOMENTUM);
			this.momentum.y = math.clamp(this.momentum.y, -MAX_MOMENTUM, MAX_MOMENTUM);
		}
		
		if(this.momentum.x != 0f || this.momentum.y != 0f)
		{
			if (this.momentum.x > 0)
			{
				this.momentum.x -= (MOMENTUM_STEP/2);
			}
			else if (this.momentum.x < 0)
			{
				this.momentum.x += (MOMENTUM_STEP/2);
			}

			if (this.momentum.y > 0)
			{
				this.momentum.y -= (MOMENTUM_STEP/2);
			}
			else if(this.momentum.y < 0)
			{
				this.momentum.y += (MOMENTUM_STEP/2);
			}
		}

		//if (tempTestCounter++ % 100 == 0) //This is only here and used to avoid printing 50 times per second.
		//{
		//	Debug.Log(this.momentum);
		//}

		if (this.momentum.x != 0.0f || this.momentum.y != 0.0f) {
			this.gameObject.transform.position += (MAX_SPEED * this.momentum);
		}
		
	}
}