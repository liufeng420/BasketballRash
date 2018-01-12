/******************************************************************************
 * Spine Runtimes Software License v2.5
 *
 * Copyright (c) 2013-2016, Esoteric Software
 * All rights reserved.
 *
 * You are granted a perpetual, non-exclusive, non-sublicensable, and
 * non-transferable license to use, install, execute, and perform the Spine
 * Runtimes software and derivative works solely for personal or internal
 * use. Without the written permission of Esoteric Software (see Section 2 of
 * the Spine Software License Agreement), you may not (a) modify, translate,
 * adapt, or develop new applications using the Spine Runtimes or otherwise
 * create derivative works or improvements of the Spine Runtimes or (b) remove,
 * delete, alter, or obscure any trademarks or any copyright, trademark, patent,
 * or other intellectual property or proprietary rights notices on or in the
 * Software, including any copy thereof. Redistributions in binary or source
 * form must include this license and terms.
 *
 * THIS SOFTWARE IS PROVIDED BY ESOTERIC SOFTWARE "AS IS" AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
 * MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO
 * EVENT SHALL ESOTERIC SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES, BUSINESS INTERRUPTION, OR LOSS OF
 * USE, DATA, OR PROFITS) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER
 * IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 *****************************************************************************/

// Contributed by: Mitch Thompson

using UnityEngine;
using Spine.Unity;
using BasketballRash;
using UnityEngine.Networking;

[RequireComponent(typeof(CharacterController))]
public class BasicPlatformerController : NetworkBehaviour
{

	[Header("Controls")]
	public string XAxis = "Horizontal";
	public string YAxis = "Vertical";
	public string JumpButton = "Jump";

	[Header("Moving")]
	public float walkSpeed = 6f;
	public float runSpeed = 10f;
	public float gravityScale = 6.6f;
    public float jumpSpeedFix = 4f; //起跳后反向速度修正系数
	public Transform shadow;

	public float walkToRun = 0.6f;

	[Header("Jumping")]
	public float jumpSpeed = 25;
	public int	 jumpCountLimits = 2;


	[Header("Attacking")]
	public float attackDuration = 0.5f;
	public int shootRate = 99;


	[Header("Graphics")]
	public SkeletonAnimation skeletonAnimation;

	[Header("Animation")]
	[SpineAnimation(dataField: "skeletonAnimation")]
	public string walkName = "Walk";
	[SpineAnimation(dataField: "skeletonAnimation")]
	public string runName = "Run";
	[SpineAnimation(dataField: "skeletonAnimation")]
	public string idleName = "Idle";
	[SpineAnimation(dataField: "skeletonAnimation")]
	public string jumpName = "Jump";
	[SpineAnimation(dataField: "skeletonAnimation")]
	public string fallName = "Fall";
	[SpineAnimation(dataField: "skeletonAnimation")]
	public string attackName = "Attack";
	[SpineAnimation(dataField: "skeletonAnimation")]
	public string beattackName = "Beattack";
	[Header("Effects")]
	public AudioSource jumpAudioSource;
	public AudioSource hardfallAudioSource;
	public AudioSource footstepAudioSource;
	public ParticleSystem landParticles;
	[SpineEvent]
	public string footstepEventName = "Footstep";
	CharacterController controller;
	Vector3 velocity = default(Vector3);


	int jumpCounts = 0;
	Vector2 input;


	PlayerActions tempActions;

	Vector3 jumpVelocity = default(Vector3);
	Vector3 lastVelocity = default(Vector3);

	bool doingAttack = false;	// 是否正在攻击
	float attackEndTime;    // 攻击结束时间
	private GameObject basketBall;
	private Spine.AnimationState spineAnimationState;
	void Awake()
	{
		controller = GetComponent<CharacterController>();
	}

