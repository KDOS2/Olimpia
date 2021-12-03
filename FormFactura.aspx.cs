namespace Aplicacion_Factura
{
    using Aplicacion_Factura.Classes;
    using System;
    using System.Web.UI.WebControls;
    using System.Linq;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Net.Http;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public partial class FormFactura : System.Web.UI.Page
    {
        #region "Metodos Protegidos"

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if (Page.Request.Params["__EVENTTARGET"] != null && Page.Request.Params["__EVENTTARGET"].Equals("Editar"))
                {
                    int id = Convert.ToInt32(Page.Request.Params["__EVENTARGUMENT"]);
                    this.Editar(id);
                }

            }
            else
            {
                this.TxtIva.Attributes.Add("onKeyPress", "return validaBtnEnviar_Click" +
                    "rNumeros(event)");
                this.TxtValor.Attributes.Add("onKeyPress", "return validarNumeros(event)");
                this.CargarDatos();
            }
        }

        /// <summary>
        /// se ejecuta cuando se esta construyendo la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GgView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnEditar = (Button)e.Row.FindControl("btnEditar");
                btnEditar.OnClientClick = "javascript:return Editar('" + ((Entities)e.Row.DataItem).id + "');";
            }
        }

        /// <summary>
        /// se ejecuta cuando se actualiza un registro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            Entities registro = ((List<Entities>)Session["Datos"]).Where(a => a.id.Equals(Convert.ToInt32(this.hdId.Value))).FirstOrDefault();
            ((List<Entities>)Session["Datos"]).Remove(registro);
            registro = new Entities();
            registro.id = Convert.ToInt32(this.hdId.Value);
            registro.NFactura = this.TxtNFactura.Text;
            registro.Nit = this.TxtNit.Text;
            registro.iva = Convert.ToDouble(this.TxtIva.Text);
            registro.descripcion = this.TxtDescripcion.Text;
            registro.valor = Convert.ToDouble(this.TxtValor.Text);
            ((List<Entities>)Session["Datos"]).Add(registro);
            this.GgView.DataSource = ((List<Entities>)Session["Datos"]).OrderBy(a => a.id);
            this.GgView.DataBind();
            this.Limpiar();
        }

        /// <summary>
        /// se ejecuta cuando se da clic al boton nuevo 
        /// realiza elregistro de una nueva factura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            int id = ((List<Entities>)Session["Datos"]).OrderByDescending(a => a.id).Select(a => a.id).First();
            Entities registro = new Entities();
            registro.id = id + 1;
            registro.NFactura = this.TxtNFactura.Text;
            registro.Nit = this.TxtNit.Text;
            registro.iva = Convert.ToDouble(this.TxtIva.Text);
            registro.descripcion = this.TxtDescripcion.Text;
            registro.valor = Convert.ToDouble(this.TxtValor.Text);
            ((List<Entities>)Session["Datos"]).Add(registro);
            this.GgView.DataSource = ((List<Entities>)Session["Datos"]).OrderBy(a => a.id);
            this.GgView.DataBind();
            this.Limpiar();
        }

        /// <summary>
        /// se ejecuta cuando se da clic al boton enviar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            bool seleccionados = false;
            string valor = string.Empty;

            foreach (GridViewRow row in this.GgView.Rows)
            {

                CheckBox check = (CheckBox)row.FindControl("chkbSelect");

                if (check.Checked)
                {
                    seleccionados = true;
                    valor = string.IsNullOrEmpty(valor) ? this.GgView.DataKeys[row.RowIndex].Value.ToString() : valor + "," + this.GgView.DataKeys[row.RowIndex].Value.ToString();
                }
            }

            if (!seleccionados)
                ScriptManager.RegisterStartupScript(this, typeof(Page), "mensaje", "<script>alert('Seleccione al menos una factura')</script>", false);
            else
            {
                List<Entities> lstEvaluar = new List<Entities>();
                foreach (Entities registro in ((List<Entities>)Session["Datos"]))
                {
                    foreach (string item in valor.Split(','))
                    {
                        if (registro.id.Equals(Convert.ToInt32(item)))
                            lstEvaluar.Add(registro);
                    }
                }

                this.EnviarEvaluar(lstEvaluar);
            }
        }

        #endregion

        #region "Metodos privados"

        /// <summary>
        /// se ejecuta cuando se va cargar la grilla
        /// </summary>
        private void CargarDatos()
        {
            DTO origen = new DTO();
            this.GgView.DataSource = origen.CargarData().OrderBy(a => a.id);
            this.GgView.DataBind();
            Session["Datos"] = origen.CargarData();
        }

        /// <summary>
        /// carga a informacion a modifcar
        /// </summary>
        /// <param name="id"></param>
        private void Editar(int id)
        {
            Entities registro = registro = ((List<Entities>)Session["Datos"]).Where(a => a.id.Equals(id)).FirstOrDefault();
            this.hdId.Value = registro.id.ToString();
            this.TxtNFactura.Text = registro.NFactura;
            this.TxtNit.Text = registro.Nit;
            this.TxtValor.Text = registro.valor.ToString();
            this.TxtIva.Text = registro.iva.ToString();
            this.TxtDescripcion.Text = registro.descripcion;
        }

        /// <summary>
        /// limpia los objetos de la interfaz
        /// </summary>
        private void Limpiar()
        {
            this.hdId.Value = string.Empty;
            this.TxtNFactura.Text = string.Empty;
            this.TxtNit.Text = string.Empty;
            this.TxtValor.Text = string.Empty;
            this.TxtIva.Text = string.Empty;
            this.TxtDescripcion.Text = string.Empty;
            this.lbError.Text = string.Empty;
        }

        /// <summary>
        /// invoca el servicio que valida a las facturas
        /// </summary>
        /// <param name="lstEvaluar"></param>
        private void EnviarEvaluar(List<Entities> lstEvaluar)
        {

            string json = string.Empty;
            object rta = 0;

            using (var client = new HttpClient())
            {
                // Parametros de usuario al servicio de autenticación                
                string sUrlApi2 = ConfigurationManager.AppSettings["URL_API_VALIDA_FACT"].ToString();
                client.BaseAddress = new Uri(sUrlApi2);
                var postTask = client.PostAsJsonAsync<List<Entities>>(sUrlApi2, lstEvaluar);
                postTask.Wait();

                var result = postTask.Result;
                var readTask = result.Content.ReadAsStringAsync();
                readTask.Wait();
                json = readTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    rta = JsonConvert.DeserializeObject<double>(json);
                    this.lbError.Text = rta.ToString();
                }
                else
                {
                    rta = json.ToString();
                    this.MensajeError(rta);
                }
            }
        }

        /// <summary>
        /// muestra el mensja cuando la validacion de las facturas no es exitoso
        /// </summary>
        /// <param name="mensaje"></param>
        private void MensajeError(object mensaje)
        {
            string[] msj = mensaje.ToString().Split('*');
            this.lbError.Text = msj.Count().Equals(1) ? msj[0].ToString() : msj[1].ToString();
        }

        #endregion
            
    }
}