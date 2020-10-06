using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SistemaPedidos.Resources.Model;
using SQLite;
using Android.Util;
using Android.Database;


namespace SistemaPedidos.Resources.DataHelper
{
    public class ConsultasTablas
    {                         
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        public bool CrearTablasGrales()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.CreateTable<Usuarios>();
                    //connection.CreateTable<Vendedor>();
                    connection.CreateTable<Productos>();
                    connection.CreateTable<PedidosMaster>();
                    connection.CreateTable<PedidosDetalle>();
                    connection.CreateTable<ListasPrecio>();
                    connection.CreateTable<TablaClientes>();
                    connection.CreateTable<CategoriaProductos>();
                    connection.CreateTable<TablaSincronizaciones>();
                    connection.CreateTable<PromocionesDescuentos>();
                    connection.CreateTable<CotizacionMoneda>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                //Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return false;
            }
        }

        //******************************************************************
        //tareas sobre tabla COTIZACION DE MONEDAS
        //******************************************************************

        public List<CotizacionMoneda> VerListaMonedas()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Table<CotizacionMoneda>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }
        public bool InstertarNuevaMoneda(CotizacionMoneda nvaMoneda)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Insert(nvaMoneda);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public List<CotizacionMoneda> VerDetalleMoneda(int idmoneda=2)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Query<CotizacionMoneda>("select cotizacion  from CotizacionMoneda where id=?", idmoneda ).ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
             
                return null;
            }
        }
        public bool ActualizarMoneda(CotizacionMoneda  Moneda)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Query<TablaClientes>("update CotizacionMoneda set cotizacion=? where id=?", Moneda.cotizacion,Moneda.id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }

        //******************************************************************
        //tareas sobre tabla usuarios
        //******************************************************************

        public bool InstertarNuevoUsuario(Usuarios nvoUsuario)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Insert(nvoUsuario);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public List<Usuarios> DatosUsuario(string usuario, string pass)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Query<Usuarios>("SELECT * FROM Usuarios where usuario =? and contraseña =?", usuario, pass).ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }

        //******************************************************************
        //tareas sonbre tabla clientes
        //******************************************************************

        public List<TablaClientes> VerListaCLientes()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Table<TablaClientes>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }
        public List<TablaClientes> VerDetalleCliente(int idcliente)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Query<TablaClientes>("select *  from TablaClientes where idclientes=?",idcliente).ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }
        public List<TablaClientes> VerDetalleClienteMain(int codclieMain)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Query<TablaClientes>("select *  from TablaClientes where codclieMain=?", codclieMain).ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }
        public List<TablaClientes> BuscarCliente(string cliente)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Query<TablaClientes>("select *  from TablaClientes where nomapell_razon like '%"+cliente+"%'").ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }
        public bool InsterarCliente(TablaClientes Cliente)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Insert(Cliente);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public bool EliminarCliente(int IdCliente)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Query<TablaClientes>("delete from TablaClientes where idclientes=?",IdCliente);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public bool ActualizarCliente(TablaClientes Cliente)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Query<TablaClientes>("update TablaClientes set nomapell_razon=?,domicilio=?,localidad=?,iva_tipo=?,cuit=?,telefono=?,contacto=?," +
                        "celular=?,email=?,observaciones=?,lista_precios=?,vendedor=? where codclieMain=?",Cliente.nomapell_razon,Cliente.domicilio,Cliente.localidad,
                        Cliente.iva_tipo,Cliente.cuit,Cliente.telefono,Cliente.contacto,Cliente.celular,Cliente.email,Cliente.observaciones,Cliente.lista_precios,
                        Cliente.vendedor,Cliente.codclieMain);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public bool ActualizarClienteLocal(TablaClientes Cliente)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Query<TablaClientes>("update TablaClientes set nomapell_razon=?,domicilio=?," +
                        "celular=?,email=?,observaciones=? where idclientes=?", Cliente.nomapell_razon, Cliente.domicilio, Cliente.celular, 
                        Cliente.email, Cliente.observaciones,Cliente.idclientes);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public bool VaciarTablaClientes()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Query<TablaClientes>("DELETE FROM TablaClientes");
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }

        //******************************************************************
        //tareas sobre taba productos
        //******************************************************************        

        public bool InsertarProducto(Productos Productos)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Insert(Productos);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public List<Productos> VerListaProductos()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Table<Productos>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }
        public List<Productos> VerListaProductosCategoria(string categoria)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Query<Productos>("select * from Productos where categoria in (select id from CategoriaProductos where nombre Like '" + categoria + "')").ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }
        public List<Productos> VerListaProductosBusqueda(string producto)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Query<Productos>("Select * from Productos where descripcion like '%" + producto +"%'").ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }
        public List<Productos> VerListaProductosBusquedaID(int idproducto)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Query<Productos>("Select * from Productos where id=?",idproducto).ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }
        public List<Productos> VerListaProductosBusquedaCod(int CodProducto)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Query<Productos>("Select * from Productos where codProdMain=?", CodProducto).ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }

        public bool ActualizarProducto(Productos producto)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Query<Usuarios>("UPDATE Productos set precio=?, ganancia=?, utilidad1=?, utilidad2=?,calcular_precio=?,bonif=?,utilidad3=?,utilidad4=? " +
                        "where codProdMain=?", producto.precio,producto.ganancia,producto.utilidad1,producto.utilidad2, producto.calcular_precio,producto.bonif, 
                        producto.utilidad3 , producto.utilidad3,producto.codProdMain);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public bool VaciarTablaProductos()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Query<Productos>("DELETE FROM Productos");
                    connection.Query<Productos>("update sqlite_squence set seq=0 where name=Productos ");
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }

        //******************************************************************
        //tareas sobte tabla Listas de precios y descuentos
        //******************************************************************

        public bool InsertarListaPrecio(ListasPrecio ListasPrecio)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Insert(ListasPrecio);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public bool InsertarPromocionesDescuentos(PromocionesDescuentos  promocionesDescuentos)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Insert(promocionesDescuentos);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public List<ListasPrecio> VerListaPrecio()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Table<ListasPrecio>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }
        public List<PromocionesDescuentos> VerPromocionesDescuentos()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Table<PromocionesDescuentos>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }
        public List<ListasPrecio> VerListaPrecioId(int idLista)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Query<ListasPrecio>("select * from ListasPrecio where id=?",idLista).ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }
        public List<PromocionesDescuentos> VerPromocionesDescuentosId(int idPromDesc)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Query<PromocionesDescuentos>("select * from PromocionesDescuentos where id=?", idPromDesc).ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }
        public bool ActualizarListasPrecios(ListasPrecio ListasPrecio)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Query<Usuarios>("UPDATE ListasPrecio set nombre=?, utilidad=?,auxcol=? " +
                        "where id=?", ListasPrecio.nombre,ListasPrecio.utilidad, ListasPrecio.auxcol, ListasPrecio.id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public bool ActualizarPromocionesDescuentos(PromocionesDescuentos promocionesDescuentos)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Query<Usuarios>("UPDATE PromocionesDescuentos set nombrepromo=?, idproducto=?,idcategoria=?, " +
                        "compra_min=?, descuento_porc=? where id=?", promocionesDescuentos.nombrepromo, promocionesDescuentos.idproducto,promocionesDescuentos.idcategoria,
                        promocionesDescuentos.compra_min,promocionesDescuentos.descuento_porc,promocionesDescuentos.id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public bool VaciarTablaListasPrecio()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Query<ListasPrecio>("DELETE FROM ListasPrecio");
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public bool VaciarTablaPromocionesDescuentos()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Query<ListasPrecio>("DELETE FROM PromocionesDescuentos");
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public List<PromocionesDescuentos> VerPromocionesDescuentosIdProd(int idProd)
        {

            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Query<PromocionesDescuentos>("select * from PromocionesDescuentos where idproducto=?", idProd).ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }
        public List<PromocionesDescuentos> VerPromocionesDescuentosIdCateg(int idCateg)
        {

            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Query<PromocionesDescuentos>("select * from PromocionesDescuentos where idcategoria=?", idCateg).ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }
        public List<PromocionesDescuentos> VerPromocionesDescuentosCant(int idProd, int cant)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Query<PromocionesDescuentos>("select * from PromocionesDescuentos where idproducto=? and cast(compra_min as integer)<=?", idProd, cant).ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }


        //******************************************************************
        //tareas sobre la tabla categoria de productos
        //******************************************************************

        public bool InsertarCateogoriaProd(CategoriaProductos categoriaProductos)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Insert(categoriaProductos);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public List<CategoriaProductos> verCategoriaProductos()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Table<CategoriaProductos>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }
        public bool ActualizarCategoriaProd(CategoriaProductos categoriaProductos)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Query<CategoriaProductos>("UPDATE CategoriaProductos set nombre=? " +
                        "where id=?", categoriaProductos.nombre, categoriaProductos.id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public bool VaciarTablaCategoriasProd()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Query<CategoriaProductos>("DELETE FROM CategoriasProductos");
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }

        //****************************************************************
        //tareas sobre la tabla de pedidos MASTER Y DETALLE
        //****************************************************************
        public bool InsertarPedido(PedidosMaster pedidosMaster)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Insert(pedidosMaster);
                    return true ;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public bool EliminarPedido(PedidosMaster pedidosMaster,PedidosDetalle pedidosDetalle)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Delete(pedidosMaster);
                    connection.Delete(pedidosDetalle);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public bool ActualizaPedido(PedidosMaster pedidosMaster)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Update(pedidosMaster);

                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public bool VaciarTablaPedidosMaster()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Query<PedidosMaster>("DELETE FROM PedidosMaster");
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public List<PedidosMaster> verListaPedidos(int estadoFin)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    if (estadoFin==0 || estadoFin == 1) { 
                        return connection.Query<PedidosMaster>("SELECT * FROM PedidosMaster where finalizado=? and enviado=0 order by id desc", estadoFin).ToList();
                    }else if (estadoFin == 2)
                    {
                        return connection.Query<PedidosMaster>("SELECT * FROM PedidosMaster where finalizado=1 and enviado=1 order by id desc").ToList();
                    }
                    else
                    {
                        return null;
                    }

                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }
        public List<PedidosMaster> verListaPedidosSincro(int estadoSincro)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Query<PedidosMaster>("SELECT * FROM PedidosMaster where enviado=?", estadoSincro).ToList();
                   
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }
        public List<PedidoMax> ObtenerUltimoIdPedido()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Query<PedidoMax>("SELECT max(id) as id FROM PedidosMaster").ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null ;
            }
        }        
        public List<PedidosMaster> VerPedidoMaster(int idpedido)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Query<PedidosMaster>("SELECT * FROM PedidosMaster where id=?", idpedido).ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }      
        public bool InsertarProductoPedido(PedidosDetalle pedidosDetalle)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Insert(pedidosDetalle);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public bool EliminarProductoPedido(PedidosDetalle pedidosDetalle)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Delete(pedidosDetalle);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public bool ModificarProductoPedido(PedidosDetalle pedidosDetalle)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Query<PedidosDetalle>("update PedidosDetalle set cantidad=?,punit=?,ptotal=? where id=?",
                        pedidosDetalle.cantidad,pedidosDetalle.punit,pedidosDetalle.ptotal,pedidosDetalle.id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public List<PedidosDetalle > VerPedidoDetalleID(int idproductoDetalle)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Query<PedidosDetalle >("Select * from PedidosDetalle where id=?", idproductoDetalle).ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }
        public bool VaciarTablaDetallePedidos()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    connection.Query<PedidosDetalle>("DELETE FROM PedidosDetalle");
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }
        public List<PedidosDetalle> verDetallePedido(int id_pedido)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Query<PedidosDetalle>("SELECT * FROM PedidosDetalle where id_master=?", id_pedido).ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }        
        public List<PedidoDetalleCant> ObtenerCantProdPedido(int idpedido)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "kigest_sltosAriel.db")))
                {
                    return connection.Query<PedidoDetalleCant>("SELECT count(*) as cantidad FROM PedidosDetalle where id_master=?",idpedido).ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, "SQLiteEx: " + ex.Message, ToastLength.Short).Show();
                return null;
            }
        }

        //**********************************************************************
        //operaciones sobre la tabla de informacion de sincronizaciones
        //**********************************************************************


        
    }
}