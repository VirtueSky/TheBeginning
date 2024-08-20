using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using PrimeTween;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityDebugSheet.Runtime.Foundation.PageNavigator.Modules;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Core;
using VirtueSky.Events;
using VirtueSky.GameService;
using VirtueSky.Inspector;
using VirtueSky.Misc;
using VirtueSky.Threading.Tasks;
using VirtueSky.Variables;

namespace TheBeginning.UI
{
    [EditorIcon("icon_leaderboard"), HideMonoScript]
    public sealed class PopupLeaderboard : UIPopup
    {
        [SerializeField] private string allTimeTableId = "ALL_TIME_RANK";
        [SerializeField] private string weeklyTableId = "WEEKLY_RANK";
        [SerializeField] private string playerNameEditor = "VirtueSky";
        [SerializeField] private IntegerVariable currentLevel;
        [SerializeField] private GameObject contentSlot;
        [SerializeField] private GameObject rootLeaderboard;
        [SerializeField] private GameObject block;
        [SerializeField] private Button buttonClose;
        [SerializeField] private Button buttonNextPage;
        [SerializeField] private Button buttonPreviousPage;
        [SerializeField] private Button buttonAllTimeRank;
        [SerializeField] private Button buttonWeeklyRank;
        [SerializeField] private Sprite spriteCurrentTab;
        [SerializeField] private Sprite spriteNormalTab;
        [SerializeField] private TextMeshProUGUI textName;
        [SerializeField] private TextMeshProUGUI textRank;
        [SerializeField] private TextMeshProUGUI textCurrentPage;
        [SerializeField] private AnimationCurve displayRankCurve;
        [SerializeField] private List<LeaderboardElementView> slots = new List<LeaderboardElementView>();

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
        [SerializeField] private StringEvent showNotificationInGameEvent;


        private LeaderboardData _allTimeData = new("alltime_data");
        private LeaderboardData _weeklyData = new("weekly_data");
        private Dictionary<string, Dictionary<string, object>> _userLeaderboardData = new();
        private int _countInOnePage;
        private Sequence[] _sequences;
        private ELeaderboardTab _currentTab = ELeaderboardTab.AllTime;
        private AsyncProcessHandle _handleAnimation;
        private bool _firstTimeEnterWeekly = true;
        private bool _firstTimeEnterWorld = true;

        protected override void OnBeforeShow()
        {
            base.OnBeforeShow();
            _countInOnePage = slots.Count;
            _sequences = new Sequence[slots.Count];
            buttonNextPage.onClick.AddListener(OnButtonNextPagePressed);
            buttonPreviousPage.onClick.AddListener(OnButtonPreviousPagePressed);
            buttonAllTimeRank.onClick.AddListener(OnButtonAllTimeRankPressed);
            buttonWeeklyRank.onClick.AddListener(OnButtonWeeklyRankPressed);
            Login();
        }

        protected override void OnBeforeHide()
        {
            base.OnBeforeHide();
            buttonNextPage.onClick.RemoveListener(OnButtonNextPagePressed);
            buttonPreviousPage.onClick.RemoveListener(OnButtonPreviousPagePressed);
            buttonAllTimeRank.onClick.RemoveListener(OnButtonAllTimeRankPressed);
            buttonWeeklyRank.onClick.RemoveListener(OnButtonWeeklyRankPressed);
        }

        private void OnButtonWeeklyRankPressed()
        {
            _currentTab = ELeaderboardTab.Weekly;
            buttonWeeklyRank.image.sprite = spriteCurrentTab;
            buttonAllTimeRank.image.sprite = spriteNormalTab;
            InitTable(_weeklyData, weeklyTableId, _firstTimeEnterWeekly);
        }

        private void OnButtonAllTimeRankPressed()
        {
            _currentTab = ELeaderboardTab.AllTime;
            buttonWeeklyRank.image.sprite = spriteNormalTab;
            buttonAllTimeRank.image.sprite = spriteCurrentTab;
            InitTable(_allTimeData, allTimeTableId, _firstTimeEnterWorld);
        }

        private void OnButtonPreviousPagePressed()
        {
            buttonPreviousPage.interactable = false;
            switch (_currentTab)
            {
                case ELeaderboardTab.AllTime:
                    PreviousPage(_allTimeData);
                    break;
                case ELeaderboardTab.Weekly:
                    PreviousPage(_weeklyData);
                    break;
            }
        }

        private void PreviousPage(LeaderboardData leaderboardData)
        {
            if (leaderboardData.currentPage > 0)
            {
                leaderboardData.currentPage--;
                buttonPreviousPage.interactable = true;
                Refresh(leaderboardData);
            }
        }

        private void OnButtonNextPagePressed()
        {
            buttonNextPage.interactable = false;
            switch (_currentTab)
            {
                case ELeaderboardTab.AllTime:
                    NextPage(_allTimeData);
                    break;
                case ELeaderboardTab.Weekly:
                    NextPage(_weeklyData);
                    break;
            }
        }


        private async void NextPage(LeaderboardData leaderboardData)
        {
            leaderboardData.currentPage++;
            if (leaderboardData.currentPage == leaderboardData.pageCount - 1)
            {
                if (leaderboardData.entries.Count > 0)
                {
                    block.SetActive(true);
                    contentSlot.SetActive(false);

                    switch (_currentTab)
                    {
                        case ELeaderboardTab.AllTime:
                            await LoadNextData(_allTimeData, allTimeTableId); // request more entry
                            break;
                        case ELeaderboardTab.Weekly:
                            await LoadNextData(_weeklyData, weeklyTableId); // request more entry
                            break;
                    }

                    block.SetActive(false);
                    Refresh(leaderboardData);
                }
            }
            else
            {
                buttonNextPage.interactable = true;
                Refresh(leaderboardData);
            }
        }

