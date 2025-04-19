using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace KBT
{
    public class DialogWindow : EditorWindow
    {

        private List<Script> editorScipts = new List<Script>();
        private List<Script> filterDialogs
        {
            get
            {
                return editorScipts.FindAll(script =>
                {
                    return script.dialogData.dialogContent.Contains(searchContent) ||
                        script.dialogData.characterName.Contains(searchContent) ||
                        script.dialogCommand.ToString().Contains(searchContent);
                });
            }
        }
        private List<Script> viewDialogs
        {
            get
            {
                return isFilting ? filterDialogs : editorScipts;
            }
        }

        private bool isFilting
        {
            get { return filter != ""; }
        }
        private int viewLength
        {
            get
            {
                return isFilting ? filterDialogs.Count : listLength;
            }
        }

        private Vector2 scrollViewVector = Vector2.zero;
        private TextAsset referenceAsset;

        private string scriptName = "";
        private string searchContent = "";
        private string filter = "";
        private int listLength = 0;

        [MenuItem("Window/KBT/Dialog")]
        public static void ShowWindow()
        {
            DialogWindow dialogWindow = GetWindow<DialogWindow>();
            dialogWindow.titleContent = new GUIContent("Dialog Window");
        }

        [Obsolete]
        void OnGUI()
        {
            referenceComponent();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Script Name", new GUIStyle(EditorStyles.label) { fixedWidth = 150 });
            scriptName = EditorGUILayout.TextField(scriptName);
            GUILayout.EndHorizontal();

            // searchComponent();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Dialog List", new GUIStyle(EditorStyles.label) { fixedWidth = 150 });
            listLength = EditorGUILayout.IntField(listLength);
            GUILayout.Space(5);
            bool newItem = GUILayout.Button("New", new GUIStyle(EditorStyles.miniButtonMid) { fixedWidth = 40 });
            GUILayout.Space(5);
            bool openAll = GUILayout.Button("Open All", new GUIStyle(EditorStyles.miniButtonMid) { fixedWidth = 80 });
            GUILayout.Space(5);
            bool closeAll = GUILayout.Button("Close All", new GUIStyle(EditorStyles.miniButtonMid) { fixedWidth = 80 });
            GUILayout.EndHorizontal();

            if (newItem)
            {
                listLength++;
            }

            if (openAll)
            {
                editorScipts.ForEach(script => script.isFoldout = true);
            }

            if (closeAll)
            {
                editorScipts.ForEach(script => script.isFoldout = false);
            }

            while (listLength > editorScipts.Count)
            {
                editorScipts.Add(new Script());
            }
            while (listLength < editorScipts.Count)
            {
                editorScipts.RemoveAt(editorScipts.Count - 1);
            }

            scrollViewVector = GUILayout.BeginScrollView(scrollViewVector);
            for (int i = 0; i < viewLength; i++)
            {
                GUILayout.BeginHorizontal(new GUIStyle() { padding = new RectOffset(5, 5, 0, 0) });
                viewDialogs[i].wait = GUILayout.Toggle(viewDialogs[i].wait, new GUIContent("Wait"), new GUIStyle(EditorStyles.toggle) { fixedWidth = 50 });
                viewDialogs[i].time = EditorGUILayout.FloatField("Time", viewDialogs[i].time, new GUIStyle(EditorStyles.numberField) { fixedWidth = 20,  });
                viewDialogs[i].isFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(viewDialogs[i].isFoldout, new GUIContent($"{i + 1}"));
                viewDialogs[i].dialogCommand = (DialogCommand)EditorGUILayout.EnumPopup(viewDialogs[i].dialogCommand, new GUIStyle(EditorStyles.popup) { fixedWidth = 150 });
                bool addNewItem = false;
                if (!isFilting)
                {
                    GUILayout.Space(5);
                    addNewItem = GUILayout.Button("+", new GUIStyle(EditorStyles.miniButtonMid) { fixedWidth = 30 });
                    GUILayout.Space(5);
                }
                bool removeItem = GUILayout.Button("-", new GUIStyle(EditorStyles.miniButtonMid) { fixedWidth = 30 });

                GUILayout.EndHorizontal();
                if (addNewItem)
                {
                    viewDialogs.Insert(i + 1, new Script());
                    listLength++;
                }

                if (removeItem)
                {
                    viewDialogs.RemoveAt(i);
                    listLength--;
                }

                if (viewDialogs[i].isFoldout)
                {
                    switch (viewDialogs[i].dialogCommand)
                    {
                        case DialogCommand.Dialog:
                            dialogComponent(i);
                            break;
                        case DialogCommand.Tachie:
                            tachieComponent(i);
                            break;
                        case DialogCommand.TachieOff:
                            tachieOffComponent(i);
                            break;
                    }
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
            GUILayout.EndScrollView();

            bool save = GUILayout.Button("Save", new GUIStyle(EditorStyles.miniButtonMid) { fixedHeight = 30 });
            if (save)
            {
                if (referenceAsset != null)
                {
                    // 儲存
                    string path = AssetDatabase.GetAssetPath(referenceAsset);
                    File.WriteAllText(path, JsonConvert.SerializeObject(editorScipts));
                }
                else
                {
                    // 另存
                    string path = EditorUtility.SaveFilePanelInProject("Save as", scriptName, "json", "save file");
                    if (path.Length > 0)
                    {
                        File.WriteAllText(path, JsonConvert.SerializeObject(editorScipts));
                    }
                }
            }
        }

        [Obsolete]
        private void referenceComponent()
        {
            GUILayout.BeginHorizontal();
            referenceAsset = (TextAsset)EditorGUILayout.ObjectField("", referenceAsset, typeof(TextAsset));
            GUILayout.Space(5);
            bool clear = GUILayout.Button("Clear", new GUIStyle(EditorStyles.miniButtonMid) { fixedWidth = 80 });
            GUILayout.Space(5);
            bool apply = GUILayout.Button("Apply", new GUIStyle(EditorStyles.miniButtonMid) { fixedWidth = 80 });
            if (clear)
            {
                clearDialog();
            }
            if (apply)
            {
                if (referenceAsset != null)
                {
                    try
                    {
                        editorScipts = JsonConvert.DeserializeObject<List<Script>>(referenceAsset.text);
                        listLength = editorScipts.Count;
                        string[] paths = AssetDatabase.GetAssetPath(referenceAsset).Split("/");
                        scriptName = paths[paths.Length - 1].Split(".json")[0];
                    }
                    catch
                    {
                        clearDialog();
                        Debug.LogWarning("僅接受Json檔案");
                        Debug.LogWarning(referenceAsset.text);
                    }
                }
                else
                {
                    clearDialog();
                }
            }
            GUILayout.EndHorizontal();
        }

        private void searchComponent()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Search", new GUIStyle(EditorStyles.label) { fixedWidth = 150 });
            searchContent = EditorGUILayout.TextField(searchContent);
            GUILayout.Space(5);
            bool clear = GUILayout.Button("Clear", new GUIStyle(EditorStyles.miniButtonMid) { fixedWidth = 80 });
            GUILayout.Space(5);
            bool search = GUILayout.Button("Search", new GUIStyle(EditorStyles.miniButtonMid) { fixedWidth = 80 });
            if (clear)
            {
                searchContent = "";
                filter = "";
            }
            if (search)
            {
                filter = searchContent;
            }
            GUILayout.EndHorizontal();
        }

        [Obsolete]
        private void dialogComponent(int i)
        {
            viewDialogs[i].dialogData.contentLayout = (DialogContentLayout)EditorGUILayout.EnumPopup(viewDialogs[i].dialogData.contentLayout, new GUIStyle(EditorStyles.popup) { fixedWidth = 150 });

            GUILayout.BeginHorizontal();

            if (viewDialogs[i].dialogData.contentLayout == DialogContentLayout.AvatarLeft)
            {
                GUILayout.BeginVertical(new GUIStyle() { fixedWidth = 100 });
                viewDialogs[i].dialogData.avatar = (Sprite)EditorGUILayout.ObjectField("", viewDialogs[i].dialogData.avatar, typeof(Sprite));
                GUILayout.EndVertical();
            }

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Character Name", new GUIStyle(EditorStyles.label) { fixedWidth = 150 });
            viewDialogs[i].dialogData.characterName = EditorGUILayout.TextField(GUIContent.none, viewDialogs[i].dialogData.characterName, new GUIStyle(EditorStyles.textField) { fixedWidth = 200 });
            GUILayout.EndHorizontal();
            viewDialogs[i].dialogData.dialogContent = EditorGUILayout.TextArea(viewDialogs[i].dialogData.dialogContent, new GUIStyle(EditorStyles.textArea) { fixedHeight = 70 });
            GUILayout.EndVertical();

            if (viewDialogs[i].dialogData.contentLayout == DialogContentLayout.AvatarRight)
            {
                GUILayout.BeginVertical(new GUIStyle() { fixedWidth = 100 });
                viewDialogs[i].dialogData.avatar = (Sprite)EditorGUILayout.ObjectField("", viewDialogs[i].dialogData.avatar, typeof(Sprite));
                GUILayout.EndVertical();
            }

            GUILayout.EndHorizontal();
        }

        [Obsolete]
        private void tachieComponent(int i)
        {
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical(new GUIStyle() { fixedWidth = 100 });
            viewDialogs[i].tachieData.sprite = (Sprite)EditorGUILayout.ObjectField("", viewDialogs[i].tachieData.sprite, typeof(Sprite));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Id", new GUIStyle(EditorStyles.label) { fixedWidth = 20 });
            viewDialogs[i].tachieData.characterId = EditorGUILayout.TextField(GUIContent.none, viewDialogs[i].tachieData.characterId, new GUIStyle(EditorStyles.textField) { fixedWidth = 200 });
            GUILayout.EndHorizontal();

            GUILayout.Label("Position");

            GUILayout.BeginHorizontal();
            GUILayout.Label("X", new GUIStyle(EditorStyles.label) { fixedWidth = 20 });
            viewDialogs[i].tachieData.positionX = EditorGUILayout.FloatField(viewDialogs[i].tachieData.positionX, new GUIStyle(EditorStyles.numberField) { fixedWidth = 80 });
            if (viewDialogs[i].tachieData.positionX < 0) viewDialogs[i].tachieData.positionX = 0;
            if (viewDialogs[i].tachieData.positionX > 1) viewDialogs[i].tachieData.positionX = 1;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Y", new GUIStyle(EditorStyles.label) { fixedWidth = 20 });
            viewDialogs[i].tachieData.positionY = EditorGUILayout.FloatField(viewDialogs[i].tachieData.positionY, new GUIStyle(EditorStyles.numberField) { fixedWidth = 80 });
            if (viewDialogs[i].tachieData.positionY < 0) viewDialogs[i].tachieData.positionY = 0;
            if (viewDialogs[i].tachieData.positionY > 1) viewDialogs[i].tachieData.positionY = 1;
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }

        [Obsolete]
        private void tachieOffComponent(int i)
        {
            viewDialogs[i].tachieData.characterId = EditorGUILayout.TextField(GUIContent.none, viewDialogs[i].tachieData.characterId, new GUIStyle(EditorStyles.textField) { fixedWidth = 200 });
        }

        private void clearDialog()
        {
            referenceAsset = null;
            editorScipts = new List<Script>();
            listLength = 0;
        }
    }
}
