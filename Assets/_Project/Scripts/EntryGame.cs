using UnityEngine.AddressableAssets;
using VirtueSky.Core;
using VirtueSky.Threading.Tasks;

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
            await Addressables.LoadSceneAsync(Constant.LAUNCHER_SCENE);
            Destroy(gameObject);
        }
    }
}