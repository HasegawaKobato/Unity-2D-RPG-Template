

using System;
using UnityEngine;

namespace KBT
{
    [Serializable]
    public class Script
    {
        public bool isFoldout = true;
        public DialogCommand dialogCommand = DialogCommand.Dialog;
        public bool wait = true;
        public float time = 1;
        public DialogData dialogData = new DialogData();
        public TachieData tachieData = new TachieData();
    }

    [Serializable]
    public class DialogData
    {
        public DialogContentLayout contentLayout = DialogContentLayout.None;
        public Sprite avatar = null;
        public string characterName = "";
        public string dialogContent = "";
    }

    [Serializable]
    public class TachieData
    {
        public Sprite sprite = null;
        public string characterId = "";
        public float positionX = 0.5f;
        public float positionY = 0.5f;
    }

    [Serializable]
    public class TachieOffData
    {
        public Sprite sprite = null;
        public string characterId = "";
        public float positionX = 0.5f;
        public float positionY = 0.5f;
    }
}
