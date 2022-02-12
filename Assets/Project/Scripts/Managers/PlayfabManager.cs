using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Project.Scripts.Managers
{
    public class PlayfabManager : MonoBehaviour
    {
        [SerializeField] private Text playerName;

        [SerializeField] private Transform leaderboardTable; 
        [SerializeField] private GameObject leadrboardTableRowPrefab;
        
        public UnityEvent onFirstLogin;
        public UnityEvent<string> onLogin;
        public UnityEvent onLoginFail;
        public UnityEvent<string> onNameSet;
        public UnityEvent onNameSetError;
        public UnityEvent onLeaderboardLoad;

        private void Start()
        {
            Login();
        }

        private void Login()
        {
            var request = new LoginWithCustomIDRequest
            {
                CustomId = SystemInfo.deviceUniqueIdentifier,
                CreateAccount = true,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true
                }
            };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        }
        
        private void OnLoginSuccess(LoginResult result)
        {
            //Debug.Log("Successful login/sign up");

            string name = null;
            
            if (result.InfoResultPayload.PlayerProfile != null)
            {
                name = result.InfoResultPayload.PlayerProfile.DisplayName;
            }

            if (name == null)
            {
                onFirstLogin.Invoke();
            }
            else
            {
                onLogin.Invoke(name);
            }
        }

        private void OnLoginFailure(PlayFabError error)
        {
            Debug.Log(error.GenerateErrorReport());
            
            onLoginFail.Invoke();
        }

        private void OnSetNameError(PlayFabError error)
        {
            Debug.Log(error.GenerateErrorReport());
            onNameSetError.Invoke();
        }

        public void SendLeaderboardData(int score)
        {
            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate
                    {
                        StatisticName = "Score",
                        Value = score
                    }
                }
            };

            PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
        }

        private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
        {
            //Debug.Log("Successful leaderboard update");
        }

        public void GetLeaderboard()
        {
            var request = new GetLeaderboardRequest
            {
                StatisticName = "Score",
                StartPosition = 0,
                MaxResultsCount = 10
            };
            
            PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
        }

        private void OnLeaderboardGet(GetLeaderboardResult result)
        {

            foreach (Transform row in leaderboardTable)
            {
                Destroy(row.gameObject);
            }
            
            foreach (var item in result.Leaderboard)
            {
                var newRow = Instantiate(leadrboardTableRowPrefab, leaderboardTable);
                TextMeshProUGUI[] texts = newRow.GetComponentsInChildren<TextMeshProUGUI>();

                texts[0].text = (item.Position + 1).ToString();
                texts[1].text = item.DisplayName;
                texts[2].text = item.StatValue.ToString();
            }
            
            onLeaderboardLoad.Invoke();
        }
        
        private void OnError(PlayFabError error)
        {
            Debug.Log(error.GenerateErrorReport());
        }
        
        //NameSetPanel > StartButton > onClick
        
        public void SetPlayerName()
        {
            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = playerName.text,
            };
            
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnSetNameError);
        }

        private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
        {
            Debug.Log("Upated display name!");
            onNameSet.Invoke(result.DisplayName);
        }
    }
}
