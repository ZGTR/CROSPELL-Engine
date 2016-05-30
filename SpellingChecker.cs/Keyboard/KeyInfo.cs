using System;
using System.Linq;

namespace SpellingChecker.Keyboard
{
    class KeyInfo
    {
        //public bool IsUpper { private set; get; }
        public char KeyChar { private set; get; }
        public bool IsShiftEnabled { private set; get; }
        public bool IsLetter { private set; get; }
        public int PositionInLayers { private set; get; }
        public int PositionInKeyboardRow { private set; get; }
        public int PositionInRowIndex { private set; get; }

        public KeyInfo(char keyChar, string[][][] keyMap)
        {
            this.KeyChar = keyChar;
            this.IsShiftEnabled = Char.IsUpper(keyChar);
            this.IsLetter = Char.IsLetter(keyChar);
           
            if (!this.IsShiftEnabled)
            {
                if (!SetIndecesFromMap(keyChar.ToString(), keyMap, 0))
                    SetIndecesFromMap(keyChar.ToString(), keyMap, 1);
            }
            else
            {
                if (!SetIndecesFromMap(keyChar.ToString(), keyMap, 1))
                    SetIndecesFromMap(keyChar.ToString(), keyMap, 0);
            }
        }

        private bool SetIndecesFromMap(string keyString, string[][][] keyMap, int layerIndex)
        {
            string[][] layerKeys = keyMap[layerIndex];
            for (int i = 0; i < layerKeys.Count(); i++)
            {
                for (int j = 0; j < layerKeys[i].Count(); j++)
                {
                    if(layerKeys[i][j] == keyString)
                    {
                        this.PositionInKeyboardRow = i;
                        this.PositionInRowIndex = j;
                        this.PositionInLayers = layerIndex;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
