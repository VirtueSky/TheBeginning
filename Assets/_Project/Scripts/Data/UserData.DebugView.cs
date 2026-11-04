using System;

namespace TheBeginning.Data
{
    public partial struct UserData
    {
        public static event Action<bool> DebugViewChanged;

        public static bool EnableDebugView
        {
            get => Get(Constant.ENABLE_DEBUG_VIEW, false);
            set
            {
                Set(Constant.ENABLE_DEBUG_VIEW, value);
                DebugViewChanged?.Invoke(value);
            }
        }
    }
}
