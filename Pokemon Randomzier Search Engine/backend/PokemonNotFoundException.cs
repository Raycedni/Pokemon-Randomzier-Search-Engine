using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Typings.backend
{
    class PokemonNotFoundException : Exception
    {
        public PokemonNotFoundException(string message):base(message)
        {

        }
    }
}
