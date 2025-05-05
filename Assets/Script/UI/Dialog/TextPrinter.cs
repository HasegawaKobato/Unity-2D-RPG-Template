using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace KBT
{
    [RequireComponent(typeof(Text))]
    public class TextPrinter : MonoBehaviour
    {
        public bool isPrintOver => tmpText.Length >= line.Length;
        public UnityEvent onPrintEnd = new UnityEvent();

        private Text dialogText => GetComponent<Text>();

        private float nextCharInterval = 0.01f;
        private float timer = 0;
        private string line = "";
        private string tmpText = "";


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!isPrintOver)
            {
                if (Time.time - timer > nextCharInterval)
                {
                    readyNextChar();
                }
            }
        }

        public void PlayLine(string _line)
        {
            line = _line;
            tmpText = "";
            dialogText.text = tmpText;
            readyNextChar();
        }

        public void PlayLine(string _line, float inSeconds)
        {
            line = _line;
            tmpText = "";
            dialogText.text = tmpText;
            nextCharInterval = inSeconds / _line.Length;
            readyNextChar();
        }

        private void readyNextChar()
        {
            timer = Time.time;
            nextChar();
        }

        private void printEnd()
        {
            onPrintEnd.Invoke();
        }

        private void nextChar()
        {
            if (!Application.isPlaying) return;
            tmpText += line[tmpText.Length];
            dialogText.text = tmpText;

            if (isPrintOver)
            {
                printEnd();
            }
        }

    }
}