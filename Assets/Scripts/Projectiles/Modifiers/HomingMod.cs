﻿using UnityEngine;
using System.Collections;

public class HomingMod : ProjectileMod
{
	#region Fields
	public float homingTime = 0f;
	public float homingLength = 0f;
	[Range(0f, 1f)]
	public float homingSpeed = 0.5f;

	private float homingTimer = 0f;
	#endregion

	#region Public Methods
	public override void ApplyModifier()
	{
		homingTimer += Time.deltaTime;

		if (homingTimer >= homingTime && homingTimer <= homingTime + homingLength)
		{
			Vector2 targetDirection = transform.position.LookAt2D(PlayerControl.Instance.collider2D.bounds.center) * Vector3.right;
			thisProjectile.direction = Vector3.Lerp(thisProjectile.direction, targetDirection, homingSpeed * 60f * Time.deltaTime).normalized;
		}
	}
	#endregion
}
