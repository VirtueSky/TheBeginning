using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Rating;

namespace TheBeginning.Services
{
    [HideMonoScript]
    public class InAppReviewInitialization : ServiceInitialization
    {
        [SerializeField] private InAppReview inAppReview;

        public override void Initialization()
        {
            inAppReview.InitInAppReview();
        }
    }
}