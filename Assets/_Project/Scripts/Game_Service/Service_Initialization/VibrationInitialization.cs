using VirtueSky.Inspector;
using VirtueSky.Vibration;

namespace TheBeginning.Services
{
    [HideMonoScript]
    public class VibrationInitialization : ServiceInitialization
    {
        public override void Initialization()
        {
            Vibration.Init();
        }
    }
}