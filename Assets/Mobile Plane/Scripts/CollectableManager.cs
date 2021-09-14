using System;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CollectableManager : MonoBehaviour
{
	[SerializeField, Tooltip("The text which tells the player how many keys they have and need.")]
	private TextMeshProUGUI collectableScoreDisplay;
	[SerializeField, Tooltip("The text that displays the amount of time remaining for the player.")] 
	private TextMeshProUGUI timeRemainingTextDisplay;

	[SerializeField] private float timeRewardDepletionHalflife;
	[SerializeField] private float initialRewardTime;
	[SerializeField] private float minimumRewardTime;
	
	public static CollectableManager theCollectableManager;
	private int numberOfCollectablesCollected = 0; 
	private float timeRemaining;

	private void Awake()
	{
		timeRemaining = 2 * minimumRewardTime;
	}

	private void Start()
	{
		theCollectableManager = this;
	}

	private float TimeReward => Mathf.Pow(0.5f, (Time.time / timeRewardDepletionHalflife)) * initialRewardTime + minimumRewardTime;
	
	public static void Collect()
	{
		theCollectableManager.numberOfCollectablesCollected++;
		theCollectableManager.timeRemaining += theCollectableManager.TimeReward;
	}
	
	private void Update()
	{
		timeRemaining -= Time.deltaTime;
		if(timeRemaining < 0)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		// update the text and bar to match the remaining number of keys.
		if(collectableScoreDisplay)
		{
			collectableScoreDisplay.text = $"{numberOfCollectablesCollected} Collectables";
		}

		if(timeRemainingTextDisplay)
		{
			timeRemainingTextDisplay.text = $"{Mathf.Round(timeRemaining)}";
		}
	}
}
