﻿using System;

namespace Evalua
{
    class Program
    {
        static void Main(string[] args)
        {
            Lenguaje L = new Lenguaje();
    
            try
            {
                L.Programa();
            }
            catch (Exception)
            {
                Console.WriteLine("Fin de Compilacion. Verifique el Código");
            }

            //while(!L.FinArchivo())
            //    L.NextToken();
        }
    }
}
