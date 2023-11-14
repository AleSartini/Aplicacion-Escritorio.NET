using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class CategoriaNegocio
    {
        public List<Categoria> listar()
        {
            AccesoDatos data = new AccesoDatos();
            try
            {
                List<Categoria> lista = new List<Categoria>();
                data.setearConsulta("select Id, Descripcion Categoria from CATEGORIAS");
                data.ejecutarLectura();
                while (data.Lector.Read())
                {
                    Categoria aux = new Categoria();
                    aux.Id = (int)data.Lector["Id"];
                    aux.Descripcion = (string)data.Lector["Categoria"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally { data.cerrarConexion(); }
        }
    }
}
