using System.IO;
using System;

namespace Evalua
{

    public class Lexico : Token
    {
        protected int Linea;
        const int E = -2;
        const int F = -1;
        int[,] TranD =
        {
   //WS,EF,EL, L, D, ., E, +, -, =, :, ;, &, |, !, >, <, *, %, ", ', ?,La, /
    { 0, F, 0, 1, 2,33, 1,23,24, 8,10,12,13,14,15,19,20,27,27,29,30,32,33,34},//Estado 0
    { F, F, F, 1, 1, F, 1, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 1
    { F, F, F, F, 2, 3, 5, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 2
    { E, E, E, E, 4, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E},//Estado 3
    { F, F, F, F, 4, F, 5, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 4
    { E, E, E, E, 7, E, E, 6, 6, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E},//Estado 5
    { E, E, E, E, 7, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E},//Estado 6
    { F, F, F, F, 7, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 7
    { F, F, F, F, F, F, F, F, F, 9, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 8
    { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 9
    { F, F, F, F, F, F, F, F, F,11, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 10
    { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 11
    { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 12
    { F, F, F, F, F, F, F, F, F, F, F, F,16, F, F, F, F, F, F, F, F, F, F, F},//Estado 13
    { F, F, F, F, F, F, F, F, F, F, F, F, F,17, F, F, F, F, F, F, F, F, F, F},//Estado 14
    { F, F, F, F, F, F, F, F, F,18, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 15
    { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 16
    { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 17
    { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 18
    { F, F, F, F, F, F, F, F, F,21, F, F, F, F, F,39, F, F, F, F, F, F, F, F},//Estado 19
    { F, F, F, F, F, F, F, F, F,22, F, F, F, F, F,22,40, F, F, F, F, F, F, F},//Estado 20
    { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 21
    { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 22
    { F, F, F, F, F, F, F,25, F,25, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 23
    { F, F, F, F, F, F, F, F,26,26, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 24
    { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 25
    { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 26
    { F, F, F, F, F, F, F, F, F,28, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 27
    { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 28
    {29, E,29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,31,29,29,29,29},//Estado 29
    {30, E,30,30,30,30,30,30,30,30,30,30,30,30,30,30,30,30,30,30,31,30,30,30},//Estado 30
    { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 31
    { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 32
    { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 33
    { F, F, F, F, F, F, F, F, F,35, F, F, F, F, F, F, F,37, F, F, F, F, F,36},//Estado 34
    { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 35
    {36, 0, 0,36,36,36,36,36,36,36,36,36,36,36,36,36,36,36,36,36,36,36,36,36},//Estado 36
    {37, E,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,38,37,37,37,37,37,37},//Estado 37
    {37, E,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,38,37,37,37,37,37, 0},//Estado 38
    { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 39
    { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F} //Estado 40
   //WS,EF,EL, L, D, ., E, +, -, =, :, ;, &, |, !, >, <, *, %, ", ', ?,La, /
        };
        private StreamReader archivo;
        protected StreamWriter log;
        public Lexico()
        {
            Linea = 1;
            archivo = new StreamReader("D:\\USER\\Programming\\C Sharp\\Archivos\\Prueba.cs");
            log = new StreamWriter("D:\\USER\\Programming\\C Sharp\\Archivos\\Prueba.log");
            log.AutoFlush = true;
            log.WriteLine("Instituto Tecnologico de Queretaro");
            log.WriteLine("Campos Rangel Sergio Leonardo");
            log.WriteLine("-----------------------------------");
            log.WriteLine("Contenido de Prueba.cs: ");
            log.WriteLine("   " + DateTime.Now.ToString("dd/MM/yyyy") + "  " + DateTime.Now.ToString("hh:mm tt"));
            log.WriteLine("-----------------------------------");
        }

        public int Columna(char Transicion)
        {

            if (Transicion == 10)
            {
                Linea++;
                return 2;
            }
            else if (FinArchivo())
                return 1;
            else if (char.IsWhiteSpace(Transicion))
                return 0;
            else if (Transicion == 'E' || Transicion == 'e')
                return 6;
            else if (char.IsLetter(Transicion))
                return 3;
            else if (char.IsDigit(Transicion))
                return 4;
            else if (Transicion == '.')
                return 5;
            else if (Transicion == '+')
                return 7;
            else if (Transicion == '-')
                return 8;
            else if (Transicion == '=')
                return 9;
            else if (Transicion == ':')
                return 10;
            else if (Transicion == ';')
                return 11;
            else if (Transicion == '&')
                return 12;
            else if (Transicion == '|')
                return 13;
            else if (Transicion == '!')
                return 14;
            else if (Transicion == '>')
                return 15;
            else if (Transicion == '<')
                return 16;
            else if (Transicion == '*')
                return 17;
            else if (Transicion == '%')
                return 18;
            else if (Transicion == '"')
                return 19;
            else if (Transicion == '\'')
                return 20;
            else if (Transicion == '?')
                return 21;
            else if (Transicion == '/')
                return 23;
            else
                return 22;
            //WS,EOF,EOL,Let,Dig,.,E,+,-,=,:,;,&,|,!,>,<,*,%,",',?,La,/
        }

        private void Clasifica(int Estado)
        {
            switch (Estado)
            {
                case 1:
                    SetClasificacion(Tipos.identificador);
                    break;
                case 2:
                    SetClasificacion(Tipos.numero);
                    break;
                case 8:
                    SetClasificacion(Tipos.asignacion);
                    break;
                case 9:
                case 18:
                case 19:
                case 20:
                    SetClasificacion(Tipos.opRelacional);
                    break;
                case 10:
                case 13:
                case 14:
                case 33:
                    SetClasificacion(Tipos.caracter);
                    break;
                case 11:
                    SetClasificacion(Tipos.inicializacion);
                    break;
                case 12:
                    SetClasificacion(Tipos.finSentencia);
                    break;
                case 15:
                case 16:
                case 17:
                    SetClasificacion(Tipos.opLogico);
                    break;
                case 23:
                case 24:
                    SetClasificacion(Tipos.opTermino);
                    break;
                case 25:
                case 26:
                    SetClasificacion(Tipos.incTermino);
                    break;
                case 27:
                case 34:
                    SetClasificacion(Tipos.opFactor);
                    break;
                case 28:
                case 35:
                    SetClasificacion(Tipos.incFactor);
                    break;
                case 31:
                    SetClasificacion(Tipos.Cadena);
                    break;
                case 32:
                    SetClasificacion(Tipos.opTernario);
                    break;
                case 39:
                    SetClasificacion(Tipos.opFlujoSalida);
                    break;
                case 40:
                    SetClasificacion(Tipos.opFlujoEntrada);
                    break;
            }
        }

        public void NextToken()
        {
            char c;
            string Buffer = "";
            int Estado = 0;

            while (Estado >= 0)
            {
                c = (char)archivo.Peek();
                Estado = TranD[Estado, Columna(c)];
                if (Estado >= 0)
                {

                    archivo.Read();
                    if (Estado > 0)
                    {
                        Clasifica(Estado);
                        Buffer += c;
                    }
                    else
                        Buffer = "";
                }
            }
            if (Estado == E)
            {

                if (Buffer[0] == '"' || Buffer[0] == '\'')
                {
                    //levantar excepcion correspondiente
                    throw new Error("Error Lexico: No se cerro la cadena", Linea, log);
                }
                else if (char.IsDigit(Buffer[0]))
                {
                    throw new Error("Error Lexico: Se espera un digito", Linea, log);
                }
                else
                {
                    throw new Error("Error Lexico: ", Linea, log);
                }
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