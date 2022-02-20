using System.IO;
using System;

/*
Requerimientos: 
    1. Implementar la clasificacion del token: zona, ciclo, condicion
    2. Implementar la cadena con comilla simple ('')
    3. Implementar el operador ternario
    4. Implementar los operadores relacionales
    5. Implementar el numero con parte fraccional y exponencial
        *Cada requerimiento vale 20 puntos*
        Para el 28 de Septiembre de 2021
*/
namespace Lexico_1
{

    public class Lexico : Token
    {
        StreamReader archivo;
        StreamWriter log;
        public Lexico()
        {
            archivo = new StreamReader("C:\\Users\\serch\\Documents\\C Sharp\\Clase1\\Hola.txt");
            log = new StreamWriter("C:\\Users\\serch\\Documents\\C Sharp\\Clase1\\Log.txt");
            log.AutoFlush = true;
            log.WriteLine("Instituo Tecnologico de Queretaro");
            log.WriteLine("Campos Rangel Sergio Leonardo");
            log.WriteLine("-----------------------------------");
            log.WriteLine("Contenido de Hola.txt: ");
        }

        private string Exponente()
        {
            char c;
            string buffer = "";
            if (char.IsDigit(c = (char)archivo.Peek()))
            {
                while (char.IsDigit(c = (char)archivo.Peek()))
                {
                    buffer += c;
                    archivo.Read();
                }
            }
            else
            {
                log.WriteLine("ERROR LEXICO: Se espera un digito");
                Console.WriteLine("ERROR LEXICO: Se espera un digito");
            }
            return buffer;
        }

