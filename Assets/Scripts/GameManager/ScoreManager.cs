using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnScoreBonusGrantEventArgs : System.EventArgs
{
    public int BonusAmount { get; set; }
}

[System.Serializable]
internal class SpeedRange
{
    [SerializeField] [Tooltip("The score multiplier that will be applied in this range")]
    private float multiplier;
    
    [SerializeField] [Range(0, 100)] [Tooltip("Range upper limit (%) - relative to maximum speed")]
    private int upperLimit;

    public float GetMultiplier()
    {
        return multiplier;
    }

    public float GetUpperLimit()
    {
        return upperLimit;
    }
    
    // Using a reference parameter, checks if the value is contained within the relative range
    // I.e.: is 'value' within the range [0, value%] of maximum speed
    
    public bool Contain(float value, float reference)
    {
        return (value / reference * 100 <= upperLimit);
    }
}

public class ScoreManager : MonoBehaviour
{
    [SerializeField] [Range(1f, 50f)] [Tooltip("A constant by which the score will be multiplied. A higher value will result in a higher score.")]
    private float constantMultiplier = 10f;
    
    [SerializeField] [Tooltip("The minimum speed the player has to reach in order to score")]
    private int minimumScoringSpeed;
    
    [SerializeField] [Tooltip("Speed ranges and their respective score multipliers")]
    private List<SpeedRange> ranges;
    
    [SerializeField]
    private TextMeshProUGUI scoreText;
    
    private Rigidbody playerRigidbody;

    private float maximumSpeed;
    private int currentRange;
    private float score;

    public event System.EventHandler<OnScoreBonusGrantEventArgs> OnScoreBonusGrant;

    void Start()
    {
        GameObject player = GetComponent<RaceManager>().player;
        playerRigidbody = player.GetComponent<Rigidbody>();
        maximumSpeed = player.GetComponent<VehicleController>().GetMaximumSpeed();
    }

    void FixedUpdate()
    {
        CalculateScore();
        scoreText.text = score.ToString();
        
        // DEBUG ---------------
        
        if (currentRange > 0)
        {
            scoreText.text += "\n{" + (currentRange + 1) + ", (" + ranges[currentRange - 1].GetUpperLimit() + "% ~ " +
                              ranges[currentRange].GetUpperLimit() + "%)} (x" + ranges[currentRange].GetMultiplier() + ")";
        }
        else
        {
            scoreText.text += "\n{" + (currentRange + 1) + ", (0% ~ " + ranges[currentRange].GetUpperLimit() +
                              "%)} (x" + ranges[currentRange].GetMultiplier() + ")";
        }
        
        // ---------------------
    }

    private void CalculateScore()
    {
        int currentSpeed = Mathf.FloorToInt(playerRigidbody.velocity.magnitude);
        
        if (currentSpeed > minimumScoringSpeed && currentSpeed >= 1)
        {

            if (currentSpeed < maximumSpeed)
            {
                int i = 0;
                // while (currentSpeed/maximumSpeed * 100 > ranges[i].GetLimit()) i++;
                while (!ranges[i].Contain(currentSpeed, maximumSpeed))
                    i++; // Find the range that contains the current speed value
                currentRange = i;
            }
            else
            {
                currentRange = ranges.Count - 1;
            }

            // scoreF += constantMultiplier * ranges[currentRange].GetMultiplier() * Time.deltaTime;
            score = Mathf.Lerp(score, score + constantMultiplier * ranges[currentRange].GetMultiplier(), Time.deltaTime);
        }
    }

    public int GetScore()
    {
        return Mathf.RoundToInt(score);
    }

    public float GetMultiplier()
    {
        return ranges[currentRange].GetMultiplier();
    }

    public int GetRange()
    {
        return currentRange;
    }

    public int GetMaxRange()
    {
        return ranges.Count - 1;
    }

    public void GrantBonus(int bonus)
    {
        score += bonus;
        
        OnScoreBonusGrantEventArgs onBonusGrantedEventArgs = new OnScoreBonusGrantEventArgs
        {
            BonusAmount = bonus
        };

        if (OnScoreBonusGrant != null)
        {
            OnScoreBonusGrant(this, onBonusGrantedEventArgs);
        }
    }
}
