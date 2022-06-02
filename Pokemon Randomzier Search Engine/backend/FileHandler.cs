using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace Pokemon_Typings.backend
{
    class FileHandler
    {
        public string getFileContent(string inputFile)
        {
            if (File.Exists(inputFile) && inputFile.EndsWith(".log"))
            {
                using (StreamReader r = File.OpenText(inputFile))
                {
                    return r.ReadToEnd();
                }
            }
            else
            {
                return "File not found";
            }
        }

        public List<string> getPokemonBaseStats_Types(string ndsLog)
        {
            string typeList = between(ndsLog, "--Pokemon Base Stats & Types--", "--Removing Impossible Evolutions--").Replace(" ", String.Empty);

            String[] spearator = {"\r\n" };

            // using the method
            List<string> strlist = typeList.Split(spearator,
               StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            strlist.RemoveAt(0);

            return strlist;
        }

        public List<string> getPokemonMoves(string ndsLog)
        {
            string typeList = between(ndsLog, "--Pokemon Movesets--", "--Trainers Pokemon--");

            String[] spearator = { "\r\n\r\n" };

            // using the method
            String[] strlist = typeList.Split(spearator,
               StringSplitOptions.RemoveEmptyEntries);

            return strlist.ToList();
        }

            private string between(string STR, string FirstString, string LastString)
        {
            string FinalString;
            int Pos1 = STR.IndexOf(FirstString) + FirstString.Length;
            int Pos2 = STR.IndexOf(LastString);
            try
            {

                FinalString = STR.Substring(Pos1, Pos2 - Pos1);
            }catch
            {
                throw new Exception("Ungültiger Dateiinhalt");
            }
            return " " + FinalString.Trim();
        }
    }
}