	void Start()
	{
		skeletonAnimation.AnimationState.Event += HandleEvent;
		spineAnimationState = skeletonAnimation.AnimationState;
		tempActions = PlayerActions.CreateWithDefaultBindings();
		Physics.IgnoreLayerCollision(gameObject.layer, gameObject.layer);

		// basket ball
		basketBall = GameObject.Find("BasketBall");
		Collider c2 = basketBall.GetComponent<Collider>();
		Physics.IgnoreCollision(controller, c2);
	}

	void HandleEvent(Spine.TrackEntry trackEntry, Spine.Event e)
	{
		if (e.Data.Name == footstepEventName)
		{
			footstepAudioSource.Stop();
			footstepAudioSource.pitch = GetRandomPitch(0.2f);
			footstepAudioSource.Play();
		}
	}

	static float GetRandomPitch(float maxOffset)
	{
		return 1f + Random.Range(-maxOffset, maxOffset);
	}

	void doJump()
	{
		jumpCounts++;
		jumpAudioSource.Stop();
		jumpAudioSource.Play();
		velocity.y = jumpSpeed;
	}

	void doAttack()
	{
		attackEndTime = Time.time + attackDuration;
		doingAttack = true;
		spineAnimationState.SetAnimation(0, attackName, false);
		spineAnimationState.AddAnimation(0, idleName, true, 0);

		//skeletonAnimation
	}

