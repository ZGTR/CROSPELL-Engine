using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpellingChecker.Keyboard;

namespace SpellingChecker.cs
{
    public class KeyboardHandler
    {
        public string[][][] KeyMap { private set; get; }

        // 13 coz the maximum distance in keyboard between any two keys is 13
        //public readonly int DefaultDistance = 13;
        //public readonly int ShiftEnabledWeight = 1;
        public readonly int ShiftKeyMarginWeight = 1;

        public KeyboardHandler(KeyboardLanguage language)
        {
            InitializeKeyMap(language);
        }

        //public KeyboardHandler(KeyboardLanguage language, int shiftEnabledWeight)
        //{
        //    //DefaultDistance = defaultDistance;
        //    ShiftEnabledWeight = shiftEnabledWeight;
        //    InitializeKeyMap(language);
        //}

        private void InitializeKeyMap(KeyboardLanguage language)
        {
            switch (language)
            {
                case KeyboardLanguage.English:
                    KeyMap = XMLKeyboardParser.ParseKeyboard("Databases\\KeyboardMaps\\KeyboardMapEnglish.xml", KeyboardLanguage.English);
                    break;
                case KeyboardLanguage.Arabic:
                    KeyMap = XMLKeyboardParser.ParseKeyboard("Databases\\KeyboardMaps\\KeyboardMapArabic.xml", KeyboardLanguage.Arabic);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("language");
            }
        }

        public int GetDistanceBetweenKeys(char char1, char char2)
        {
            try
            {
                int totalDistance = 0;
                KeyInfo key1 = new KeyInfo(char1, this.KeyMap);
                KeyInfo key2 = new KeyInfo(char2, this.KeyMap);
                if (key1.IsLetter)
                    totalDistance += AddWeightInMapWeightSymbol(key1, key2);
                else
                    totalDistance += AddWeightInMapWeightNotSymbol(key1, key2);
                return totalDistance;
            }
            catch (Exception)
            {}
            return 0;
        }

        private int AddWeightInMapWeightSymbol(KeyInfo key1, KeyInfo key2)
        {
            int weight = 0;
            int row1 = key1.PositionInKeyboardRow;
            int row2 = key2.PositionInKeyboardRow;

            int indexInRow1 = key1.PositionInRowIndex;
            int indexInRow2 = key2.PositionInRowIndex;

            if (row1 == row2)
            {
                if (indexInRow1 == indexInRow2)
                {
                    weight += 0;
                    if (key1.IsShiftEnabled == key2.IsShiftEnabled)
                        weight  += 0;
                    else
                        weight += 1;
                }
                else
                {
                    weight += Math.Abs(indexInRow2 - indexInRow1);
                    if (key1.IsShiftEnabled == key2.IsShiftEnabled)
                        weight += 0;
                    else
                        weight += 1;
                }
            }
            else
            {
                weight += Math.Abs(row2 - row1);
                int weightRow = Math.Abs(row2 - row1) + 1;
                if (indexInRow1 == indexInRow2)
                {
                    weight += this.ShiftKeyMarginWeight * (Math.Abs(row2 - row1));
                    if (key1.IsShiftEnabled == key2.IsShiftEnabled)
                        weight += 0;
                    else
                        weight += 1 * weightRow;
                }
                else
                {
                    weight += Math.Abs(indexInRow2 - indexInRow1);
                    if (key1.IsShiftEnabled == key2.IsShiftEnabled)
                        weight += 0;
                    else
                        weight += 1 * weightRow;
                }
            }
            return weight;
        }

        private int AddWeightInMapWeightNotSymbol(KeyInfo key1, KeyInfo key2)
        {
            int weight = 0;
            int row1 = key1.PositionInKeyboardRow;
            int row2 = key2.PositionInKeyboardRow;

            int indexInRow1 = key1.PositionInRowIndex;
            int indexInRow2 = key2.PositionInRowIndex;

            if (row1 == row2)
            {
                if (indexInRow1 == indexInRow2)
                {
                    throw new Exception("The word in dictionary contains not valid chars");
                }
                else
                {
                    weight += Math.Abs(indexInRow2 - indexInRow1);
                    if (key1.PositionInLayers == 0)
                        weight += 0;
                    else
                        weight += 1;
                }
            }
            else
            {
                weight += Math.Abs(row2 - row1);
                int weightRow = Math.Abs(row2 - row1) + 1;
                if (indexInRow1 == indexInRow2)
                {
                    weight += this.ShiftKeyMarginWeight;
                    if (key1.PositionInLayers == 0)
                        weight += 0;
                    else
                        weight += 1 * weightRow;
                }
                else
                {
                    weight += Math.Abs(indexInRow2 - indexInRow1);
                    if (key1.PositionInLayers == 0)
                        weight += 0;
                    else
                        weight += 1 * weightRow;
                }
            }
            return weight;
        }
    }
}