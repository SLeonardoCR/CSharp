using System;
using System.IO;

namespace Prueba_clase
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Programa para manejar archivos de texto:");
            StreamReader archivo = new StreamReader("C:\\Users\\serch\\Documents\\C Sharp\\Clase1\\Hola.txt");//leer archivo
            StreamWriter log = new StreamWriter("C:\\Users\\serch\\Documents\\C Sharp\\Clase1\\Log.txt"); // editar un archivo del programa 
            log.AutoFlush=true; //guardarlo 


            log.WriteLine("Campos Rangel Sergio Leonardo");// lo que aparece en el programa
            log.WriteLine("Lenguajes y Automatas 1");
            log.WriteLine("Archivo Hola.txt:");// se invoca al archivo que ya se tiene.

            int letras=0, numeros=0, espacios=0, caracteres=0;
            char c;
            while(!archivo.EndOfStream) //abrir archivo 
            {
                c=(char)archivo.Read();
                if(char.IsLetter(c) )
                {
                    char C=char.ToUpper(c); 
                    if(C=='A'||C=='E'||C=='I'||C=='O'||C=='U'){
                        log.Write("#");//encriptacion de documento
                    }else{
                        log.Write(c);
                    }
                    letras++;
                }
                else if(char.IsDigit(c)) // numeros 
                {
                    numeros++;
                    log.Write(c);
                }
                else if(char.IsWhiteSpace(c)) // espacios
                { 
                    espacios++;
                    log.Write(c);
                }
                else
                {
                    caracteres++;
                    log.Write(c);
                }
                Console.Write(c);
            }
            // impresiones de pantalla
            Console.WriteLine("\nLetras = " + letras);
            Console.WriteLine("\nNumeros = " + numeros);
            Console.WriteLine("\nEspacios = " + espacios);
            Console.WriteLine("\nCaracteres = " + caracteres); 
            
            archivo.Close();//Cierre de archivos.
            log.Close(); //cierre 

        }
    }
}
