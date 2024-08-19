using Unity.Services.Core;
using VirtueSky.Inspector;

namespace TheBeginning.Services
{
    [HideMonoScript]
    public class AuthenticationInitialization : ServiceInitialization
    {
        public override void Initialization()
        {
            UnityServices.InitializeAsync();
        }
    }
}