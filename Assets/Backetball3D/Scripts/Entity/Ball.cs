using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DisRate
{
	public float	dis;//离篮框的距离
	public int		rate;
}

public class Ball : MonoBehaviour {


	[Header("Ball Moving Limit")]
	public float RateKMin = 1;
	public float RateKMax = 10;            // 斜率K
	public float ErrorRadiusXZ = 2;
	public float NetEffect = 0.5f;
	[Header("Shoot Rate")]
	public int MissDegreeMin = -90;
	public int MissDegreeMax = 90;
	public DisRate[] DisRateAry;

   public enum BallStateEnum
	{
		Free,
		Play,	
	};
	private BallStateEnum ballState;

	public BallStateEnum BallState
	{
		get
		{
			return ballState;
		}
		set
		{
			if (value == BallStateEnum.Free)
			{
				rigibody.useGravity = true;
			}
			else
			{
				rigibody.useGravity = false;
				rigibody.velocity = Vector3.zero;
			}
			ballState = value;
		}
	}

	public void Shoot(float weightRateK, int successRate, bool isLeft)
	{
		if (BallState == BallStateEnum.Play)
		{
			BallState = BallStateEnum.Free;
			player = null;
			Vector3 desPos = isLeft ? basketRingL.transform.position : basketRingR.transform.position;
			float dis = Mathf.Abs(desPos.x - transform.position.x);
			for (int i = 0; i < DisRateAry.Length; ++i )
			{
				if (dis <= DisRateAry[i].dis || i == DisRateAry.Length)
				{
					successRate += DisRateAry[i].rate;
					break;
				}
			}
			bool success = Random.Range(1, 100) <= successRate;
			if (success == false)
			{
				float degree = Random.Range(MissDegreeMin, MissDegreeMax);
				desPos.z += Mathf.Sin(Mathf.Deg2Rad * degree);
				desPos.x += Mathf.Cos(Mathf.Deg2Rad * degree);
			}
			float time = CalcTime(desPos, transform.position, weightRateK);
			Vector3 speed = CalucSpeed(desPos, transform.position, time);
			rigibody.velocity = speed;
		}	 
	}
							
	private Rigidbody rigibody;		
	private GameObject basketRingL;
	private GameObject basketRingR;
	private GameObject player;
	// Use this for initialization
	void Start () {
		rigibody = GetComponent<Rigidbody>();
		basketRingL = GameObject.Find("BasketRingLeft");
		basketRingR = GameObject.Find("BasketRingRight");
	}

	private void OnCollisionEnter(Collision collision)
	{  
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			if (BallState == BallStateEnum.Free)
			{
				BallState = BallStateEnum.Play;		 
				player = other.gameObject;
			}

		}
		else if(other.gameObject.tag == "HoopL")
		{
			rigibody.velocity *= NetEffect;
		}
		else if(other.gameObject.tag == "HoopR")
		{
			rigibody.velocity *= NetEffect;
		}
	}

	private void FixedUpdate()
	{
		switch (BallState)
		{
			case BallStateEnum.Free:
				{
					stopBall();
				}
				break;
			case BallStateEnum.Play:
				{
					tracePlayer();
				}
				break;
		}



	}
	private Vector3 _smoothDampVelocity;
	private Vector3 _smoothDampVelocity1;
	private void stopBall()
	{
		if (transform.position.y < 0.6)
		{
			float x = Mathf.Abs(rigibody.velocity.x);
			float y = Mathf.Abs(rigibody.velocity.y);
			float z = Mathf.Abs(rigibody.velocity.z);
			if (x < 3 &&
				y < 3 &&
				z < 3)
			{

				if (x < 0.05 && y< 0.05 && z < 0.05)
				{
					rigibody.angularVelocity = Vector3.SmoothDamp(rigibody.velocity, Vector3.zero, ref _smoothDampVelocity, 0.2f);
					rigibody.velocity = Vector3.zero;
				}
				else
				{
					rigibody.velocity = Vector3.SmoothDamp(rigibody.velocity, Vector3.zero, ref _smoothDampVelocity1, 0.4f);
				}
			}
		}
	}
	private Vector3 CalucSpeed(Vector3 posDes, Vector3 posSrc, float time)
	{
		Vector3 speed;
		speed = (posDes - posSrc)/time;	   
		speed.y = speed.y - time * Physics.gravity.y/2;				
		return speed;
	}

	private float CalcTime(Vector3  posDes, Vector3 posSrc, float timeWeight)
	{
		Vector3 dis = posSrc - posDes;
		dis.x = Mathf.Abs(dis.x);
		dis.y = Mathf.Abs(dis.y);
		dis.z = Mathf.Abs(dis.z);
		float dd = dis.x > dis.z ? dis.x : dis.z;
		float timeMin = Mathf.Sqrt(2 * (dd*RateKMin + dis.y ) / Mathf.Abs(Physics.gravity.y));
		float timeMax = Mathf.Sqrt(2 * (dd * RateKMax + dis.y) / Mathf.Abs(Physics.gravity.y));
		float ret = Mathf.Lerp(timeMin, timeMax, timeWeight);
		return ret;
	}

	private void tracePlayer()
	{
		if (player)
		{
			rigibody.transform.position = player.transform.position+ new Vector3(0, 1, 0);	

		}
	}
}
