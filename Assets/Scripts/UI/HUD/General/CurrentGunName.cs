﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class CurrentGunName : MonoBehaviour
{
	#region Fields
	private static CurrentGunName instance;

	public float showTime = 1f;

	private bool showing = false;
	private Text text;
	#endregion

	#region Public Properties
	public static CurrentGunName Instance
	{
		get { return instance; }
	}
	#endregion

	#region MonoBehaviour
	private void Awake()
	{
		instance = this;

		text = GetComponent<Text>();
	}
	#endregion

	#region Public Methods
	public void Show(string gunName, Color gunColor, float time = 0f)
	{
		if (showing)
		{
			DOTween.Complete(text);
		}

		time = (time == 0f) ? showTime : time;
		text.text = gunName.ToUpper();
		text.color = new Color(gunColor.r, gunColor.g, gunColor.b, 0f);
		text.DOFade(1f, time * 0.15f)
			.SetEase(Ease.InQuad);
		text.DOFade(0f, time * 0.15f)
			.SetDelay(time * 0.85f)
			.SetEase(Ease.OutQuad)
			.OnComplete(() => showing = false);

		showing = true;
	}
	#endregion
}
