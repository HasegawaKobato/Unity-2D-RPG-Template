using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace KBT
{
    [RequireComponent(typeof(Text))]
    public class TextPrinter : MonoBehaviour
    {
        private Text dialogText => GetComponent<Text>();
        public bool isPrintOver => tmpText.Length >= line.Length;

        private string line = "";
        private string tmpText = "";


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PlayLine(string _line)
        {
            line = _line;
            tmpText = "";
            dialogText.text = tmpText;
            startPrint();
        }

        private async void startPrint()
        {
            while (!isPrintOver)
            {
                nextChar();
                await Task.Delay(20);
            }
            printEnd();
        }

        private async void printEnd()
        {
            await Task.Delay(1000);
        }

        private void nextChar()
        {
            if (!Application.isPlaying) return;
            tmpText += line[tmpText.Length];
            dialogText.text = tmpText;
        }

    }
}