        public void NextToken()
        {
            char c;
            string Buffer = "";
            while (char.IsWhiteSpace(c = (char)archivo.Read()))
            {
            }
            Buffer += c;
            if (char.IsLetter(c))
            {
                SetClasificacion(Tipos.identificador);
                while (char.IsLetterOrDigit(c = (char)archivo.Peek()))
                {
                    Buffer += c;
                    archivo.Read();
                }

            }
            else if (char.IsDigit(c))
            {
                SetClasificacion(Tipos.numero);
                while (char.IsDigit(c = (char)archivo.Peek()))
                {
                    Buffer += c;
                    archivo.Read();
                }

                if (c == '.')
                {
                    Buffer += c;
                    archivo.Read();
                    if (char.IsDigit(c = (char)archivo.Peek()))
                    {
                        while (char.IsDigit(c = (char)archivo.Peek()))
                        {
                            Buffer += c;
                            archivo.Read();
                            c = (char)archivo.Peek();
                        }
                    }
                    else
                    {
                        log.WriteLine("ERROR LEXICO: Se espera un digito");
                        Console.WriteLine("ERROR LEXICO: Se espera un digito");
                    }
                }
                if (c == 'E' || c == 'e')
                {
                    Buffer += c;
                    archivo.Read();
                    if (((c = (char)archivo.Peek()) == '-') || ((c = (char)archivo.Peek()) == '+'))
                    {
                        Buffer += c;
                        archivo.Read();
                        Buffer += Exponente();
                    }
                    else if ((char.IsDigit(c = (char)archivo.Peek())))
                    {
                        Buffer += Exponente();
                    }
                    else
                    {
                        log.WriteLine("ERROR LEXICO: Se espera un digito");
                        Console.WriteLine("ERROR LEXICO: Se espera un digito");
                    }
                }
            }
            else if (c == '?')
            {
                SetClasificacion(Tipos.opTernario);
            }
            else if (c == ';')
            {
                SetClasificacion(Tipos.finSentencia);
            }
            else if (c == '"')
            {
                SetClasificacion(Tipos.Cadena);
                while ((c = (char)archivo.Peek()) != '"')
                {
                    Buffer += c;
                    archivo.Read();
                }
                archivo.Read();
                Buffer += c;
            }
            else if (c == '\'')
            {
                SetClasificacion(Tipos.Cadena);
                while ((c = (char)archivo.Peek()) != '\'')
                {
                    Buffer += c;
                    archivo.Read();
                }
                archivo.Read();
                Buffer += c;
            }
            else if (c == '&')
            {
                SetClasificacion(Tipos.caracter);
                if ((c = (char)archivo.Peek()) == '&')
                {
                    archivo.Read();
                    Buffer += c;
                    SetClasificacion(Tipos.opLogico);
                }
            }

            //Operadores relacionales
            else if (c == '=')
            {
                SetClasificacion(Tipos.asignacion);
                if ((c = (char)archivo.Peek()) == '=')
                {
                    archivo.Read();
                    Buffer += c;
                    SetClasificacion(Tipos.opRelacional);
                }
            }
            else if (c == '<')
            {
                SetClasificacion(Tipos.opRelacional);
                if ((c = (char)archivo.Peek()) == '=' || (c = (char)archivo.Peek()) == '>')
                {
                    archivo.Read();
                    Buffer += c;
                }
            }
            else if (c == '>')
            {
                SetClasificacion(Tipos.opRelacional);
                if ((c = (char)archivo.Peek()) == '=')
                {
                    archivo.Read();
                    Buffer += c;
                }
            }

            //Operadores Logicos
            else if (c == '|')
            {
                SetClasificacion(Tipos.caracter);
                if ((c = (char)archivo.Peek()) == '|')
                {
                    archivo.Read();
                    Buffer += c;
                    SetClasificacion(Tipos.opLogico);
                }
            }
            else if (c == '!')
            {
                SetClasificacion(Tipos.opLogico);
                if ((c = (char)archivo.Peek()) == '=')
                {
                    archivo.Read();
                    Buffer += c;
                    SetClasificacion(Tipos.opRelacional);
                }
            }
            //incTerminos
            else if (c == '+')
            {
                SetClasificacion(Tipos.opTermino);
                if ((c = (char)archivo.Peek()) == '+')
                {
                    archivo.Read();
                    Buffer += c;
                    SetClasificacion(Tipos.incTermino);
                }
                else if ((c = (char)archivo.Peek()) == '=')
                {
                    archivo.Read();
                    Buffer += c;
                    SetClasificacion(Tipos.incTermino);
                }
            }
            else if (c == '-')
            {
                SetClasificacion(Tipos.opTermino);
                if ((c = (char)archivo.Peek()) == '-')
                {
                    archivo.Read();
                    Buffer += c;
                    SetClasificacion(Tipos.incTermino);
                }
                if ((c = (char)archivo.Peek()) == '=')
                {
                    archivo.Read();
                    Buffer += c;
                    SetClasificacion(Tipos.incTermino);
                }
            }
            //4. incFactores
            else if (c == '*')
            {
                SetClasificacion(Tipos.caracter);
                if ((c = (char)archivo.Peek()) == '=')
                {
                    archivo.Read();
                    Buffer += c;
                    SetClasificacion(Tipos.incFactor);
                }
            }
            else if (c == '/')
            {
                SetClasificacion(Tipos.caracter);
                if ((c = (char)archivo.Peek()) == '=')
                {
                    archivo.Read();
                    Buffer += c;
                    SetClasificacion(Tipos.incFactor);
                }
            }
            else if (c == '%')
            {
                SetClasificacion(Tipos.caracter);
                if ((c = (char)archivo.Peek()) == '=')
                {
                    archivo.Read();
                    Buffer += c;
                    SetClasificacion(Tipos.incFactor);
                }
            }
            else if (c == ':')
            {
                SetClasificacion(Tipos.caracter);
                if ((c = (char)archivo.Peek()) == '=')
                {
                    archivo.Read();
                    Buffer += c;
                    SetClasificacion(Tipos.inicializacion);
                }
            }
            else
            {
                SetClasificacion(Tipos.caracter);

            }
            SetContenido(Buffer);
            if (GetClasificacion() == Tipos.identificador)
            {
                switch (GetContenido())
                {
                    case "char":
                    case "int":
                    case "float":
                        SetClasificacion(Tipos.tipoDato);
                        break;
                    //agregar los otros casos para zona, condicion, ciclo
                    case "private":
                    case "public":
                    case "protected":
                        SetClasificacion(Tipos.zona);
                        break;
                    case "if":
                    case "else":
                    case "switch":
                        SetClasificacion(Tipos.condicion);
                        break;
                    case "while":
                    case "for":
                    case "do":
                        SetClasificacion(Tipos.ciclo);
                        break;
                }
            }
            if (!FinArchivo())
            {
                log.WriteLine(GetContenido() + " <" + GetClasificacion() + ">");
            }

        }
        public bool FinArchivo()
        {
            return archivo.EndOfStream;
        }

    }
}