	void doShoot(float t)
	{
		basketBall.GetComponent<Ball>().Shoot(t, shootRate, skeletonAnimation.Skeleton.FlipX);
	}
	void FixedUpdate()
	{

		input.x = tempActions.Move.X;
		input.y = tempActions.Move.Y;
        // input.x = Input.GetAxis(XAxis);
        // input.y = Input.GetAxis(YAxis);
        
		if (controller.isGrounded)
		{
			velocity.x = 0;
 			velocity.z = 0;
			//velocity.y = 0;
		}

		float dt = Time.fixedDeltaTime;
        if (input.x != 0)
        {
            if (!controller.isGrounded)
            {
                if (Mathf.Abs(jumpVelocity.x) < 0.001f)
				{
					velocity.x = jumpSpeedFix * Mathf.Sign(input.x);
				}
				else
				{
                    if (Mathf.Sign(input.x) == Mathf.Sign(jumpVelocity.x))
                    {
                        velocity.x = jumpVelocity.x;
                    }
                    else
                    {
                        velocity.x = (Mathf.Abs(jumpVelocity.x) - jumpSpeedFix) * Mathf.Sign(jumpVelocity.x);
                    }
				}
            }
            else
            {
                velocity.x = Mathf.Abs(input.x) > walkToRun ? runSpeed : walkSpeed;
                velocity.x *= Mathf.Sign(input.x);
            }
        }

        if (input.y != 0)
        {
            if(!controller.isGrounded)
            {
				if (Mathf.Abs(jumpVelocity.z) < 0.001f)
				{
					// 原先没有输入
					velocity.z = jumpSpeedFix * Mathf.Sign(input.y);
				}
				else
				{
                    if (Mathf.Sign(input.y) == Mathf.Sign(jumpVelocity.z))
                    {
                        velocity.z = jumpVelocity.z;
                    }
                    else
                    {
                        velocity.z = (Mathf.Abs(jumpVelocity.z) - jumpSpeedFix) * Mathf.Sign(jumpVelocity.z);
                    }
				}
            }
            else
            {
                velocity.z = Mathf.Abs(input.y) > walkToRun ? runSpeed : walkSpeed;
                velocity.z *= Mathf.Sign(input.y);
            }
        }

        //if (input.x != 0)
        //{
        //    if (!controller.isGrounded)
        //    {
        //        float speed = Mathf.Abs(input.x) > walkToRun ? runSpeed : walkSpeed;

        //        if (Mathf.Sign(input.x) ==  Mathf.Sign(velocity.x))
        //        {
        //            velocity.x = speed * Mathf.Sign(velocity.x);
        //        }
        //        else
        //        {
        //            velocity.x = speed * jumpSpeedFix * Mathf.Sign(velocity.x);
        //        }
        //    }
        //    else
        //    {
        //        velocity.x = Mathf.Abs(input.x) > walkToRun ? runSpeed : walkSpeed;
        //        velocity.x *= Mathf.Sign(input.x);
        //    }
        //}
        //if (input.y != 0)
        //{
        //    if(!controller.isGrounded)
        //    {
        //        float speed = Mathf.Abs(input.y) > walkToRun ? runSpeed : walkSpeed;
        //        if (Mathf.Sign(input.y) == Mathf.Sign(velocity.z))
        //        {
        //            velocity.z = speed * Mathf.Sign(velocity.z);
        //        }
        //        else
        //        {
        //            velocity.z = speed * jumpSpeedFix * Mathf.Sign(velocity.z);
        //        }
        //    }
        //    else
        //    {
        //        velocity.z = Mathf.Abs(input.y) > walkToRun ? runSpeed : walkSpeed;
        //        velocity.z *= Mathf.Sign(input.y);
        //    }
        //}


        var gravityDeltaVelocity = Physics.gravity * gravityScale * dt;

		if (controller.isGrounded)
		{
		}
		else
		{
			{
				velocity += gravityDeltaVelocity;
			}
		}
		// JUMP 
		if (tempActions.Jump.WasPressed)
		{
			// if (Input.GetButtonDown(JumpButton))
			if (controller.isGrounded)
			{
				jumpCounts = 0;
			}

			if (jumpCounts < jumpCountLimits)
			{
                if (input.x != 0)
                {
					jumpVelocity.x = Mathf.Abs(input.x) > walkToRun ? runSpeed : walkSpeed;
                    jumpVelocity.x *= Mathf.Sign(input.x);
                }
				else
				{
					jumpVelocity.x = 0;
					if (jumpCounts > 0)
					{
						velocity.x = 0;
					}
				}
				if (input.y != 0)
				{
					jumpVelocity.z = Mathf.Abs(input.y) > walkToRun ? runSpeed : walkSpeed;
                    jumpVelocity.z *= Mathf.Sign(input.y);
				}
				else
				{
					jumpVelocity.z = 0;
					if (jumpCounts > 0)
					{
						velocity.z = 0;
					}
				}
                
				doJump();
			}
		}

		if (tempActions.Fire.IsPressed)
		{
			doAttack();
		}

		if (tempActions.Shoot.IsPressed)
		{
			doShoot(0.5f);
		}


		if (lastVelocity.x != velocity.x || lastVelocity.z != velocity.z)
		{
			Debug.Log("x:" + velocity.x + " y:" + velocity.y + " z:" + velocity.z);
			lastVelocity = velocity;
		}
		controller.Move(velocity * dt);


		if (controller.isGrounded)
		{
            // Attack
            if (doingAttack)
            {
				if (Time.time > attackEndTime)
				{
					doingAttack = false;
				}
            }
			else
			{
                if (input.x == 0 && input.y == 0)
                {
                    skeletonAnimation.AnimationName = idleName;
                }
                else
                {
                    bool bRun = Mathf.Abs(input.x) > walkToRun || Mathf.Abs(input.y) > walkToRun;
                    skeletonAnimation.AnimationName = bRun ? runName : walkName;
                    // footstepAudioSource.Play();
                    //landParticles.Emit((int)(velocity.y / -9f) + 5);
                }
			}
		}
		else
		{
			if (velocity.y < 0)
			{
				skeletonAnimation.AnimationName = fallName;
			}
			else
			{
				skeletonAnimation.AnimationName = jumpName;
			}
		}

		if (input.x != 0)
			skeletonAnimation.Skeleton.FlipX = input.x < 0;

		Vector3 v = shadow.position;
		v.y = 0.1f;
		shadow.position = v;

	}
}