using System;
using Saving;
using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// manages adding to the scores and time limit and deals with spawning and managing collectables
/// </summary>
public class CollectableManager : MonoBehaviour
{
	[SerializeField, Tooltip("The text which tells the player how many keys they have and need.")]
	private TextMeshProUGUI collectableScoreDisplay;
	[SerializeField, Tooltip("The text that displays the amount of time remaining for the player.")] 
	private TextMeshProUGUI timeRemainingTextDisplay;

	[SerializeField, Tooltip("How fast the time reward depletes")] private float timeRewardDepletionHalflife;
	[SerializeField, Tooltip("how much the reward time is at the beginning of the game.")] private float initialRewardTime;
	[SerializeField, Tooltip("the minimum reward time")] private float minimumRewardTime;
	
	public static CollectableManager theCollectableManager;
	private int numberOfCollectablesCollected = 0; 
	private float timeRemaining;

	private void Awake()
	{
		timeRemaining = 3 * minimumRewardTime;
	}

	private void Start()
	{
		theCollectableManager = this;
	}

	/// <summary>
	/// a property that determines the appropriate reward time based on how far into the game you are
	/// </summary>
	private float TimeReward => Mathf.Pow(0.5f, (Time.time / timeRewardDepletionHalflife)) * initialRewardTime + minimumRewardTime;
	
	/// <summary>
	/// adds to the time remaining and score
	/// </summary>
	public static void Collect()
	{
		theCollectableManager.numberOfCollectablesCollected++;
		theCollectableManager.timeRemaining += theCollectableManager.TimeReward;
		ScoreSystem.theScoreSystem.Score = theCollectableManager.numberOfCollectablesCollected;
	}
	
	private void Update()
	{
		timeRemaining -= Time.deltaTime;
		if(timeRemaining < 0)
		{
			ScoreSystem.theScoreSystem.SaveScore();
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		// update the text and bar to match the remaining number of keys.
		if(collectableScoreDisplay)
		{
			collectableScoreDisplay.text = $"Score: {numberOfCollectablesCollected}";
		}

		if(timeRemainingTextDisplay)
		{
			timeRemainingTextDisplay.text = $"Time remaining: {Mathf.Round(timeRemaining)}";
		}
	}
}
