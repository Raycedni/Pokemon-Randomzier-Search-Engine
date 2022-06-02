using System;
using System.Collections.Generic;
using System.Linq;


namespace Pokemon_Typings.backend
{
    class PokemonDatabase
    {      

        private Dictionary<string, Pokemon> pokemonDic = new Dictionary<string, Pokemon>();

        public void fillDatabase(List<string> pokemonTypeList, List<string> pokemonMoveSetList)
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