        private async void Login()
        {
            block.SetActive(true);
            rootLeaderboard.SetActive(false);

#if UNITY_EDITOR
            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
#endif
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
                    showNotificationInGameEvent.Raise("Failed to retrieve Google play games authorization code");
                    return;
                }
            }
            else
            {
                status.SetNotLoggedIn();
                gpgsGetNewServerCodeEvent.Raise();
                await UniTask.WaitUntil(() => status.Value == StatusLogin.Successful);
            }
            
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

#if UNITY_IOS && !UNITY_EDITOR
            status.SetNotLoggedIn();
            loginEvent.Raise();
            await UniTask.WaitUntil(() => status.Value == StatusLogin.Successful);

            if (string.IsNullOrEmpty(serverCode.Value))
            {
                // Login failed
                Debug.Log("Login failed");
                showNotificationInGameEvent.Raise("Failed to login Apple");
            }

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
            InitTable(_allTimeData, allTimeTableId, _firstTimeEnterWorld);
        }

        private async void InitTable(LeaderboardData leaderboardData, string tableId, bool firstTimeEnter)
        {
            rootLeaderboard.SetActive(false);
            block.SetActive(true);
            LeaderboardEntry resultAdded;
            if (firstTimeEnter)
            {
                resultAdded =
                    await LeaderboardsService.Instance.AddPlayerScoreAsync(tableId, currentLevel.Value);
            }
            else
            {
                resultAdded = await LeaderboardsService.Instance.GetPlayerScoreAsync(tableId);
            }

            leaderboardData.myRank = resultAdded.Rank;
            if (string.IsNullOrEmpty(AuthenticationService.Instance.PlayerName))
            {
                await AuthenticationService.Instance.UpdatePlayerNameAsync(GetPlayerName());
                await LoadNextData(leaderboardData, tableId);
                block.SetActive(false);
                Refresh(leaderboardData);
            }
            else
            {
                if (firstTimeEnter)
                {
                    firstTimeEnter = false;
                    await LoadNextData(leaderboardData, tableId);
                }

                block.SetActive(false);
                Refresh(leaderboardData);
            }
        }


        string GetPlayerName()
        {
#if UNITY_EDITOR_64
            return playerNameEditor;
#else
            return nameVariable.Value;
#endif
        }


        private async UniTask<bool> LoadNextData(LeaderboardData leaderboardData, string tableId)
        {
            leaderboardData.offset = (leaderboardData.entries.Count - 1).Max(0);
            var scores = await LeaderboardsService.Instance.GetScoresAsync(tableId,
                new GetScoresOptions { Limit = leaderboardData.limit, Offset = leaderboardData.offset });
            leaderboardData.entries.AddRange(scores.Results);
            leaderboardData.pageCount = (leaderboardData.entries.Count / (float)_countInOnePage).CeilToInt();
            return true;
        }

        private void Refresh(LeaderboardData data)
        {
            buttonNextPage.interactable = true;
            string[] playerNameSplits = AuthenticationService.Instance.PlayerName.Split('#');
            textName.text = playerNameSplits[0];
            textRank.text = $"{data.myRank + 1}";
            rootLeaderboard.SetActive(true);
            slots.ForEach(slot => slot.gameObject.SetActive(false));
            textCurrentPage.text = $"{data.currentPage + 1}";
            if (data.currentPage >= data.currentPage - 1) // reach the end
            {
                buttonNextPage.gameObject.SetActive(false);
                buttonPreviousPage.gameObject.SetActive(data.currentPage != 0);
            }

            block.SetActive(true);
            foreach (var sequence in _sequences)
            {
                sequence.Stop();
            }

            var pageData = new List<LeaderboardEntry>();
            for (int i = 0; i < _countInOnePage; i++)
            {
                int index = data.currentPage * _countInOnePage + i;
                if (data.entries.Count <= index) break;

                pageData.Add(data.entries[index]);
            }

            buttonPreviousPage.gameObject.SetActive(data.currentPage != 0);
            buttonNextPage.gameObject.SetActive(data.currentPage < data.pageCount - 1);
            contentSlot.SetActive(true);
            block.SetActive(false);
            App.StartCoroutine(PageSetup(pageData));
        }

        private IEnumerator PageSetup(List<LeaderboardEntry> pageData)
        {
            for (int i = 0; i < pageData.Count; i++)
            {
                slots[i].Init(pageData[i].Rank + 1, pageData[i].PlayerName.Split('#')[0], (int)pageData[i].Score,
                    ColorDivision(pageData[i].Rank + 1, pageData[i].PlayerId),
                    pageData[i].PlayerId.Equals(AuthenticationService.Instance.PlayerId));
                slots[i].gameObject.SetActive(true);

                _sequences[i].Stop();
                // todo play anim
                _sequences[i] = Sequence.Create();
                _sequences[i]
                    .Chain(Tween.Scale(slots[i].transform,
                        new Vector3(0.92f, 0.92f, 0.92f),
                        new Vector3(1.04f, 1.06f, 1),
                        0.2f,
                        Ease.OutQuad));
                _sequences[i]
                    .Chain(Tween.Scale(slots[i].transform,
                        new Vector3(1.04f, 1.06f, 1),
                        Vector3.one,
                        0.15f,
                        Ease.InQuad));
                yield return new WaitForSeconds(displayRankCurve.Evaluate(i / (float)pageData.Count));
            }
        }

        private LeaderboardElementColor ColorDivision(int rank, string playerId)
        {
            if (playerId.Equals(AuthenticationService.Instance.PlayerId)) return colorRankYou;

            return rank switch
            {
                1 => colorRank1,
                2 => colorRank2,
                3 => colorRank3,
                _ => colorOutRank
            };
        }
    }
}