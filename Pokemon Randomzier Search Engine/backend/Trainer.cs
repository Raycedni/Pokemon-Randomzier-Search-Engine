using Pokemon_Typings.backend;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Pokemon_Randomzier_Search_Engine.backend
{
    public class Trainer
    {
        public string trainerNameOriginal;
        public string trainerNameRandomized;

        public List<Pokemon> pokemonList = new List<Pokemon>();

        public int encounterNr;

        public Trainer(string trainerInfo, RandomizerDatabase pokemonDatabase)
        {
            int i;
            string[] seperator = { "(", " => ", ")", " - "};

            string[] values = trainerInfo.Split(seperator,
               StringSplitOptions.RemoveEmptyEntries);

            trainerNameOriginal = values[1];
            trainerNameRandomized = values[2];

            for(i = 0; i < values.Length; i++)
            {
                if (values[i].Contains("Lv"))
                    break;
            }

            foreach(string pokemon in getPokemonNamesFromTrainerInfo(values[i]))
            {
                pokemonList.Add(pokemonDatabase.GetPokemon(pokemon.Split(' ')[0]));
                pokemonList[pokemonList.Count - 1].level = int.Parse(pokemon.Split(' ')[1]);
            }
            
        }

        private List<string> getPokemonNamesFromTrainerInfo(string trainerInfo)
        {
            List<string> tmpList = new List<string>();

            string[] seperator = { "," };

            string[] pokemonNameAndLevel = trainerInfo.Split(seperator,
               StringSplitOptions.RemoveEmptyEntries);

            

            seperator.SetValue(" ", 0);

            string[] splitInfo;

            string pokemonName, levelInfo;

            foreach (string s in pokemonNameAndLevel)
            {
                splitInfo = s.Split(seperator,
                                    StringSplitOptions.RemoveEmptyEntries);

                pokemonName = splitInfo[0];

                if(pokemonName.Contains("@"))
                {
                    pokemonName = pokemonName.Split('@')[0];
                }

                levelInfo = splitInfo[1];

                levelInfo = int.Parse(Regex.Replace(levelInfo, "[^0-9]", "")).ToString();
                

                tmpList.Add(pokemonName + " " + levelInfo);
            }


            return tmpList;
        }
    }
}
