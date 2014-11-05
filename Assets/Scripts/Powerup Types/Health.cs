﻿using UnityEngine;
using System.Collections;

public class Health : Powerup
{
	public float minHealth = 5f;
	public float maxHealth = 100f;
	public Sprite popupImage;

	private float healthAmount;

	new void Awake()
	{
		base.Awake();

		healthAmount = Random.Range(minHealth, maxHealth) * Random.Range(1f, 10f);
		healthAmount /= 10;
		healthAmount = Mathf.RoundToInt(Mathf.Clamp(healthAmount, minHealth, maxHealth));
	}

	new void OnTriggerEnter2D(Collider2D trigger)
	{
		base.OnTriggerEnter2D(trigger);
	}

	protected override void Pickup()
	{
		player.AddHealth(healthAmount);
		PopupMessage.CreatePopup(player.popupMessagePoint.position, ((int)healthAmount).ToString(), popupImage);

		base.Pickup();
	}
}
