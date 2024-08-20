using System.Collections.Generic;
using Unity.Services.Leaderboards.Models;

namespace TheBeginning.UI
{
    public class LeaderboardData
    {
        public int currentPage;
        public List<LeaderboardEntry> entries;
        public bool firstTime;
        public int pageCount;
        public int myRank;
        public int offset;
        public int limit;
        private readonly string _key;

        public LeaderboardData(string key)
        {
            _key = key;
            firstTime = true;
            entries = new List<LeaderboardEntry>();
            currentPage = 0;
            pageCount = 1;
            offset = 0;
            limit = 100;
            myRank = -1;
        }
    }

    public enum ELeaderboardTab
    {
        AllTime,
        Weekly
    }
}