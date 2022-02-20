using System.IO;
using System;

namespace SINTAXIS_1
{
    public class Error : Exception
    {
        public Error(string message, int Linea, StreamWriter log)
        {
            Console.WriteLine(message+" Linea "+ Linea );
            log.WriteLine(message+" Linea "+ Linea );
        }
    }
}