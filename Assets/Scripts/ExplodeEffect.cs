﻿using UnityEngine;
using System.Collections;

public class ExplodeEffect : MonoBehaviour 
{
	public GameObject pixelPrefab;

	private Sprite sprite;
	private Vector2 colliderSize;

	public void Explode(Vector3 velocity, Vector2 colliderSize)
	{
		sprite = GetComponentInChildren<SpriteRenderer>().sprite;

		for (int i = 0; i <= colliderSize.x * 10f; i++)
		{
			for (int j = 0; j <= colliderSize.y * 10f; j++)
			{
				Vector3 pixelPosition = transform.TransformPoint((i / 10f) - (colliderSize.x / 2f), (j / 10f) + 0.05f, 0);

				Color pixelColor;

				pixelColor = sprite.texture.GetPixel((int)sprite.rect.x + i, 
													 (int)sprite.rect.y + j);
				
				if (pixelColor.a != 0f)
				{
					GameObject pixelInstance = Instantiate(pixelPrefab, pixelPosition, Quaternion.identity) as GameObject;
					pixelInstance.GetComponent<SpriteRenderer>().color = pixelColor;
					pixelInstance.rigidbody2D.AddForce(new Vector2(((velocity.x * 10f) + Random.Range(-250, 250)), 
																   ((velocity.y * 10f) + Random.Range(-250, 300))));
				}
			}
		}
	}
}
