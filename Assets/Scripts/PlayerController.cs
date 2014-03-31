using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
	// Movement
	public float speed;
	public float tilt;
	public Boundary boundary;

	// Combat + Firing
	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	public float nextFire;

	// Update Methods

	void Update()
	{
#if (UNITY_IPHONE || UNITY_ANDROID)
		if (Input.touchCount > 1)
		{
			RequestFireShot();
		}
#else
		if (Input.GetButton ("Fire1"))
		{
			RequestFireShot();
		}
#endif
	}
	
	void FixedUpdate()
	{
		float moveHorizontal = 0;
		float moveVertical = 0; 

#if (UNITY_IPHONE || UNITY_ANDROID)
		if (Input.touchCount > 0)
		{
			int i = 0;
			while (i < Input.touchCount)
			{
				if (moveHorizontal == 0 && (Input.GetTouch(i).deltaPosition.x < -5 || Input.GetTouch(i).deltaPosition.x > 5))
				{
					moveHorizontal = Input.GetTouch(i).deltaPosition.normalized.x;
				}
				if (moveVertical == 0 && (Input.GetTouch(i).deltaPosition.y < -5 || Input.GetTouch(i).deltaPosition.y > 5))
				{
					moveVertical = Input.GetTouch(i).deltaPosition.normalized.y;
				}
				i++;
			}
		}
#else
		moveHorizontal = Input.GetAxis ("Horizontal");
		moveVertical = Input.GetAxis ("Vertical");
#endif

		HandleMovement(moveHorizontal, moveVertical);
	}

	// Movement Methods

	void HandleMovement(float moveHorizontal, float moveVertical)
	{
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rigidbody.velocity = movement * speed;
		
		rigidbody.position = new Vector3
			(
				Mathf.Clamp (rigidbody.position.x, boundary.xMin, boundary.xMax),
				0.0f,
				Mathf.Clamp (rigidbody.position.z, boundary.zMin, boundary.zMax)
				);
		
		rigidbody.rotation = Quaternion.Euler (0.0f, 0.0f, rigidbody.velocity.x * -tilt);
	}
	
	// Combat Methods
	
	void RequestFireShot()
	{
		if (Time.time > nextFire)
		{
			FireShot();
		}
	}
	
	void FireShot()
	{
		nextFire = Time.time + fireRate;
		Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
		audio.Play ();
	}

}
