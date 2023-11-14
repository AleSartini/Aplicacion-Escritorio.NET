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
using Dominio;
using System.Runtime.InteropServices;

namespace Presentacion
{
    public partial class Catalogo : Form
    {
        private List<Articulo> listaCatalogo;
        private Articulo articulo;       
        public Catalogo()
        {
            InitializeComponent();
        }
        private void Catalogo_Load(object sender, EventArgs e)
        {
            try
            {
                cargar();
                btnFiltrar.Enabled = false;  
                ocultarColumnas();
                cargarImagen(listaCatalogo[0].Imagen);
                dgvCatalogo.Columns["Precio"].DefaultCellStyle.Format = "0.00";

                cboCampo.Items.Add("Precio");
                cboCampo.Items.Add("Descripcion");
                cboCampo.Items.Add("Nombre");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void dgvCatalogo_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvCatalogo.CurrentRow != null)
                {
                    Articulo seleccionado = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;
                    cargarImagen(seleccionado.Imagen);
                }

            }
            catch (Exception ex) { throw ex; }
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            btnAgregarImg agregar = new btnAgregarImg();
            agregar.ShowDialog();

            cargar();
        }
        private void btnModificar_Click(object sender, EventArgs e)
        {            
            articulo = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;

            btnAgregarImg modificar = new btnAgregarImg(articulo);
            modificar.ShowDialog();
            
            cargar();
        }
        private void btnDetalle_Click(object sender, EventArgs e)
        {
            articulo = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;

            VerDetalle detalle = new VerDetalle(articulo);
            detalle.ShowDialog();
            
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                articulo = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;
                DialogResult respuesta = MessageBox.Show("¿Desea eliminar el articulo seleccionado?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    negocio.eliminar(articulo.Id);
                }
                cargar();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void txtFiltroRapido_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtFiltroRapido.Text;
            List<Articulo> listaFiltrada;
            try
            {
                if (filtro.Length > 2)
                {
                    listaFiltrada = listaCatalogo.FindAll(x => x.Categoria.Descripcion.ToLower().Contains(filtro.ToLower()) || x.Marca.Descripcion.ToLower().Contains(filtro.ToLower()));
                }
                else
                {
                    listaFiltrada = listaCatalogo;
                }
                dgvCatalogo.DataSource = null;
                dgvCatalogo.DataSource = listaFiltrada;
                dgvCatalogo.Columns["Precio"].DefaultCellStyle.Format = "0.00";
                ocultarColumnas();
            }
            catch (Exception ex) { throw ex; }

        }
        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                txtFiltro.Clear();
                string opcion = cboCampo.Text;

                if (opcion == "Precio")
                {
                    cboCriterio.Items.Clear();
                    cboCriterio.Items.Add("Mayor a");
                    cboCriterio.Items.Add("Menor a");
                    cboCriterio.Items.Add("Igual a");
                }
                else
                {
                    cboCriterio.Items.Clear();
                    cboCriterio.Items.Add("Comienza con");
                    cboCriterio.Items.Add("Termina con");
                    cboCriterio.Items.Add("Contiene");
                }
              

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void cboCriterio_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFiltro.Clear();
        }
        private void txtFiltro_KeyPress(object sender, KeyPressEventArgs e)
        {           
                try
                {                   
                    if (cboCampo.Text == "Precio")
                    {
                        soloNumeros(e);
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            validarBoton();
            if (txtFiltro.Text == "")
            {
                cargar();
            }
        }
        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
       
            try
            {
                string campo = cboCampo.Text;
                string criterio = cboCriterio.Text;
                string filtro = txtFiltro.Text;
                if (filtro != "")
                {
                    dgvCatalogo.DataSource = negocio.filtroAvanzado(campo, criterio, filtro);
                }
                else
                {
                    cargar();
                }
                

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }        
        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listaCatalogo = negocio.listar();
                dgvCatalogo.DataSource = listaCatalogo;
            }
            catch (Exception)
            {

                throw;
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
        private void ocultarColumnas()
        {
            try
            {
                dgvCatalogo.Columns["Id"].Visible = false;
                dgvCatalogo.Columns["Imagen"].Visible = false;
                dgvCatalogo.Columns["Codigo"].Visible = false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void validarBoton()
        {
            bool boton = !String.IsNullOrEmpty(cboCampo.Text) && !String.IsNullOrEmpty(cboCriterio.Text)
                && !String.IsNullOrEmpty(txtFiltro.Text);
            btnFiltrar.Enabled = boton;
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
