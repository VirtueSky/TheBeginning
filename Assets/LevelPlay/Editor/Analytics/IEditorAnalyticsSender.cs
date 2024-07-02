namespace Unity.Services.LevelPlay.Editor.Analytics
{
     interface IEditorAnalyticsSender
     {
         void SendEvent(string eventName, EventBody body);
     }
}
