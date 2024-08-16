using System.Collections.Generic;
using System.Threading.Tasks;
using PrimeTween;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityDebugSheet.Runtime.Foundation.PageNavigator.Modules;
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.GameService;
using VirtueSky.Inspector;
using VirtueSky.Threading.Tasks;
using VirtueSky.Variables;

namespace TheBeginning.UI
{
    [EditorIcon("icon_leaderboard"), HideMonoScript]
    public sealed class PopupLeaderboard : UIPopup
    {
        [SerializeField] private string allTimeTableId = "ALL_TIME_RANK";
        [SerializeField] private string weeklyTableId = "WEEKLY_RANK";
        [SerializeField] private IntegerVariable currentLevel;
        [SerializeField] private GameObject rootLeaderboard;
        [SerializeField] private GameObject block;

        [SerializeField] private LeaderboardElementColor colorRank1 = new(new Color(1f, 0.82f, 0f),
            new Color(0.44f, 0.33f, 0f),
            new Color(0.99f, 0.96f, 0.82f),
            new Color(1f, 0.55f, 0.01f),
            new Color(0.47f, 0.31f, 0f));

        [SerializeField] private LeaderboardElementColor colorRank2 = new(new Color(0.79f, 0.84f, 0.91f),
            new Color(0.29f, 0.4f, 0.6f),
            new Color(0.94f, 0.94f, 0.94f),
            new Color(0.45f, 0.54f, 0.56f),
            new Color(0.18f, 0.31f, 0.48f));

        [SerializeField] private LeaderboardElementColor colorRank3 = new(new Color(0.8f, 0.59f, 0.31f),
            new Color(0.34f, 0.23f, 0.09f),
            new Color(1f, 0.82f, 0.57f),
            new Color(0.3f, 0.22f, 0.12f),
            new Color(0.4f, 0.25f, 0.1f));

        [SerializeField] private LeaderboardElementColor colorRankYou = new(new Color(0.47f, 0.76f, 0.92f),
            new Color(0.08f, 0.53f, 0.71f),
            new Color(0.09f, 0.53f, 0.71f),
            new Color(0.22f, 0.58f, 0.85f),
            new Color(0.08f, 0.27f, 0.42f));

        [SerializeField] private LeaderboardElementColor colorOutRank = new();

        [TitleColor("Authentication", CustomColor.Aqua, CustomColor.Aquamarine)] [SerializeField]
        private EventNoParam loginEvent;

        [SerializeField] private StringVariable serverCode;
        [SerializeField] private StatusLoginVariable status;
        [SerializeField] private StringVariable nameVariable;
        [SerializeField] private EventNoParam gpgsGetNewServerCodeEvent;


        private LeaderboardData _allTimeData = new("alltime_data");
        private LeaderboardData _weeklyData = new("weekly_data");
        private Dictionary<string, Dictionary<string, object>> _userLeaderboardData = new();
        private int _countInOnePage;
        private Sequence[] _sequences;
        private ELeaderboardTab _currentTab = ELeaderboardTab.AllTime;
        private AsyncProcessHandle _handleAnimation;
        private bool _firstTimeEnterWeekly = true;
        private bool _firstTimeEnterWorld = true;

        private async void Init()
        {
            block.SetActive(true);
            rootLeaderboard.SetActive(false);

            InitEditor();
            InitAndroid();
            InitIos();

            await Excute();

            return;

            async Task Excute()
            {
                rootLeaderboard.SetActive(false);
                LeaderboardEntry resultAdded;
                if (_firstTimeEnterWorld)
                {
                    resultAdded =
                        await LeaderboardsService.Instance.AddPlayerScoreAsync(allTimeTableId, currentLevel.Value);
                }
                else
                {
                    resultAdded = await LeaderboardsService.Instance.GetPlayerScoreAsync(allTimeTableId);
                }

                _allTimeData.myRank = resultAdded.Rank;
                if (string.IsNullOrEmpty(AuthenticationService.Instance.PlayerName))
                {
                    await AuthenticationService.Instance.UpdatePlayerNameAsync(nameVariable.Value);
                    await LoadNextDataAllTimeScores();
                    block.SetActive(false);
                    Refresh(_allTimeData);
                }
                else
                {
                    if (_firstTimeEnterWorld)
                    {
                        _firstTimeEnterWorld = false;
                        await LoadNextDataAllTimeScores();
                    }

                    block.SetActive(false);
                    Refresh(_allTimeData);
                }
            }
        }

        private async UniTask<bool> LoadNextDataAllTimeScores()
        {
            _allTimeData.offset = _allTimeData.entries.Count - 1 != 0 ? _allTimeData.entries.Count - 1 : 0;
            var scores = await LeaderboardsService.Instance.GetScoresAsync(allTimeTableId,
                new GetScoresOptions { Limit = _allTimeData.limit, Offset = _allTimeData.offset });
            _allTimeData.entries.AddRange(scores.Results);
            _allTimeData.pageCount = (int)System.Math.Ceiling(_allTimeData.entries.Count / (float)_countInOnePage);
            return true;
        }

        private async UniTask<bool> LoadNextDataWeeklyScore()
        {
            _weeklyData.offset = _weeklyData.entries.Count - 1 != 0 ? _weeklyData.entries.Count - 1 : 0;
            var scores = await LeaderboardsService.Instance.GetScoresAsync(weeklyTableId,
                new GetScoresOptions { Limit = _weeklyData.limit, Offset = _weeklyData.offset });
            _weeklyData.entries.AddRange(scores.Results);
            _weeklyData.pageCount = (int)System.Math.Ceiling(_weeklyData.entries.Count / (float)_countInOnePage);
            return true;
        }

        private void Refresh(LeaderboardData allTimeData)
        {
        }


        #region Init Authentication

        private async void InitEditor()
        {
#if UNITY_EDITOR
            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
#endif
        }

        private async void InitAndroid()
        {
#if UNITY_ANDROID && VIRTUESKY_GPGS
            if (!GooglePlayGamesAuthentication.IsSignIn())
            {
                status.SetNotLoggedIn();
                loginEvent.Raise();
                await UniTask.WaitUntil(() => status.Value == StatusLogin.Successful);
                if (string.IsNullOrEmpty(serverCode.Value))
                {
                    // Login failed
                    Debug.Log("Login failed");
                    return;
                }
            }
            else
            {
                status.SetNotLoggedIn();
                gpgsGetNewServerCodeEvent.Raise();
                await UniTask.WaitUntil(() => status.Value == StatusLogin.Successful);
            }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
            if (AuthenticationService.Instance.SessionTokenExists)
            {
                // signin cached
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            else
            {
                await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(serverCode.Value);
            }
#endif
        }

        private async void InitIos()
        {
#if UNITY_IOS
            status.SetNotLoggedIn();
            loginEvent.Raise();
            await UniTask.WaitUntil(() => status.Value == StatusLogin.Successful);

            if (string.IsNullOrEmpty(serverCode.Value))
            {
                // Login failed
                Debug.Log("Login failed");
            }
#endif

#if UNITY_IOS && !UNITY_EDITOR
            if (AuthenticationService.Instance.SessionTokenExists)
            {
                // signin cached
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            else
            {
                await AuthenticationService.Instance.SignInWithAppleAsync(serverCode.Value);
            }
#endif
        }

        #endregion
    }
}