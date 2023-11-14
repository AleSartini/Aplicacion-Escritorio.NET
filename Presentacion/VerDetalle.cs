using Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class VerDetalle : Form
    {
        Articulo articulo;
        public VerDetalle(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
        }

        private void VerDetalle_Load(object sender, EventArgs e)
        {
            try
            {
                txtId.Text = articulo.Id.ToString();
                txtCodigo.Text = articulo.Codigo.ToString();
                txtNombre.Text = articulo.Nombre;
                txtDescripcion.Text = articulo.Descripcion;
                txtCategoria.Text = articulo.Categoria.ToString();
                TxtMarca.Text = articulo.Marca.ToString();
                txtImagen.Text = articulo.Imagen;
                    cargarImagen(txtImagen.Text);
                txtPrecio.Text = articulo.Precio.ToString("0.00");         
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pbxImagen.Load(imagen);
            }
            catch (Exception ex)
            {

                pbxImagen.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }
    }
}
