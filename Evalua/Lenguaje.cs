using System;
using System.Collections.Generic;

namespace Evalua
{

    /*
    Requerimientos: 
                1. Ajustar salida del "printf": quitar las " y ejecutar las sentencias de escape
                (\n \t)
                Asociar la lista de variables a las salidas %
            2. Cuando una varible no esté declarada levantar la excepción (Tipos.identificador)  
                3. Agregar "do while"
                4. Controlar ejecución del if anidado
                resolver que no imprima el segundo if cuando el primero no se cumple
    */
    public class Lenguaje : Sintaxis
    {
        List<Variable> LV;
        Stack<float> SE;

        List<string> LS;
        public Lenguaje()
        {
            LV = new List<Variable>();
            SE = new Stack<float>();
            LS = new List<string>();
        }
        //Programa -> Librerias Variables Main
        public void Programa()
        {
            Librerias();
            Variables();
            Main();
            ImprimeLista();
        }

        //Librerias	->	#include<identificador(.h)?> Librerias?     .h optativo
        private void Librerias()
        {
            Match("#");
            Match("include");
            Match("<");
            Match(Tipos.identificador);
            if (GetContenido() == ".")
            {
                Match(".");
                Match("h");
            }
            Match(">");
            if (GetContenido() == "#")
                Librerias();
        }

        private Variable.TDatos StringToEnum(string tipo)
        {
            switch (tipo)
            {
                case "char": return Variable.TDatos.CHAR;
                case "int": return Variable.TDatos.INT;
                case "float": return Variable.TDatos.FLOAT;
                default: return Variable.TDatos.SinTipo;
            }
        }

        //Variables -> tipoDato ListaIdentificadores; Variables?
        private void Variables()
        {
            Variable.TDatos tipo = Variable.TDatos.CHAR;
            tipo = StringToEnum(GetContenido());
            Match(Tipos.tipoDato);
            ListaIdentificadores(tipo);
            Match(Tipos.finSentencia);
            if (GetClasificacion() == Tipos.tipoDato)
                Variables();
        }
        private void ImprimeLista()
        {
            log.WriteLine("Lista de Variables");
            foreach (Variable L in LV)
            {
                log.WriteLine(L.getNombre() + " " + L.getTipoDato() + " " + L.getValor());
            }
        }
        private bool Existe(string nombre)
        {
            foreach (Variable L in LV)
            {
                if (L.getNombre() == nombre)
                    return true;
                //log.WriteLine(L.getNombre() +" " + L.getTipoDato() +" " + L.getValor());
            }
            return false;
        }

        private void Modifica(string nombre, float valor)
        {
            foreach (Variable L in LV)
            {
                if (L.getNombre() == nombre)
                    L.setValor(valor);
            }
        }

        private float GetValor(string nombre)
        {
            foreach (Variable L in LV)
            {
                if (L.getNombre() == nombre)
                    return L.getValor();
            }
            return 0;
        }
        //ListaIdentificadores -> identificador(,ListaIdentificadores)?
        private void ListaIdentificadores(Variable.TDatos tipo)
        {
            if (tipo != Variable.TDatos.SinTipo)
            {
                if (!Existe(GetContenido()))
                    LV.Add(new Variable(GetContenido(), tipo));
                else
                    throw new Error("Error de Sintaxis: Variable duplicada " + GetContenido(), Linea, log);
            }
            else
            {
                LS.Add(GetValor(GetContenido()).ToString());
            }
            Match(Tipos.identificador);
            if (GetContenido() == ",")
            {
                Match(",");
                ListaIdentificadores(tipo);
            }
        }

        //Main	->	void main() BloqueInstrucciones
        private void Main()
        {
            Match("void");
            Match("main");
            Match("(");
            Match(")");
            BloqueInstrucciones(true);
        }

        //BloqueInstrucciones  -> {ListaInstrucciones}
        private void BloqueInstrucciones(bool Ejecuta)
        {

            Match("{");
            ListaInstrucciones(Ejecuta);
            Match("}");
        }

        //ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones(bool Ejecuta)
        {
            Instruccion(Ejecuta);
            if (GetContenido() != "}")
                ListaInstrucciones(Ejecuta);
        }

        //Instruccion -> Printf | Scanf | If | For | While 
        private void Instruccion(bool Ejecuta)
        {
            if (GetContenido() == "printf")
                Printf(Ejecuta);
            else if (GetContenido() == "scanf")
                Scanf(Ejecuta);
            else if (GetContenido() == "if")
                If(Ejecuta);
            else if (GetContenido() == "for")
                For(Ejecuta);
            else if (GetContenido() == "while")
                While(Ejecuta);
            else if (GetContenido() == "switch")
                Switch(Ejecuta);
            else if (GetContenido() == "do")
                DoWhile(Ejecuta);
            else
                Asignacion(Ejecuta);
        }

