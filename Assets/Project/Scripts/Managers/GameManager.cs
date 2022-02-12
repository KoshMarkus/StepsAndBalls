using PlayFab;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Project.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("How much steps to every milestone")]
        
        [SerializeField] private int rangeUntilMilestone;
        private int score;
        private int milestoneScore;
        
        public UnityEvent<string> onScoreChange;
        public UnityEvent<string> onGameOver;
        public UnityEvent<int> onLeaderboardUpdate;
        public UnityEvent onMilestoneReach;
        public UnityEvent onGameStarted;
        
        //For UnityEvents
        
        //GameManager > onGameStarted
        
        public void GameStarted()
        {
            score = 0;
            onScoreChange.Invoke(score.ToString());
            
            milestoneScore = score + rangeUntilMilestone;
            
            onGameStarted.Invoke();
        }
        
        //Player > onDeath
        
        public void GameOver()
        {
            onGameOver.Invoke(score.ToString());
            onLeaderboardUpdate.Invoke(score);
        }
        
        //Player > onStepUp
        
        public void AddScore()
        {
            score++;
            onScoreChange.Invoke(score.ToString());

            if (score == milestoneScore)
            {
                onMilestoneReach.Invoke();
                
                milestoneScore = score + rangeUntilMilestone;
            }
        }
    }
}
