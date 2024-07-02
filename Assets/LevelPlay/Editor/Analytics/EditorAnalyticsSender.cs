using System;
using System.Collections.Generic;
using UnityEditor;

namespace Unity.Services.LevelPlay.Editor.Analytics
{
    class EditorAnalyticsSender : IEditorAnalyticsSender
    {
        const int k_EventVersion = 1;
        const string k_ServicesCorePackageName = "com.unity.services.core";

        static Queue<QueuedEvent> eventsQueue = new Queue<QueuedEvent>();
        static bool servicesCoreIsReady;

        [InitializeOnLoadMethod]
        static void CheckAnalyticsReady()
        {
            LevelPlayPackmanQuerier.instance.CheckIfPackageIsInstalledWithUpm(k_ServicesCorePackageName, coreIsInstalled =>
            {
                servicesCoreIsReady = true;
                while (eventsQueue.Count > 0)
                {
                    var eventEntry = eventsQueue.Dequeue();
                    SendEventWithBody(eventEntry.Name, eventEntry.Body);
                }
            });
        }

        public void SendEvent(string eventName, EventBody body)
        {
            if (!servicesCoreIsReady)
            {
                eventsQueue.Enqueue(new QueuedEvent { Name = eventName, Body = body, });
            }
            else
            {
                SendEventWithBody(eventName, body);
            }
        }

        static void SendEventWithBody(string eventName, object body)
        {
            try
            {
                EditorAnalytics.SendEventWithLimit(eventName, body, k_EventVersion);
            }
            catch (Exception)
            {
                // Silent catch because error in analytics shouldn't prevent users from executing their action
            }
        }

        class QueuedEvent
        {
            internal string Name;
            internal EventBody Body;
        }
    }
}
