using Pokemon_Randomzier_Search_Engine.backend;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Pokemon_Typings.backend
{
    public class RandomizerDatabase
    {      

        private Dictionary<string, Pokemon> pokemonDic = new Dictionary<string, Pokemon>();
        private Dictionary<string, List<Trainer>> trainerDic = new Dictionary<string, List<Trainer>>();

        public void fillPokemonDatabase(List<string> pokemonTypeList, List<string> pokemonMoveSetList)
        {
            pokemonDic.Clear();

            Pokemon tmpPokemon;
            Pokemon previousPokemon;

            for(int i=0; i <= pokemonTypeList.Count-1; i++)
            {
                try
                {
                    tmpPokemon = new Pokemon(pokemonTypeList[i], pokemonMoveSetList[i]);

                    if (!pokemonDic.Keys.Contains(tmpPokemon.name))
                    {
                        if (pokemonDic.Count > 0)
                        {
                            previousPokemon = pokemonDic.ElementAt(pokemonDic.Count - 1).Value;
                            if (previousPokemon.evolution == tmpPokemon.name)
                            {
                                tmpPokemon.preEvolution = previousPokemon.name;
                                tmpPokemon.evolutionStage = getPokemonEvolutionStage(tmpPokemon);
                            }
                        }
                        pokemonDic.Add(tmpPokemon.name, tmpPokemon);
                    }
                }catch
                {

                }
            }
        }

        public void fillTrainerDatabase(List<string> trainerList)
        {
            trainerDic.Clear();

            Trainer tmpTrainer;

            foreach (string trainerString in trainerList)
            {
                //Trainer wird von string ausgelesen
                tmpTrainer = new Trainer(trainerString, this);

                //Wird geprüft ob für Trainer bereits ein Encounter existiert
                if (trainerDic.ContainsKey(tmpTrainer.trainerNameRandomized))
                {
                    //Encounter wird dem Trainer hinzugefügt
                    trainerDic[tmpTrainer.trainerNameRandomized].Add(tmpTrainer);
                }
                else
                {
                    //Neues Encounter wird erstellt und dann dem Dictionary hinzugefügt
                    trainerDic.Add(tmpTrainer.trainerNameRandomized, new List<Trainer>());
                    trainerDic[tmpTrainer.trainerNameRandomized].Add(tmpTrainer);
                    tmpTrainer = null;
                }
            }
        }

        public List<Trainer> getTrainer(string trainerName)
        {
            if (trainerDic.ContainsKey(trainerName))
                return trainerDic[trainerName];
            else
                throw new Exception("Trainer: " + trainerName + " nicht gefunden");            
        }

        public List<String> getTrainerNames()
        {
            return trainerDic.Keys.ToList();
        }

        private int getPokemonEvolutionStage(Pokemon pokemon)
        {
            try
            {
                return GetPokemon(pokemon.preEvolution).evolutionStage + 1;
            }
            catch
            {
                return 1;
            }
        }

        private int getIndexOfPokemon(Pokemon pokemon)
        {
            return pokemon.index-1;
        }

        private Pokemon getFirstEvolution(Pokemon pokemon)
        {
            while (pokemon.evolutionStage > 1)
            {
                pokemon = GetPokemon(pokemon.preEvolution);
            }

            return pokemon;
        }

        public string getPokemonType(string pokemonName)
        {
            if (pokemonDic.Keys.Contains(pokemonName))
            {

                return pokemonDic[pokemonName].type;
            }
            else
            {
                return "not found";
            }
        }

        public string getPokemonMoves(string pokemonName)
        {
            if (pokemonDic.Keys.Contains(pokemonName))
            {

                return pokemonDic[pokemonName].moves;
            }
            else
            {
                return "not found";
            }
        }

        public Pokemon GetPokemon(string name)
        {
            if (pokemonDic.ContainsKey(name))
                return pokemonDic[name];
            else
                throw new PokemonNotFoundException("Pokemon: " + name + " nicht gefunden");
        }

        public List<Pokemon> getPokemonFamily(string pokemonName)
        {
            return getPokemonFamily(GetPokemon(pokemonName));
        }

        public List<Pokemon> getPokemonFamily(Pokemon pokemon)
        {     
            List<Pokemon> returnList = new List<Pokemon>();

            pokemon = getFirstEvolution(pokemon);

            returnList.Add(pokemon);

            while(pokemon.evolution != "")
            {
                pokemon = GetPokemon(pokemon.evolution);
                returnList.Add(pokemon);
            }

            return returnList;
        }

        public Pokemon getPokemonAtIndex(int index)
        {
            return pokemonDic.ElementAt(index).Value;
        }

        public List<string> getRegisteredPokemon()
        {
            return pokemonDic.Keys.ToList();
        }
    }
}
