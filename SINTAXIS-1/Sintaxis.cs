using System.IO;
using System;

namespace SINTAXIS_1
{
    public class Sintaxis : Lexico
    {
        public Sintaxis()
        {
            NextToken();
        }
        public void Match(string Espera)//compara el contenido contra la gramática
        {
            if (GetContenido() == Espera)
                NextToken();
            else
            {
                throw new Error("Error de Sintaxis: Se espera un "+ Espera, Linea, log);
            }
        }
        public void Match(Tipos Espera)//compara la clasificacion del token contra lo que espera la gramática
        {
            if (GetClasificacion() == Espera)
                NextToken();
            else
            {
                throw new Error("Error de Sintaxis: Se espera un "+ Espera, Linea, log);
            }
        }
    }
}