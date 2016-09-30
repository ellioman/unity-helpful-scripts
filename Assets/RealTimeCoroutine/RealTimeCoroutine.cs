using UnityEngine;
using System.Collections;

public class RealTimeCoroutine : MonoBehaviour
{
	[SerializeField] private TextMesh timeScaleTextMesh;
	[SerializeField] private TextMesh normalTimeTextMesh;
	[SerializeField] private TextMesh realTimeTextMesh;

	private float updateTextInterval = 0.01f;

	private void Start()
	{
		StartCoroutine(ShowTimeScale());
		StartCoroutine(ShowNormalTime());
		StartCoroutine(ShowRealTime());
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Space))
		{
			Time.timeScale = (Time.timeScale == 0f) ? 1f : 0f;
		}
	}

	private IEnumerator ShowTimeScale()
	{
		while(true)
		{
			timeScaleTextMesh.text = Time.timeScale.ToString();
			yield return WaitForRealTime(updateTextInterval);
		}
	}

	private IEnumerator ShowNormalTime()
	{
		while(true)
		{
			normalTimeTextMesh.text = Time.time.ToString();
			yield return new WaitForSeconds(updateTextInterval);
		}
	}

	private IEnumerator ShowRealTime()
	{
		float startTime = Time.realtimeSinceStartup;
		while(true)
		{
			realTimeTextMesh.text = (Time.realtimeSinceStartup - startTime).ToString();
			yield return WaitForRealTime(updateTextInterval);
		}
	}

	public static IEnumerator WaitForRealTime(float delay)
	{
		while(true)
		{
			float pauseEndTime = Time.realtimeSinceStartup + delay;
			while (Time.realtimeSinceStartup < pauseEndTime)
			{
				yield return 0;
			}
			break;
		}
	}
}
