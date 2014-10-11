﻿using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour 
{
	public enum Difficulty
	{
		Easy,
		Normal,
		Difficult,
		Brutal,
		Insane,
		Boss
	};

	public Difficulty difficulty = Difficulty.Easy;
	public float maxHealth = 10f;
	public float damage = 5f;
	public float knockback = 3f;
	public Color flashColor = new Color(1f, 0.47f, 0.47f, 1f);
	public float flashLength = 0.1f;
	public float gravity = -35f;
	public float moveSpeed = 5f;
	public float groundDamping = 10f;
	public float inAirDamping = 5f;

	[HideInInspector]
	public float health;

	protected bool right = false;
	protected bool left = false;
	protected bool invincible = false;

	[HideInInspector]
	protected float normalizedHorizontalSpeed = 0;

	private SpriteRenderer spriteRenderer;
	private ExplodeEffect explodeEffect;

	protected CharacterController2D controller;
	protected Animator anim;
	protected Vector3 velocity;
	protected Transform frontCheck;
	protected PlayerControl playerControl;

	protected virtual void Awake()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		explodeEffect = GetComponent<ExplodeEffect>();
		anim = GetComponent<Animator>();
		controller = GetComponent<CharacterController2D>();
		frontCheck = transform.FindChild("frontCheck");
		playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();

		health = maxHealth;
	}

	void OnTriggerEnter2D(Collider2D enemy)
	{
		if (enemy.tag == "PlayerProjectile")
		{
			if (health > 0f)
			{
				TakeDamage(enemy.gameObject);
			}
		}
	}

	void TakeDamage(GameObject enemy)
	{
		float damage = enemy.GetComponent<Projectile>().damage;
		float knockback = enemy.GetComponent<Projectile>().knockback;
		enemy.GetComponent<Projectile>().CheckDestroy();

		if (!invincible)
		{
			health -= damage;

			if (health <= 0f)
			{
				explodeEffect.Explode(velocity, spriteRenderer.sprite);
				playerControl.AddPointsFromEnemy(maxHealth, damage);
				Destroy(gameObject);

			}
			else
			{
				velocity.x = Mathf.Sqrt(Mathf.Pow(knockback, 2) * -gravity);
				velocity.y = Mathf.Sqrt(knockback * -gravity);

				if (transform.position.x - enemy.transform.position.x < 0)
				{
					velocity.x *= -1;
				}

				if (velocity.x > 0 || velocity.y > 0)
				{
					controller.move(velocity * Time.deltaTime);
				}

				spriteRenderer.color = flashColor;
				Invoke("ResetColor", flashLength);
			}
		}
	}

	protected void InitialUpdate()
	{
		velocity = controller.velocity;

		if (controller.isGrounded)
		{
			velocity.y = 0;
		}
	}

	protected void GetMovement()
	{
		if (right)
		{
			normalizedHorizontalSpeed = 1;

			if (transform.localScale.x < 0f)
			{
				Flip();
			}
		}
		else if (left)
		{
			normalizedHorizontalSpeed = -1;

			if (transform.localScale.x > 0f)
			{
				Flip();
			}
		}
		else
		{
			normalizedHorizontalSpeed = 0;
		}
	}

	protected void ApplyMovement()
	{
		float smoothedMovementFactor = controller.isGrounded ? groundDamping : inAirDamping;

		velocity.x = Mathf.Lerp(velocity.x, normalizedHorizontalSpeed * moveSpeed, Time.fixedDeltaTime * smoothedMovementFactor);
		velocity.y += gravity * Time.fixedDeltaTime;

		controller.move(velocity * Time.fixedDeltaTime);
	}

	protected void CheckFrontCollision()
	{
		Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position);

		foreach (Collider2D hit in frontHits)
		{
			if (hit.tag == "Obstacle" || hit.tag == "WorldBoundaries")
			{
				Flip();

				right = !right;
				left = !right;
				break;
			}
		}
	}

	protected void Flip()
	{
		transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}

	void ResetColor()
	{
		spriteRenderer.color = Color.white;
	}
}
