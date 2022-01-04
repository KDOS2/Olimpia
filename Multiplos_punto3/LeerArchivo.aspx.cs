namespace Multiplos_punto3
{
    using Multiplos_punto3.Transversal;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;

    public partial class LeerArchivo : System.Web.UI.Page
    {

        #region "Variables pribadas"

        private string strCarpeta = string.Empty;
        private string archivResultado = string.Empty;
        private string strMensajeInicial = string.Empty;
        private string strEsMultiplo = string.Empty;
        private string strNoEsMultiplo = string.Empty;
        private string strNoEsNumero = string.Empty;
        private string strExtension = string.Empty;

        #endregion

        #region "Metodos protegidos"

        protected void Page_Load(object sender, EventArgs e)
        {
            this.strCarpeta = Server.MapPath("./") + ConfigurationManager.AppSettings["Carpeta_Archivos"].ToString();
            this.archivResultado = ConfigurationManager.AppSettings["NombreResultado"].ToString();
            this.strMensajeInicial = ConfigurationManager.AppSettings["MensajeInicial"].ToString();
            this.strEsMultiplo = ConfigurationManager.AppSettings["esMultiplo"].ToString();
            this.strNoEsMultiplo = ConfigurationManager.AppSettings["noEsMultiplo"].ToString();
            this.strNoEsNumero = ConfigurationManager.AppSettings["noEsNumero"].ToString();
            this.strExtension = ConfigurationManager.AppSettings["Extension"].ToString();
        }

        /// <summary>
        /// se ejecuta cuando se da clic al boton procesar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnProcesar_Click(object sender, EventArgs e)
        {
            if (this.ValidarExtension().Equals(this.strExtension))
            {
                List<ValidarEntity> lstValidador = this.LeerTxt(this.FileLoad.FileName);
                this.EliminarArchivo(this.FileLoad.FileName);
                this.EliminarArchivo(this.archivResultado);
                this.GenerarArchivo(lstValidador);
                this.DescargarArchivo();
            }
            else
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Mensaje", "<script> alert('El archivo debe tener una extension txt.')</script>");
        }

        #endregion

        #region "Metodos Privados"

        /// <summary>
        /// Lee el archivo cargado
        /// </summary>
        /// <param name="nombre">Nombre del archivo a procesar</param>
        /// <returns>Lista con las validaciones a ser mostradas en el archivo de resutlado</returns>
        /// 
        private List<ValidarEntity> LeerTxt(string nombre)
        {
            string strFileName = nombre;
            List<ValidarEntity> lstValidar = new List<ValidarEntity>();

            this.FileLoad.PostedFile.SaveAs(this.strCarpeta + strFileName);
            
            using (StreamReader ReaderObject = new StreamReader(this.strCarpeta + strFileName))
            {
                string Linea;
                int lineas = 1;
                
                while ((Linea = ReaderObject.ReadLine()) != null)
                {
                    ValidarEntity lineaEvaluada = new ValidarEntity();
                    lineaEvaluada.linea = lineas;
                    lineaEvaluada.esnumero = this.ValidarEstructuraArchvo(Linea);
                    lineaEvaluada.numero = lineaEvaluada.esnumero ? Convert.ToDouble(Linea) : 0;
                    lineaEvaluada.multiplo3 = lineaEvaluada.esnumero ? (this.SumaDigitos(Linea) % 3).Equals(0) : false;
                    lineas++;
                    lstValidar.Add(lineaEvaluada);
                }                
            }

            return lstValidar;
        }

        /// <summary>
        /// Metodo que realiza la sumatoria de los digitos del numero 
        /// a ser evaluado
        /// </summary>
        /// <param name="cadena">numero a evaluar</param>
        /// <returns>sumatoria de los digitos a evaluar</returns>
        private double SumaDigitos(string cadena)
        {
            char[] arregloCadena;
            arregloCadena = cadena.ToCharArray();
            double suma = 0;

            foreach (char caracter in arregloCadena)
                suma += Convert.ToDouble(caracter.ToString());

            return suma;
        }

        /// <summary>
        /// Genera el archivo de resultado
        /// </summary>
        /// <param name="lstValidar"></param>
        private void GenerarArchivo(List<ValidarEntity> lstValidar)
        {
            string rutaCompleta = this.strCarpeta + this.archivResultado;
            string esMultiplo = string.Empty;
            using (StreamWriter filas = File.AppendText(rutaCompleta))
            {
                foreach (ValidarEntity registro in lstValidar.OrderBy(a => a.linea).ToList())
                {
                    if (registro.esnumero)
                    {
                        esMultiplo = registro.multiplo3.Equals(true) ? this.strEsMultiplo : this.strNoEsMultiplo;
                        filas.WriteLine(this.strMensajeInicial + registro.linea.ToString() + " " + this.strEsMultiplo);
                    }
                    else
                        filas.WriteLine(this.strMensajeInicial + registro.linea.ToString() + this.strNoEsNumero);
                }

                filas.Close();
            }
        }

        /// <summary>
        /// Elimina el archivo seleccionado
        /// </summary>
        /// <param name="nombreArchivo"></param>
        private void EliminarArchivo(string nombreArchivo)
        {
            if(File.Exists(this.strCarpeta + nombreArchivo))
                File.Delete(this.strCarpeta + nombreArchivo);
        }

        /// <summary>
        /// descarga el archivo de resultados
        /// con la informacion evaluada
        /// </summary>
        /// 
        private void DescargarArchivo()
        {
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition", "attachment; filename=" + this.archivResultado + ";");
            response.TransmitFile(this.strCarpeta + this.archivResultado);
            response.Flush();
            response.End();
        }

        /// <summary>
        /// valida si todos los registros son numeros o no
        /// </summary>
        /// <param name="dato">registro</param>
        /// <returns>true o false</returns>
        private bool ValidarEstructuraArchvo(string dato)
        {
            return dato.All(char.IsDigit);
        }

        /// <summary>
        /// estrae la extension del archivo
        /// </summary>
        /// <param name="nombre">nombre del archivo</param>
        /// <returns>extension</returns>
        private string ValidarExtension()
        {
            return Path.GetExtension(this.FileLoad.FileName);
        }

        #endregion
    
    }
}