using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class MarcaNegocio
    {
        public List<Marca> listar()
        {
            AccesoDatos data = new AccesoDatos();
            try
            {
                List<Marca> lista = new List<Marca>();

                data.setearConsulta("select Id, Descripcion Marca from MARCAS");
                data.ejecutarLectura();
                while (data.Lector.Read())
                {
                    Marca aux = new Marca();
                    aux.Id = (int)data.Lector["Id"];
                    aux.Descripcion = (string)data.Lector["Marca"];

                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { data.cerrarConexion(); }

        }
    }
}
