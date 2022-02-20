using System;

namespace Lexico_1
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
