using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> listar()
        {
            AccesoDatos data = new AccesoDatos();
            List <Articulo> lista = new List<Articulo>();

            try
            {
                data.setearConsulta("select A.Id, Codigo, Nombre, A.Descripcion,C.Descripcion Categoria, M.Descripcion Marca, A.IdMarca, A.IdCategoria, ImagenUrl, Precio from ARTICULOS A, CATEGORIAS C, MARCAS M Where C.Id = A.IdCategoria and M.Id = A.IdMarca");

                data.ejecutarLectura();
                while (data.Lector.Read()) 
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)data.Lector["Id"];
                    aux.Codigo = (string)data.Lector["Codigo"];
                    aux.Nombre = (string)data.Lector["Nombre"];
                    aux.Descripcion = (string)data.Lector["Descripcion"];
                    
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)data.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)data.Lector["Categoria"];
                    
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)data.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)data.Lector["Marca"];
                    if (!(data.Lector["ImagenUrl"] is DBNull))
                    { 
                        aux.Imagen = (string)data.Lector["ImagenUrl"];
                    }
                    aux.Precio = (decimal)data.Lector["Precio"];
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
        public void agregar(Articulo nuevo)
        {
            AccesoDatos data = new AccesoDatos();
            try
            {
                data.setearConsulta("insert into ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio) values(@codigo, @nombre, @descripcion, @idMarca, @idCat, @imagen, @precio)");               
                data.setearParametro("@codigo", nuevo.Codigo);
                data.setearParametro("@nombre", nuevo.Nombre);
                data.setearParametro("@descripcion",nuevo.Descripcion);
                data.setearParametro("@idMarca",nuevo.Marca.Id);
                data.setearParametro("idCat",nuevo.Categoria.Id);
                data.setearParametro("@imagen",nuevo.Imagen);
                data.setearParametro("@precio",nuevo.Precio);
                
                data.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { data.cerrarConexion(); }
        }
        public void modificar(Articulo modificar) 
        {
            AccesoDatos data = new AccesoDatos();

            try
            {
                data.setearConsulta("Update Articulos set Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, IdMarca = @marca, IdCategoria = @categoria, ImagenUrl = @img, Precio = @precio where Id = @id");
                data.setearParametro("@id", modificar.Id);
                data.setearParametro("@codigo", modificar.Codigo);
                data.setearParametro("@nombre", modificar.Nombre);
                data.setearParametro("@descripcion", modificar.Descripcion);
                data.setearParametro("@marca", modificar.Marca.Id);
                data.setearParametro("@categoria", modificar.Categoria.Id);
                data.setearParametro("@img", modificar.Imagen);
                data.setearParametro("@precio", modificar.Precio);

                data.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { data.cerrarConexion();}
        }
        public void eliminar (int eliminar)
        {
            AccesoDatos data = new AccesoDatos ();
            try
            {
                data.setearConsulta("delete from ARTICULOS where Id = @id");
                data.setearParametro("@id", eliminar);
                data.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { data.cerrarConexion();}   
        }
        public List<Articulo> filtroAvanzado (string campo, string criterio, string filtro)
        {           
            AccesoDatos data = new AccesoDatos();
            try
            {
                List<Articulo> lista = new List<Articulo>();
                string consulta = "select A.Id, Codigo, Nombre, A.Descripcion,C.Descripcion Categoria, M.Descripcion Marca, A.IdMarca, A.IdCategoria, ImagenUrl, Precio from ARTICULOS A, CATEGORIAS C, MARCAS M Where C.Id = A.IdCategoria and M.Id = A.IdMarca and  ";

                if (campo == "Precio")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "Precio > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "Precio < " + filtro;
                            break;
                        default:
                            consulta += "Precio = " + filtro;
                            break;
                    }
                }
                else if (campo == "Descripcion")
                    switch (criterio)
                    {
                        case ("Comienza con"):
                            consulta += "A.Descripcion like'" + filtro + "%'";
                            break;
                        case ("Termina con"):
                            consulta += "A.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "A.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                else
                {
                    switch (criterio)
                    {
                        case ("Comienza con"):
                            consulta += "Nombre like'" + filtro + "%'";
                            break;
                        case ("Termina con"):
                            consulta += "Nombre like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;
                    }
                }

                data.setearConsulta(consulta);
                data.ejecutarLectura();

                while (data.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)data.Lector["Id"];
                    aux.Codigo = (string)data.Lector["Codigo"];
                    aux.Nombre = (string)data.Lector["Nombre"];
                    aux.Descripcion = (string)data.Lector["Descripcion"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)data.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)data.Lector["Categoria"];

                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)data.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)data.Lector["Marca"];
                    if (!(data.Lector["ImagenUrl"] is DBNull))
                    {
                        aux.Imagen = (string)data.Lector["ImagenUrl"];
                    }
                    aux.Precio = (decimal)data.Lector["Precio"];

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
