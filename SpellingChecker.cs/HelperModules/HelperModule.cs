using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace ZGTR_PorterAlgorithmApp
{
    public class HelperModule
    {
        public static List<String> CrackTextToWords(String filePath)
        {
            List<String> listOut = new List<string>();
            try
            {
                //
                Encoding arabicEncoding = Encoding.GetEncoding("Windows-1256");
                //StreamReader sw = new StreamReader(fileStream, arabicEncodingSW);
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                StringBuilder currensWord = new StringBuilder();
                List<byte> currentBytes = new List<byte>();
                while (true)
                {
                    try
                    {
                        int newByte = fileStream.ReadByte();
                        char ch = Char.ToLower((char)newByte);
                        if (!Char.IsSeparator(ch))
                        {
                            while (true)
                            {
                                try
                                {
                                    currensWord.Append(ch);
                                    currentBytes.Add((byte)newByte);
                                    newByte = fileStream.ReadByte();
                                    ch = Char.ToLower((char)newByte);
                                    if (!Char.IsLetter(ch))
                                    {
                                        string strArabic = arabicEncoding.GetString(currentBytes.ToArray());
                                        listOut.Add(strArabic);
                                        currensWord = new StringBuilder();
                                        currentBytes = new List<byte>();
                                        break;
                                    }
                                }
                                catch (Exception)
                                {
                                
                                    throw;
                                }
                            }
                        }
                        if (newByte < 0)
                            break;
                        }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception e )
            {
                //throw new Exception("Error while reading the file.");
            }
            return listOut;
        }

        private static int iFileCounter = 0;
        public static void WriteDistanceMatrixToFile(int[,] ints)
        {
            StreamWriter sw = new StreamWriter("File" + iFileCounter + ".txt");
            for (int i = 0; i < ints.GetLength(0); i++)
            {
                for (int j = 0; j < ints.GetLength(1); j++)
                {
                    sw.Write(ints[i,j] + "\t");
                }
                sw.Write(Environment.NewLine);
            }
            sw.Close();
            iFileCounter++;
        }
    }
}
