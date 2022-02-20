using System.IO;
using System;

/* 

/*
Requerimientos: 
    1. Numeros /
    2. Operadores Lógicos /
    3. Operadores Relacionales /
    4. Operadores de termino e incremento de termino /
    5. Cadena /
    6. Comentarios
    7. Excepcion del error 
*/
namespace Lexico_2
{

    public class Lexico : Token
    {
        const int E = -2;
        const int F = -1;
        StreamReader archivo;
        StreamWriter log;
        public Lexico()
        {
            archivo = new StreamReader("D:\\USER\\C Sharp\\Lexico_2\\Hola.txt");
            log = new StreamWriter("D:\\USER\\C Sharp\\Lexico_2\\Log.txt");
            log.AutoFlush = true;
            log.WriteLine("Instituo Tecnologico de Queretaro");
            log.WriteLine("Campos Rangel Sergio Leonardo");
            log.WriteLine("-----------------------------------");
            log.WriteLine("Contenido de Hola.txt: ");
        }

        public int Automata(int EstadoActual, char Transicion)
        {
            int SigEstado = EstadoActual;
            switch (EstadoActual)
            {
                case 0:
                    if (char.IsWhiteSpace(Transicion))  //Inicial
                        SigEstado = 0;
                    else if (char.IsLetter(Transicion))//Identificadores
                        SigEstado = 1;
                    else if (char.IsDigit(Transicion))//Numeros
                        SigEstado = 2;
                    else if (Transicion == '=')//Asignacion
                        SigEstado = 8;
                    else if (Transicion == ':')//Inicializacion
                        SigEstado = 10;
                    else if (Transicion == ';')//Fin de Sentencia
                        SigEstado = 12;
                    else if (Transicion == '&')//Operadores Lógicos
                        SigEstado = 13;
                    else if (Transicion == '|')
                        SigEstado = 14;
                    else if (Transicion == '!')
                        SigEstado = 15;
                    else if (Transicion == '>')//Operadores Relacionales
                        SigEstado = 19;
                    else if (Transicion == '<')
                        SigEstado = 20;
                    else if (Transicion == '+')//Operador de incremento de termino
                        SigEstado = 23;
                    else if (Transicion == '-')
                        SigEstado = 24;
                    else if (Transicion == '*' || Transicion == '%')//11. Operadores incremento factor
                        SigEstado = 27;
                    else if (Transicion == '"')//Cadena
                        SigEstado = 29;
                    else if (Transicion == '\'')
                        SigEstado = 30;
                    else if (Transicion == '?')//Operador ternario
                        SigEstado = 32;
                    else if (Transicion == '/')
                        SigEstado = 34;
                    else
                        SigEstado = 33;
                    break;
                case 1:
                    SetClasificacion(Tipos.identificador);
                    if (!char.IsLetterOrDigit(Transicion))
                        SigEstado = F;
                    break;
                case 2:
                    SetClasificacion(Tipos.numero);
                    if (char.IsDigit(Transicion))
                        SigEstado = 2;
                    else if (Transicion == '.')
                        SigEstado = 3;
                    else if (Transicion == 'E' || Transicion == 'e')
                        SigEstado = 5;
                    else
                        SigEstado = F;
                    break;
                case 3:
                    if (char.IsDigit(Transicion))
                        SigEstado = 4;
                    else
                        SigEstado = E;
                    break;
                case 4:
                    if (char.IsDigit(Transicion))
                        SigEstado = 4;
                    else if (Transicion == 'E' || Transicion == 'e')
                        SigEstado = 5;
                    else
                        SigEstado = F;
                    break;
                case 5:
                    if (Transicion == '-' || Transicion == '+')
                        SigEstado = 6;
                    else if (char.IsDigit(Transicion))
                        SigEstado = 7;
                    else
                        SigEstado = E;
                    break;
                case 6:
                    if (char.IsDigit(Transicion))
                        SigEstado = 7;
                    else
                        SigEstado = E;
                    break;
                case 7:
                    if (char.IsDigit(Transicion))
                        SigEstado = 7;
                    else
                        SigEstado = F;
                    break;
                case 8:
                    SetClasificacion(Tipos.asignacion);
                    SigEstado = F;
                    if (Transicion == '=')
                        SigEstado = 9;
                    else
                        SigEstado = F;
                    break;
                case 9:
                    SetClasificacion(Tipos.opRelacional);
                    SigEstado = F;
                    break;
                case 10:
                    SetClasificacion(Tipos.caracter);
                    if (Transicion == '=')
                        SigEstado = 11;
                    else
                        SigEstado = F;
                    break;
                case 11:
                    SetClasificacion(Tipos.inicializacion);
                    SigEstado = F;
                    break;
                case 12:
                    SetClasificacion(Tipos.finSentencia);
                    SigEstado = F;
                    break;
                case 13:
                    SetClasificacion(Tipos.caracter);
                    if (Transicion == '&')
                        SigEstado = 16;
                    else
                        SigEstado = F;
                    break;
                case 14:
                    SetClasificacion(Tipos.caracter);
                    if (Transicion == '|')
                        SigEstado = 17;
                    else
                        SigEstado = F;
                    break;
                case 15:
                    SetClasificacion(Tipos.opLogico);
                    if (Transicion == '=')
                        SigEstado = 18;
                    else
                        SigEstado = F;
                    break;
                case 16:
                case 17:
                    SetClasificacion(Tipos.opLogico);
                    SigEstado = F;
                    break;
                case 18:
                    SetClasificacion(Tipos.opRelacional);
                    SigEstado = F;
                    break;
                case 19:
                    SetClasificacion(Tipos.opRelacional);
                    if (Transicion == '=')
                        SigEstado = 21;
                    else
                        SigEstado = F;
                    break;
                case 20:
                    SetClasificacion(Tipos.opRelacional);
                    if (Transicion == '=' || Transicion == '>')
                        SigEstado = 22;
                    else
                        SigEstado = F;
                    break;
                case 21:
                case 22:
                    SigEstado = F;
                    break;
                case 23:
                    SetClasificacion(Tipos.opTermino);
                    if (Transicion == '+' || Transicion == '=')
                        SigEstado = 25;
                    else
                        SigEstado = F;
                    break;
                case 24:
                    SetClasificacion(Tipos.opTermino);
                    if (Transicion == '-' || Transicion == '=')
                        SigEstado = 26;
                    else
                        SigEstado = F;
                    break;
                case 25:
                case 26:
                    SetClasificacion(Tipos.incTermino);
                    SigEstado = F;
                    break;
                case 27:
                    SetClasificacion(Tipos.opFactor);
                    if (Transicion == '=')
                        SigEstado = 28;
                    else
                        SigEstado = F;
                    break;
                case 28:
                    SetClasificacion(Tipos.incFactor);
                    SigEstado = F;
                    break;
                case 29:
                    if (Transicion == '"')
                        SigEstado = 31;
                    else if (FinArchivo()||Transicion == 10)
                        SigEstado = E;
                    else
                        SigEstado = 29;
                    break;
                case 30:
                    if (Transicion == '\'')
                        SigEstado = 31;
                    else if (FinArchivo())
                        SigEstado = E;
                    else
                        SigEstado = 30;
                    break;
                case 31:
                    SetClasificacion(Tipos.Cadena);
                    SigEstado = F;
                    break;
                case 32:
                    SetClasificacion(Tipos.opTernario);
                    SigEstado = F;
                    break;
                case 33:
                    SetClasificacion(Tipos.caracter);
                    SigEstado = F;
                    break;
                case 34:
                    SetClasificacion(Tipos.opFactor);
                    if (Transicion == '=')
                        SigEstado = 35;
                    else if (Transicion == '/')
                        SigEstado = 36;
                    else if (Transicion == '*')
                        SigEstado = 37;
                    else
                        SigEstado = F;
                    break;
                case 35:
                    SetClasificacion(Tipos.incFactor);
                    SigEstado = F;
                    break;
                case 36:
                    if (Transicion == 10)
                        SigEstado = 0; 
                    else
                        SigEstado = 36;
                    break;
                case 37:
                    if (FinArchivo())
                        SigEstado = E;
                    else if (Transicion == '*')
                        SigEstado = 38;
                    else
                        SigEstado = 37;
                    break;
                case 38:
                    if(Transicion == '*')
                        SigEstado = 38;
                    else if (Transicion == '/')
                        SigEstado = 0;
                    else if (FinArchivo())
                        SigEstado = E;
                    else
                        SigEstado = 37;
                    break;

            }
            return SigEstado;
        }

