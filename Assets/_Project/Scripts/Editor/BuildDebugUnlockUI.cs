using System.Linq;
using TheBeginning.UI;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Events;

namespace TheBeginning.EditorTools
{
    public static class BuildDebugUnlockUI
    {
        const string PrefabPath = "Assets/_Project/Prefabs/Popup/Game/PopupSetting.prefab";
        const string NotificationEventPath = "Assets/_Sunflower/Scriptable/Event/show_notification_in_game_event.asset";
        const string TitleName = "Title";

        [MenuItem("Tools/Build Debug Unlock UI")]
        public static void Execute()
        {
            var root = PrefabUtility.LoadPrefabContents(PrefabPath);
            try
            {
                var title = root.GetComponentsInChildren<Transform>(true).First(t => t.name == TitleName);
                var titleText = title.GetComponent<TextMeshProUGUI>();
                var font = titleText.font;
                titleText.raycastTarget = true;

                var old = root.transform.Find("DebugPasswordPanel");
                if (old) Object.DestroyImmediate(old.gameObject);

                var panel = NewUI("DebugPasswordPanel", root.transform);
                Stretch(panel);
                panel.gameObject.AddComponent<Image>().color = new Color(0, 0, 0, 0.75f);

                var box = NewUI("Box", panel);
                box.sizeDelta = new Vector2(700, 420);
                box.gameObject.AddComponent<Image>().color = new Color(0.15f, 0.15f, 0.2f, 1f);

                var header = Text("Header", box, font, "Enter Password", 44);
                header.rectTransform.anchoredPosition = new Vector2(0, 150);
                header.rectTransform.sizeDelta = new Vector2(640, 70);

                var inputRt = NewUI("InputPassword", box);
                inputRt.sizeDelta = new Vector2(600, 90);
                inputRt.anchoredPosition = new Vector2(0, 40);
                var inputBg = inputRt.gameObject.AddComponent<Image>();
                var textArea = NewUI("Text Area", inputRt);
                Stretch(textArea, 20, 8);
                textArea.gameObject.AddComponent<RectMask2D>();
                var placeholder = Text("Placeholder", textArea, font, "Password...", 36);
                Stretch(placeholder.rectTransform);
                placeholder.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                placeholder.fontStyle = FontStyles.Italic;
                placeholder.alignment = TextAlignmentOptions.MidlineLeft;
                var inputText = Text("Text", textArea, font, "", 36);
                Stretch(inputText.rectTransform);
                inputText.color = Color.black;
                inputText.alignment = TextAlignmentOptions.MidlineLeft;
                var input = inputRt.gameObject.AddComponent<TMP_InputField>();
                input.targetGraphic = inputBg;
                input.textViewport = textArea;
                input.textComponent = inputText;
                input.placeholder = placeholder;
                input.fontAsset = font;
                input.pointSize = 36;
                input.contentType = TMP_InputField.ContentType.Password;
                input.lineType = TMP_InputField.LineType.SingleLine;

                var message = Text("Message", box, font, "", 30);
                message.rectTransform.anchoredPosition = new Vector2(0, -35);
                message.rectTransform.sizeDelta = new Vector2(600, 50);
                message.color = new Color(1f, 0.35f, 0.35f, 1f);

                var confirm = MakeButton("BtnConfirm", box, font, "OK", new Vector2(150, -130),
                    new Color(0.2f, 0.65f, 0.3f, 1f));
                var close = MakeButton("BtnClose", box, font, "Cancel", new Vector2(-150, -130),
                    new Color(0.6f, 0.25f, 0.25f, 1f));

                panel.gameObject.SetActive(false);
                panel.SetAsLastSibling();

                var unlocker = title.GetComponent<DebugViewUnlocker>();
                if (unlocker == null) unlocker = title.gameObject.AddComponent<DebugViewUnlocker>();
                var so = new SerializedObject(unlocker);
                so.FindProperty("passwordPanel").objectReferenceValue = panel.gameObject;
                so.FindProperty("inputPassword").objectReferenceValue = input;
                so.FindProperty("buttonConfirm").objectReferenceValue = confirm;
                so.FindProperty("buttonClose").objectReferenceValue = close;
                so.FindProperty("textMessage").objectReferenceValue = message;
                so.FindProperty("showNotificationInGameEvent").objectReferenceValue =
                    AssetDatabase.LoadAssetAtPath<StringEvent>(NotificationEventPath);
                so.ApplyModifiedPropertiesWithoutUndo();

                PrefabUtility.SaveAsPrefabAsset(root, PrefabPath);
                Debug.Log("Debug unlock UI built.");
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(root);
            }
        }

        static RectTransform NewUI(string name, Transform parent)
        {
            var go = new GameObject(name, typeof(RectTransform)) { layer = 5 };
            var rt = (RectTransform)go.transform;
            rt.SetParent(parent, false);
            return rt;
        }

        static void Stretch(RectTransform rt, float padX = 0, float padY = 0)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = new Vector2(padX, padY);
            rt.offsetMax = new Vector2(-padX, -padY);
        }

        static TextMeshProUGUI Text(string name, Transform parent, TMP_FontAsset font, string text, float size)
        {
            var t = NewUI(name, parent).gameObject.AddComponent<TextMeshProUGUI>();
            if (font) t.font = font;
            t.text = text;
            t.fontSize = size;
            t.alignment = TextAlignmentOptions.Center;
            t.raycastTarget = false;
            return t;
        }

        static Button MakeButton(string name, Transform parent, TMP_FontAsset font, string label, Vector2 pos,
            Color color)
        {
            var rt = NewUI(name, parent);
            rt.sizeDelta = new Vector2(240, 90);
            rt.anchoredPosition = pos;
            var img = rt.gameObject.AddComponent<Image>();
            img.color = color;
            var btn = rt.gameObject.AddComponent<Button>();
            btn.targetGraphic = img;
            var t = Text("Text", rt, font, label, 38);
            Stretch(t.rectTransform);
            t.color = Color.white;
            return btn;
        }
    }
}
