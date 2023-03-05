﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	float xSpeed;
	[SerializeField]
	float jump;
	[SerializeField]
	float timeSpawn;
	[SerializeField]
	Transform bulletPositionSpawn;
	[SerializeField]
	GameObject bulletObject;

	Rigidbody2D rigid;
	SpriteRenderer sprite;
	Animator animat;
	bool isJump;
	bool isShooting;

	// Start is called before the first frame update
	void Start()
	{
		rigid = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer>();
		animat = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		// left = -1, right = 1, default = 0
		float xDirection = Input.GetAxisRaw("Horizontal");
		// move left, right
		if (xDirection < 0 || xDirection > 0)
		{
			if (xDirection < 0)
			{
				sprite.flipX = true;
				float xPos = bulletPositionSpawn.localPosition.x > 0 ? bulletPositionSpawn.localPosition.x * -1 : bulletPositionSpawn.localPosition.x;
				bulletPositionSpawn.localPosition = new Vector3(xPos, bulletPositionSpawn.localPosition.y, bulletPositionSpawn.localPosition.z);
			}
			else
			{
				sprite.flipX = false;
				float xPos = bulletPositionSpawn.localPosition.x > 0 ? bulletPositionSpawn.localPosition.x : bulletPositionSpawn.localPosition.x * -1;
				bulletPositionSpawn.localPosition = new Vector3(xPos, bulletPositionSpawn.localPosition.y, bulletPositionSpawn.localPosition.z);
			}
			transform.position += Vector3.right * xDirection * xSpeed * Time.deltaTime;
			animat.SetInteger("playerAni", 1);
		}
		// jump
		if (Input.GetKeyDown(KeyCode.Space) && !isJump)
		{
			rigid.AddForce(Vector2.up * jump);
			isJump = true;
		}
		if (isJump && xDirection != 0)
		{
			animat.SetInteger("playerAni", 2);
		}
		if (xDirection == 0 && !isJump)
		{
			animat.SetInteger("playerAni", 0);
		}
		// shooting
		if (Input.GetKeyDown(KeyCode.R) && !isShooting)
		{
			isShooting = true;
			if (sprite.flipX)
			{
				Instantiate(bulletObject, bulletPositionSpawn.position, Quaternion.Euler(new Vector3(0, 0, -90)));
			}
			else
			{
				Instantiate(bulletObject, bulletPositionSpawn.position, Quaternion.Euler(new Vector3(0, 0, 90)));
			}
			Debug.Log("Ban da ban");
			StartCoroutine(ShootingCountDown(timeSpawn));
		}

	}

	IEnumerator ShootingCountDown(float time)
	{
		yield return new WaitForSeconds(time);
		isShooting = false;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			isJump = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("BulletEnemy"))
		{
			Debug.Log("Dính đạn địch");
			Destroy(collision.gameObject);
		}
		else if (collision.gameObject.CompareTag("Enemy"))
		{
			Debug.Log("Đã va chạm với địch");

		}
		else if (collision.gameObject.CompareTag("DeathZone"))
		{
			Debug.Log("Đã va chạm với deathzone");
		}
	}
}
