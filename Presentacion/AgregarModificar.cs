using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;

namespace Presentacion
{
    public partial class btnAgregarImg : Form
    {
        private OpenFileDialog archivo = null;
        private Articulo articulo = null;
        public btnAgregarImg()
        {
            InitializeComponent();
        }
        public btnAgregarImg(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Articulo";
        }
        private void AgregarModificar_Load(object sender, EventArgs e)
        {
            btnAceptar.Enabled = false;

            CategoriaNegocio negocioCat = new CategoriaNegocio();
            cboCategoria.DataSource = negocioCat.listar();

            MarcaNegocio negocioMarca = new MarcaNegocio();
            cboMarca.DataSource = negocioMarca.listar();

            if (articulo != null)
            {
                txtCodigo.Text = articulo.Codigo;
                txtNombre.Text = articulo.Nombre;
                txtDescripcion.Text = articulo.Descripcion;
                txtImagen.Text = articulo.Imagen;
                cargarImagen(txtImagen.Text);
                txtPrecio.Text = articulo.Precio.ToString("0.00");
                cboCategoria.Text = articulo.Categoria.ToString();
                cboMarca.Text = articulo.Marca.ToString();
            }


        }
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                if (articulo == null)
                {
                    articulo = new Articulo();
                }
                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Imagen = txtImagen.Text;
                articulo.Precio = decimal.Parse(txtPrecio.Text);
                articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
                articulo.Marca = (Marca)cboMarca.SelectedItem;



                if (articulo.Id != 0)
                {
                    negocio.modificar(articulo);
                    MessageBox.Show("Modificado Exitosamente");
                }
                else
                {
                    negocio.agregar(articulo);
                    MessageBox.Show("Agregado exitosamente");
                }
            
                if (archivo != null && !(txtImagen.Text.ToLower().Contains("http")))
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["carpeta-imagen"] + archivo.SafeFileName);               
                }
                
                Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {           
            try
            {
                soloNumeros(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void txtPrecio_TextChanged(object sender, EventArgs e)
        {
            validarBoton();
        }
        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            validarBoton();
        }
        private void txtDescripcion_TextChanged(object sender, EventArgs e)
        {
            validarBoton();
        }
        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            validarBoton();
        }
        private void txtImagen_TextChanged(object sender, EventArgs e)
        {
            cargarImagen(txtImagen.Text);
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
        private void validarBoton()
        {
            var boton = !String.IsNullOrEmpty(txtCodigo.Text) && !String.IsNullOrEmpty(txtDescripcion.Text)
                && !String.IsNullOrEmpty(txtNombre.Text) && !String.IsNullOrEmpty(txtPrecio.Text.ToString());
            btnAceptar.Enabled = boton;
        }

        private void button1_Click(object sender, EventArgs e)
        {                    
            archivo = new OpenFileDialog();
           
            archivo.Filter = "jpg|*.jpg;|png|*.png";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtImagen.Text = archivo.FileName;
                cargarImagen(archivo.FileName);               
            }
            
        }
        public void soloNumeros(KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }   
}
