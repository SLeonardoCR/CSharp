using System;

namespace Lexico_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Lexico L = new Lexico();

            while(!L.FinArchivo())
            {
                L.NextToken();
            }
        }
    }
}
