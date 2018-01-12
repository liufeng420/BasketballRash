using UnityEngine;
using System.Collections;



public class SmoothFollow3D : MonoBehaviour
{
	public Transform target;					   
	public float smoothDampTime = 0.2f;
	[HideInInspector]
	public Vector3 cameraOffset;
	public Vector3 min;
	public Vector3 max;
	public bool useFixedUpdate = false;


	//private Vector3 _smoothDampVelocity;
	private Vector3 targetLastPos;
	
	void Awake()
	{
		Application.targetFrameRate = 50;						 
	}

	void LateUpdate()
	{
		if ( !useFixedUpdate )
			updateCameraPosition();
	}

	void FixedUpdate()
	{
		if( useFixedUpdate )
			updateCameraPosition();
	}


	void updateCameraPosition()
	{
		//Debug.Log("deltaV:" + (target.position - transform.position));
		if (targetLastPos == target.position)
		{
			return;
		}
		targetLastPos = target.position;
		float zShowY = target.position.z * Mathf.Sin( transform.rotation.eulerAngles.x * Mathf.Deg2Rad );

		Vector3 goalPoint = target.position + cameraOffset;
		goalPoint.y += zShowY;
		goalPoint.x = Mathf.Clamp(goalPoint.x, min.x, max.x);
		goalPoint.y = Mathf.Clamp(goalPoint.y, min.y, max.y);
		goalPoint.z = transform.position.z;// z no need to move
		transform.position = goalPoint;

		Debug.Log("Update:" + (target.position));
		//transform.position = Vector3.SmoothDamp(transform.position, goalPoint, ref _smoothDampVelocity, smoothDampTime);
		//if (playerController.velocity.x > 0)
		//{
		//	transform.position = Vector3.SmoothDamp(transform.position, goalPoint, ref _smoothDampVelocity, smoothDampTime);
		//}
		//else
		//{
		//	var leftOffset = cameraOffset;
		//	leftOffset.x *= -1;
		//	transform.position = Vector3.SmoothDamp(transform.position, goalPoint, ref _smoothDampVelocity, smoothDampTime);
		//}
		//if (testFixed % 5 == 0)
		//{
		//	Debug.Log("Late:" + testLate+"Fiexed:"+testFixed+"Update:"+ testUpdate);
		//}
	}
}
