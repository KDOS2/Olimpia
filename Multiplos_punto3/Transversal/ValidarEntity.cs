namespace Multiplos_punto3.Transversal
{
    /// <summary>
    /// clase que contiene la evaluacion de la linea
    /// </summary>
    public class ValidarEntity
    {
        /// <summary>
        /// setea la linea evaluada
        /// </summary>
        public int linea { get; set; }

        /// <summary>
        /// numero evaluado
        /// </summary>
        public double numero { get; set; }

        /// <summary>
        /// sies multiplo de tres = 1 sino = 0
        /// </summary>
        public bool multiplo3 { get; set; }

        /// <summary>
        /// identifica si es numero o no
        /// </summary>
        public bool esnumero { get; set; }
    }
}