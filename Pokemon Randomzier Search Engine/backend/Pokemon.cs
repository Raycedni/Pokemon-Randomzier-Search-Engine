using System;

namespace Pokemon_Typings.backend
{
    public class Pokemon
    {
        public int index;
        public string name;
        public string type;
        
        public string moves;
        public int level = -1;
        public string carriedItem = "no item";

        public string preEvolution = "";
        public string evolution = "";
        public int evolutionStage = 1;

        public Pokemon(string typeLogInfo, string moveLogInfo)
        {
            String[] seperator = { "|" };
            string tmpEvolution;

            String[] values = typeLogInfo.Split(seperator,
               StringSplitOptions.RemoveEmptyEntries);

            index = int.Parse(values[0]);
            name = values[1];
            type = values[2];


            moves = moveLogInfo;
            seperator.SetValue(" -> ", 0);
            values = moveLogInfo.Split(seperator,
               StringSplitOptions.RemoveEmptyEntries);

            tmpEvolution = values[1].Substring(0, values[1].IndexOf("\r"));
            if (tmpEvolution != " (no evolution)")
            {
                evolution = tmpEvolution;
            }
            
        }

        public string toString()
        {
            return name;
        }
    }
}