        //Printf -> printf(Cadena (,ListaIdentificadores?));
        private void Printf(bool Ejecuta)
        {
            Match("printf");
            Match("(");
            string cadena = GetContenido().Replace("\"", "").Replace("\\t", "\t").Replace("\\n", "\n");
            Match(Tipos.Cadena);
            if (GetContenido() == ",")
            {
                Match(",");
                LS.Clear();
                ListaIdentificadores(Variable.TDatos.SinTipo);
                //Console.WriteLine("La variable a = " + cadena);
                //Console.WriteLine("La lista de valores es: ");
                foreach (string L in LS)
                {
                    //Console.WriteLine(L);
                    if (cadena.Contains("%f") || cadena.Contains("%d"))
                    {
                        cadena = cadena.Remove(cadena.IndexOf('%')) + L + cadena.Substring(cadena.IndexOf('%') + 2);
                    }
                    //Console.WriteLine(cadena);
                }
            }
            Match(")");
            Match(Tipos.finSentencia);
            if (Ejecuta)
                Console.Write(cadena);
        }

        //Scanf -> scanf(Cadena, ListaDeAmpersas);
        private void Scanf(bool Ejecuta)
        {
            Match("scanf");
            Match("(");
            Match(Tipos.Cadena);
            Match(",");
            ListaDeAmpersas(Ejecuta);
            Match(")");
            Match(Tipos.finSentencia);
        }

        //ListaDeAmpersas -> &identificador (,ListaDeAmpersas)? 
        private void ListaDeAmpersas(bool Ejecuta)
        {
            Match("&");
            if (!Existe(GetContenido())) //Busca en Match(Tipos.identificador) y agregar excepcion
                throw new Error("Error de Sintaxis: Variable "+ GetContenido() +" no declarada ", Linea, log);
            if (Ejecuta)
            {
                float valor = float.Parse(Console.ReadLine());
                Modifica(GetContenido(), valor);
            }
            Match(Tipos.identificador);
            if (GetContenido() == ",")
            {
                Match(",");
                ListaDeAmpersas(Ejecuta);
            }
        }

        //If -> if(Condicion) BloqueInstrucciones | Instruccion (else BloqueInstrucciones | Instruccion)?
        private void If(bool Ejecuta)
        {
            Match("if");
            Match("(");

            bool Evalua = Condicion();

            if (Ejecuta == false)
                Evalua = false;

            Console.WriteLine(Evalua);
            Match(")");
            if (GetContenido() == "{")
            {
                BloqueInstrucciones(Evalua);
            }
            else
                Instruccion(Evalua);

            if (Ejecuta == false)
                Evalua = true;

            if (GetContenido() == "else")
            {
                Match("else");
                if (GetContenido() == "{")
                {
                    BloqueInstrucciones(!Evalua);
                }
                else
                    Instruccion(!Evalua);
            }
        }

        //Condicion	-> numero | identificador opRelacional numero | identificador
        //Condicion	-> Expresion opLogico Expresion
        //Condicion	-> (!)? Expresion opLogico Expresion
        private bool Condicion()
        {
            if (GetContenido() == "!")
                Match(Tipos.opLogico);
            Expresion();
            string Operador = GetContenido();
            Match(Tipos.opRelacional);
            Expresion();
            float Resultado2 = SE.Pop();
            float Resultado1 = SE.Pop();
            switch (Operador)
            {
                case "==":
                    return Resultado1 == Resultado2; //El return tiene un break dentro por lo tanto no es necesario en el código
                case ">":
                    return Resultado1 > Resultado2;
                case ">=":
                    return Resultado1 >= Resultado2;
                case "<":
                    return Resultado1 < Resultado2;
                case "<=":
                    return Resultado1 <= Resultado2;
                default:
                    return Resultado1 != Resultado2;
            }
        }

        //For -> for(identificador=numero; Condicion; identificador incTermino) BloqueInstrucciones | Instruccion 
        //For -> for(identificador=Expresion; Condicion; identificador incTermino) BloqueInstrucciones | Instruccion
        private void For(bool Ejecuta)
        {
            Match("for");
            Match("(");
            if (!Existe(GetContenido())) //Busca en Match(Tipos.identificador) y agregar excepcion
                throw new Error("Error de Sintaxis: Variable "+ GetContenido() +" no declarada ", Linea, log);
            string Variable1 = GetContenido();
            Match(Tipos.identificador);
            Match("=");
            Expresion();
            float Resultado = SE.Pop();
            Modifica(Variable1, Resultado);
            Match(Tipos.finSentencia);
            bool Evalua = Condicion();
            Match(Tipos.finSentencia);
            if (!Existe(GetContenido())) //Busca en Match(Tipos.identificador) y agregar excepcion
                throw new Error("Error de Sintaxis: Variable "+ GetContenido() +" no declarada ", Linea, log);
            string Variable2 = GetContenido();
            Match(Tipos.identificador);
            string Operador = GetContenido();
            Match(Tipos.incTermino);

            if (Operador == "++")
                Modifica(Variable2, GetValor(Variable2) + 1);
            else if (Operador == "--")
                Modifica(Variable2, GetValor(Variable2) - 1);

            Match(")");
            if (GetContenido() == "{")
            {
                BloqueInstrucciones(Evalua);
            }
            else
                Instruccion(Evalua);
        }

