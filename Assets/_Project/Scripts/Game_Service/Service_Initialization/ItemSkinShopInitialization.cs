using TheBeginning.Config;
using UnityEngine;
using VirtueSky.Inspector;

namespace TheBeginning.Services
{
    [HideMonoScript]
    public class ItemSkinShopInitialization : ServiceInitialization
    {
        [SerializeField] private ItemConfig itemConfig;

        public override void Initialization()
        {
            itemConfig.Initialize();
        }
    }
}