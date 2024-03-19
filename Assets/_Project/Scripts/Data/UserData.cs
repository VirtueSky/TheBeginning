using VirtueSky.DataStorage;

namespace TheBeginning.UserData
{
    public partial struct UserData
    {
        public static bool IsFirstOpenGame
        {
            get => Get(Constant.IS_FIRST_OPEN_GAME, 0) == 1;
            set => Set(Constant.IS_FIRST_OPEN_GAME, value ? 1 : 0);
        }

        public static int GetNumberShowGameObject(string gameObjectID)
        {
            return Get($"{Constant.GAMEOBJECT_SHOW}_{gameObjectID}", 0);
        }

        public static void IncreaseNumberShowGameObject(string gameObjectID)
        {
            int value = GetNumberShowGameObject(gameObjectID);
            Set($"{Constant.GAMEOBJECT_SHOW}_{gameObjectID}", ++value);
        }

        public static int ProgressAmount
        {
            get => Get(Constant.PROGRESS_AMOUNT, 0);
            set => Set(Constant.PROGRESS_AMOUNT, value);
        }


        public static T Get<T>(string key, T defaultValue = default) => GameData.Get(key, defaultValue);
        public static void Set<T>(string key, T data) => GameData.Set(key, data);
    }
}