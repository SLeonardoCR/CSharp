namespace SINTAXIS_1
{

    /*
    Requerimientos: 
            1. Se requiere indicar en que linea está el error Lexico o sintactico
            2. Modificar la gramática para considerar la negacion. Ejemplo: 
               if(!a<0) osea (!)?
            3. Modificar Condicion y For
            4. Agregar las produccion Switch
                Switch(Expresion)
                    Varios Case: Instruccion | Bloque de Instrucciones 
                    break?;
                    (Default: Intruccion | BloqueInstrucciones)?
            5. Codificar el método Switch
    */
    public class Lenguaje : Sintaxis
    {
        //Programa -> Librerias Variables Main
        public void Programa()
        {
            Librerias();
            Variables();
            Main();
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

        //Variables -> tipoDato ListaIdentificadores; Variables?
        private void Variables()
        {
            Match(Tipos.tipoDato);
            ListaIdentificadores();
            Match(Tipos.finSentencia);
            if (GetClasificacion() == Tipos.tipoDato)
                Variables();
        }

        //ListaIdentificadores -> identificador(,ListaIdentificadores)?
        private void ListaIdentificadores()
        {
            Match(Tipos.identificador);
            if (GetContenido() == ",")
            {
                Match(",");
                ListaIdentificadores();
            }
        }

        //Main	->	void main() BloqueInstrucciones
        private void Main()
        {
            Match("void");
            Match("main");
            Match("(");
            Match(")");
            BloqueInstrucciones();
        }

        //BloqueInstrucciones  -> {ListaInstrucciones}
        private void BloqueInstrucciones()
        {

            Match("{");
            ListaInstrucciones();
            Match("}");
        }

        //ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones()
        {
            Instruccion();
            if (GetContenido() != "}")
                ListaInstrucciones();
        }

        //Instruccion -> Printf | Scanf | If | For | While 
        private void Instruccion()
        {
            if (GetContenido() == "printf")
                Printf();
            else if (GetContenido() == "scanf")
                Scanf();
            else if (GetContenido() == "if")
                If();
            else if (GetContenido() == "for")
                For();
            else if (GetContenido() == "while")
                While();
            else if (GetContenido() == "switch")
                Switch();
            else
                Asignacion();
        }

        //Printf -> printf(Cadena (,ListaIdentificadores?));
        private void Printf()
        {
            Match("printf");
            Match("(");
            Match(Tipos.Cadena);
            if (GetContenido() == ",")
            {
                Match(",");
                ListaIdentificadores();
            }
            Match(")");
            Match(Tipos.finSentencia);
        }

        //Scanf -> scanf(Cadena, ListaDeAmpersas);
        private void Scanf()
        {
            Match("scanf");
            Match("(");
            Match(Tipos.Cadena);
            Match(",");
            ListaDeAmpersas();
            Match(")");
            Match(Tipos.finSentencia);
        }

        //ListaDeAmpersas -> &identificador (,ListaDeAmpersas)? 
        private void ListaDeAmpersas()
        {
            Match("&");
            Match(Tipos.identificador);
            if (GetContenido() == ",")
            {
                Match(",");
                ListaDeAmpersas();
            }
        }

        //If -> if(Condicion) BloqueInstrucciones | Instruccion (else BloqueInstrucciones | Instruccion)?
        private void If()
        {
            Match("if");
            Match("(");
            Condicion();
            Match(")");
            if (GetContenido() == "{")
            {
                BloqueInstrucciones();
            }
            else
                Instruccion();
            if (GetContenido() == "else")
            {
                Match("else");
                if (GetContenido() == "{")
                {
                    BloqueInstrucciones();
                }
                else
                    Instruccion();
            }
        }

        //Condicion	-> numero | identificador opRelacional numero | identificador
        //Condicion	-> Expresion opLogico Expresion
        //Condicion	-> (!)? Expresion opLogico Expresion
        private void Condicion()
        {
            if (GetContenido() == "!")
                Match(Tipos.opLogico);
            Expresion();
            Match(Tipos.opRelacional);
            Expresion();
        }

        //For -> for(identificador=numero; Condicion; identificador incTermino) BloqueInstrucciones | Instruccion 
        //For -> for(identificador=Expresion; Condicion; identificador incTermino) BloqueInstrucciones | Instruccion
        private void For()
        {
            Match("for");
            Match("(");
            Match(Tipos.identificador);
            Match("=");
            Expresion();
            Match(Tipos.finSentencia);
            Condicion();
            Match(Tipos.finSentencia);
            Match(Tipos.identificador);
            Match(Tipos.incTermino);
            Match(")");
            if (GetContenido() == "{")
            {
                BloqueInstrucciones();
            }
            else
                Instruccion();
        }

        //While -> while(condicion) BloqueInstrucciones | Instruccion
        private void While()
        {
            Match("while");
            Match("(");
            Condicion();
            Match(")");
            if (GetContenido() == "{")
            {
                BloqueInstrucciones();
            }
            else
                Instruccion();
        }

        //Switch -> switch(Expresion) {ListaCases (Default)?} 
        private void Switch()
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
            if (GetContenido() == "case")
            {
                Case();
            }
            else if (GetContenido() == "{")
                BloqueInstrucciones();
            else
                Instruccion();
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
                BloqueInstrucciones();
            else
                Instruccion();
            if (GetContenido() == "break")
            {
                Match("break");
                Match(";");
            }
        }

        //Asignacion -> identificador = Expresion;
        private void Asignacion()
        {
            Match(Tipos.identificador);
            Match(Tipos.asignacion);
            Expresion();
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
                Match(Tipos.opTermino);
                Termino();
            }
        }

        //PorFactor -> (opFactor Factor)?
        private void PorFactor()
        {
            if (GetClasificacion() == Tipos.opFactor)
            {
                Match(Tipos.opFactor);
                Factor();
            }
        }

        //Factor -> numero | identificador | (Expresion)
        private void Factor()
        {
            if (GetClasificacion() == Tipos.numero)
                Match(Tipos.numero);
            else if (GetClasificacion() == Tipos.identificador)
                Match(Tipos.identificador);
            else
            {
                Match("(");
                Expresion();
                Match(")");
            }
        }
    }
}