        public void NextToken()
        {
            char c;
            string Buffer = "";
            int Estado = 0;


            while (Estado >= 0)
            {
                c = (char)archivo.Peek();
                Estado = Automata(Estado, c);
                if (Estado >= 0)
                {
                    archivo.Read();
                    if (Estado > 0)
                        Buffer += c;
                    else
                        Buffer = "";
                }
            }

            

            SetContenido(Buffer);
            if (Estado == E)
            {
                if(Buffer [0] == '"' || Buffer [0] == '\'')
                {
                    log.WriteLine("Error Lexico: No se cerro la cadena");
                    Console.WriteLine("Error Lexico: No se cerro la cadena");
                }
                else if (char.IsDigit(Buffer [0]))
                {
                    log.WriteLine("\t>>" + GetContenido() + "<< Error Lexico: Se espera un digito");
                    Console.WriteLine("\t>>" + GetContenido() + "<< Error Lexico: Se espera un digito");
                }
                else
                {
                    log.WriteLine("\t>>" + GetContenido() + "<< Error Lexico");
                    Console.WriteLine("\t>>" + GetContenido() + "<< Error Lexico");
                }
            }
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
                log.WriteLine(GetContenido() + " <" + GetClasificacion() + ">");
        }
        public bool FinArchivo()
        {
            return archivo.EndOfStream;
        }

    }
}