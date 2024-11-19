using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using VirtueSky.Core;

namespace TheBeginning.SceneFlow
{
    public class EntryGame : BaseMono
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private async void Start()
        {
            await Addressables.LoadSceneAsync(Constant.SERVICE_SCENE);
            Destroy(gameObject);
        }
    }
}