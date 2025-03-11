using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;

public class PlayerMovement : MonoBehaviour
{
	private const float MAX_SPEED = 0.10f;
	private const float MIN_SPEED = -0.10f;

	private int tempTestCounter = 0;
	private InputAction moveAction;
	//Vector2 for x and y acceleration
	private Vector3 speed = new Vector3(0f, 0f, 0f);
	//Vector2 for seed offset to prevent instantiating a new Vector2 every FixedUpdate
	private Vector3 addedSpeed = new Vector3(0f, 0f, 0f);

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
			this.addedSpeed.x = xAdjust * 0.01f;
			this.addedSpeed.y = yAdjust * 0.01f;

			//Do this here so we're not adding vectors every fixed update
			this.speed += addedSpeed;
			//Clamp speed to ensure we don't get too zoomy
			this.speed.x = math.clamp(this.speed.x, MIN_SPEED, MAX_SPEED);
			this.speed.y = math.clamp(this.speed.y, MIN_SPEED, MAX_SPEED);
		}
		else if(this.speed.x != 0f || this.speed.y != 0f) {
			//Manual check if speed is close enough to 0 to quell it. The code below was having issues where speed would persist
			//at 0.01 or -0.01, causing the player to drift off.
			if (math.abs(this.speed.x) <= 0.1f && math.abs(this.speed.x) > 0.0f)
			{
				this.speed.x = 0;
			}
			if (math.abs(this.speed.y) <= 0.1f && math.abs(this.speed.y) > 0.0f)
			{
				this.speed.y = 0;
			}
			
			//Short if statement format: boolean ? do if true : do if false;
			//The below lines do the following:
			//If this.speed.x/y is 0, assign it to 0. Otherwise, assign it to -0.01 if it's positive or 0.01 if it's negative.
			this.addedSpeed.x = this.speed.x == 0.0f ? 0.0f : this.speed.x < 0f ? 0.01f : -0.01f;
			this.addedSpeed.y = this.speed.y == 0.0f ? 0.0f : this.speed.y < 0f ? 0.01f : -0.01f;

			//This whole else if section is to bring speed back to 0.0f when the player stops moving.
			this.speed += this.addedSpeed;
			this.addedSpeed.x = 0.0f;
			this.addedSpeed.y = 0.0f;
		}

		//if (tempTestCounter++ % 100 == 0) //This is only here and used to avoid printing 50 times per second.
		//{
		//	Debug.Log(this.addedSpeed + ", " + this.speed);
		//}

		this.gameObject.transform.position = this.gameObject.transform.position + this.speed;
	}
}