        //While -> while(condicion) BloqueInstrucciones | Instruccion
        private void While(bool Ejecuta)
        {
            Match("while");
            Match("(");
            bool Evalua = Condicion();
            Match(")");
            if (GetContenido() == "{")
            {
                BloqueInstrucciones(Evalua);
            }
            else
                Instruccion(Evalua);
        }

        //DoWhile -> Do BloqueInstrucciones | Instruccion while(condicion)
        private void DoWhile(bool Ejecuta)
        {
            Match("do");
            if (GetContenido() == "{")
            {
                BloqueInstrucciones(true);
            }
            else
                Instruccion(true);
            Match("while");
            Match("(");
            bool Evalua = Condicion();
            Match(")");
            Match(";");
        }

        //Switch -> switch(Expresion) {ListaCases (Default)?} 
        private void Switch(bool Ejecuta)
        {
            Match("switch");
            Match("(");
            Expresion();
            Match(")");
            Match("{");
            ListaCases();
            if (GetContenido() == "default")
                Default();
            Match("}");
        }

        //ListaCases -> Case ListaCases?
        private void ListaCases()
        {
            Case();
            if (GetContenido() == "case")
                ListaCases();
        }

        //Case -> case numero: Case | Instruccion | BloqueInstrucciones (break;)?
        private void Case()
        {
            Match("case");
            Match(Tipos.numero);
            Match(":");
            bool Evalua = Condicion();
            if (GetContenido() == "case")
            {
                Case();
            }
            else if (GetContenido() == "{")
                BloqueInstrucciones(Evalua);
            else
                Instruccion(Evalua);
            if (GetContenido() == "break")
            {
                Match("break");
                Match(";");
            }

        }

        //Default -> default: Instruccion | BloqueInstrucciones (break;)?
        private void Default()
        {
            Match("default");
            Match(":");
            if (GetContenido() == "{")
                BloqueInstrucciones(true);
            else
                Instruccion(true);
            if (GetContenido() == "break")
            {
                Match("break");
                Match(";");
            }
        }

        //Asignacion -> identificador = Expresion;
        private void Asignacion(bool Ejecuta)
        {
            string Variable = GetContenido();
            if (!Existe(GetContenido())) //Busca en Match(Tipos.identificador) y agregar excepcion
                throw new Error("Error de Sintaxis: Variable "+ GetContenido() +" no declarada ", Linea, log);
            Match(Tipos.identificador);
            Match(Tipos.asignacion);
            Expresion();
            float Resultado = SE.Pop(); //Resultado
            Modifica(Variable, Resultado);
            Match(Tipos.finSentencia);
        }

        //Expresion -> Termino MasTermino
        private void Expresion()
        {
            Termino();
            MasTermino();
        }

        //Termino -> Factor PorFactor
        private void Termino()
        {
            Factor();
            PorFactor();
        }

        //MasTermino -> (opTermino Termino)?
        private void MasTermino()
        {
            if (GetClasificacion() == Tipos.opTermino)
            {
                string Operador = GetContenido();
                //Console.Write(GetContenido() + " ");
                Match(Tipos.opTermino);
                Termino();//Segundo Termino
                //Console.Write(Operador + " ");
                float N1 = SE.Pop(); //2 Numeros del Stack
                float N2 = SE.Pop();
                switch (Operador)
                {
                    case "-":
                        SE.Push(N2 - N1);
                        break;
                    case "+":
                        SE.Push(N2 + N1);
                        break;
                }
            }
        }

        //PorFactor -> (opFactor Factor)?
        private void PorFactor()
        {
            if (GetClasificacion() == Tipos.opFactor)
            {
                string Operador = GetContenido();
                Match(Tipos.opFactor);
                Factor();//Segundo Factor
                //Console.Write(Operador + " ");
                float N1 = SE.Pop(); //2 Numeros del Stack
                float N2 = SE.Pop();
                switch (Operador)
                {
                    case "*":
                        SE.Push(N2 * N1);
                        break;
                    case "/":
                        SE.Push(N2 / N1);
                        break;
                }
            }
        }

        //Factor -> numero | identificador | (Expresion)
        private void Factor()
        {
            if (GetClasificacion() == Tipos.numero)
            {
                //Console.Write(GetContenido() + " ");
                SE.Push(float.Parse(GetContenido()));
                Match(Tipos.numero);
            }
            else if (GetClasificacion() == Tipos.identificador)
            {
                //Console.Write(GetContenido() + " ");
                if (!Existe(GetContenido())) //Busca en Match(Tipos.identificador) y agregar excepcion
                    throw new Error("Error de Sintaxis: Variable "+ GetContenido() +" no declarada ", Linea, log);
                SE.Push(GetValor((GetContenido())));
                Match(Tipos.identificador);
            }
            else
            {
                Match("(");
                Expresion();
                Match(")");
            }
        }
    }
}