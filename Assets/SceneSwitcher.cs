using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneSwitcher : MonoBehaviour
{
	public void Engaged()
	{
		SceneManager.LoadScene("EngagedView");
	}
	public void Multi()
	{
		SceneManager.LoadScene("MultimodalityView");
	}
	public void Juxta()
	{
		SceneManager.LoadScene("JuxtapositionView");
	}
	public void exitgame()
	{
		Debug.Log("exitgame");
		Application.Quit();
	}
}