using UnityEngine;
using System.Collections;

class DisplayStrings
{
#if (UNITY_IPHONE || UNITY_ANDROID)
	public static string restartText = "Tap to Restart";
#else
	public static string restartText = "Press 'R' to Restart";
#endif
	public static string gameOverText = "Game Over!";
	public static string scoreText = "Score: ";
}

public class GameController : MonoBehaviour {

	public GameObject hazard;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	public GUIText scoreText;
	public GUIText gameOverText;
	public GUIText restartText;
	
	private bool gameOver;
	private bool restart;
	private int score;

	void Start ()
	{
		// Clear everything

		gameOver = false;
		restart = false;
		restartText.text = "";
		gameOverText.text = "";
		score = 0;

		UpdateScore ();
		StartCoroutine( SpawnWaves () );
	}

	void Update ()
	{
		if (restart)
		{
#if (UNITY_IPHONE || UNITY_ANDROID)
			if (Input.touchCount > 0)
			{
				Application.LoadLevel (Application.loadedLevel);
			}
#else
			if (Input.GetKeyDown (KeyCode.R))
			{
				Application.LoadLevel (Application.loadedLevel);
			}
#endif
		}
	}

	IEnumerator SpawnWaves()
	{
		yield return new WaitForSeconds (startWait);

		while (true) {
			for (int i = 0; i < Random.Range((int)(hazardCount/2), hazardCount); i++)
			{
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);

			if (gameOver)
			{
				restartText.text = DisplayStrings.restartText;
				restart = true;
				break;
			}		
		}
	}

	public void AddScore(int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}

	void UpdateScore()
	{
		scoreText.text = DisplayStrings.scoreText + score;
	}

	public void GameOver ()
	{
		gameOverText.text = DisplayStrings.gameOverText;
		gameOver = true;
	}

}
