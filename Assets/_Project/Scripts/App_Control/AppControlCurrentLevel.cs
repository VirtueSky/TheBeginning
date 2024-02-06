namespace TheBeginning.AppControl
{
    public struct AppControlCurrentLevel
    {
        private static Level _currentLevel;

        public static void Init(Level level)
        {
            if (_currentLevel != null)
            {
                UnityEngine.Object.Destroy(_currentLevel);
            }

            AppControlCurrentLevel._currentLevel = level;
        }
    }
}