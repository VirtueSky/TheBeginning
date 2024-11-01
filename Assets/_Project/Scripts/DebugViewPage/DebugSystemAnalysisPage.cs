using System.Collections;
using System.Threading.Tasks;
using Tayx.Graphy;
using UnityDebugSheet.Runtime.Core.Scripts;
using UnityEngine;

namespace TheBeginning.DebugViewPage
{
    public class DebugSystemAnalysisPage : DefaultDebugPageBase
    {
        protected override string Title => "System Analysis";
        private Sprite iconFps;
        private Sprite iconRam;
        private Sprite iconAudio;
        private Sprite iconAdvanced;

        public void Init(Sprite _iconFps, Sprite _iconRam, Sprite _iconAudio, Sprite _iconAdvanced)
        {
            iconFps = _iconFps;
            iconRam = _iconRam;
            iconAudio = _iconAudio;
            iconAdvanced = _iconAdvanced;
        }


#if UDS_USE_ASYNC_METHODS
        public override Task Initialize()
        {
            OnInitialize();
            return base.Initialize();
        }
#else
        public override IEnumerator Initialize()
        {
            OnInitialize();
            return base.Initialize();
        }
#endif
        void OnInitialize()
        {
            AddEnumPicker(GraphyManager.Instance.FpsModuleState, "Fps", icon: iconFps,
                activeValueChanged: @enum => { GraphyManager.Instance.FpsModuleState = (GraphyManager.ModuleState)@enum; });
            AddEnumPicker(GraphyManager.Instance.RamModuleState, "Ram", icon: iconRam,
                activeValueChanged: @enum => { GraphyManager.Instance.RamModuleState = (GraphyManager.ModuleState)@enum; });
            AddEnumPicker(GraphyManager.Instance.AudioModuleState, "Audio", icon: iconAudio,
                activeValueChanged: @enum => { GraphyManager.Instance.AudioModuleState = (GraphyManager.ModuleState)@enum; });
            AddEnumPicker(GraphyManager.Instance.AdvancedModuleState, "Advanced", icon: iconAdvanced,
                activeValueChanged: @enum => { GraphyManager.Instance.AdvancedModuleState = (GraphyManager.ModuleState)@enum; });
        }
    }
}