namespace Lexico_3
{
    public class Token
    {
        string Contenido;
        public enum Tipos{
            identificador, numero, caracter, asignacion, finSentencia, opLogico, opRelacional,
            opTermino, opFactor, incTermino, incFactor, Cadena, inicializacion, tipoDato,
            zona, ciclo, condicion, opTernario, opFlujoEntrada, opFlujoSalida,

        }

        Tipos Clasificacion;

        public void SetContenido(string contenido){
            this.Contenido=contenido;
        }

        public void SetClasificacion(Tipos clasificacion){
            this.Clasificacion = clasificacion;
        }

        public string GetContenido(){
            return Contenido;
        }

        public Tipos GetClasificacion(){
            return Clasificacion;
        }
    }
}