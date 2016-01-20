using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
// DEPRECATED -- USE "STATS" AT TOP OF GAME VIEW TO VIEW FPS
//public class DisplayFrameRate : MonoBehaviour {
//
//	public Text fpsText;
//
//	// Use this for initialization
//	void Start () {
//		fpsText.text = "0";
//	}
//	
//	int m_frameCounter = 0;
//	float m_timeCounter = 0.0f;
//	float m_lastFramerate = 0.0f;
//	public float m_refreshTime = 0.5f;
//	
//	void Update()
//	{
//		if( m_timeCounter < m_refreshTime )
//		{
//			m_timeCounter += Time.deltaTime;
//			m_frameCounter++;
//		}
//		else
//		{
//			//This code will break if you set your m_refreshTime to 0, which makes no sense.
//			m_lastFramerate = (float)m_frameCounter/m_timeCounter;
//			m_frameCounter = 0;
//			m_timeCounter = 0.0f;
//		}
//		fpsText.text = "FPS: " + Math.Round(m_lastFramerate, 2);
//	}
//
//}
