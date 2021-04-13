﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{


	private static T m_Instance = null;

	public static T instance {
		get {
			if (m_Instance == null) {
				m_Instance = GameObject.FindObjectOfType (typeof(T)) as T;
				if (m_Instance == null) {
					m_Instance = new GameObject ("Singleton of " + typeof(T).ToString (), typeof(T)).GetComponent<T> ();
					m_Instance.Init ();
				}

			}
			return m_Instance;
		}
	}

	private void Awake ()
	{

		if (m_Instance == null) {
			m_Instance = this as T;
		}
		DontDestroyOnLoad (gameObject);
	}

	public virtual void Init ()
	{
	}


	private void OnApplicationQuit ()
	{
		m_Instance = null;
	}

}