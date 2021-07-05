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
using System.Threading.Tasks;
using Refit;
using SistemaPedidos.Resources.Model;
using SistemaPedidos.Resources;

namespace SistemaPedidos.Resources.Interface
{
    [Headers("User-Agent: :request:")]

    interface IObtenerEstadoVendedor
    {
        

        [Get("/fact_EstadoVendedor_obtener.php")]
        Task<RespuestaServerUsuarios> GetServerUsuarios();
    }
    interface IObtenerClientesServer
    {
        
        [Get("/fact_clientes_obtener.php")]
        Task<RespuestaServerClientes> GetServerClientes();
    }
    interface IModificarClienteServer
    {
        [Post("/fact_clientes_modificar.php")]
        Task ModificarClienteServer([Body]ClienteServer clienteServer);
    }
    interface IEliminarClienteServer
    {
        [Post("/fact_clientes_eliminar.php")]
        Task EliminarClienteServer([Body]ClienteServer clienteServer);
    }
    interface IObtenerProductosServer
    {
        [Get("/fact_productos_obtener.php")]
        Task<RespuestaServerProductos> GetServerProductos();
    }
    interface IObtenerListaPrecio
    {
        [Get("/fact_ListasPrecio_obtener.php")]
        Task<RespuestaServerListasPrecio> GetServerListasPrecio();
    }
    interface IObtenerPromocionesDescuentos
    {
        [Get("/fact_PromocionesDescuentos_obtener.php")]
        Task<RespuestaServerPromocionesDescuentos> GetServerPromocionesDescuentos();
    }
    interface IObtenerCategoriaProductos
    {
        [Get("/fact_CategoriaProductos_obtener.php")]
        Task<RespuestaServerCategProductos> GetServerCategProductos();
    }
    interface IVerEstadisticasVentas
    {
        [Get("/fact_verEstadisticasVenta_obtener.php?vendedor={vendedor}&desde={desde}&hasta={hasta}")]
        Task<RespuestaEstadisticasVenta> verEstadisticasVentas(int vendedor,string desde, string hasta);
    }
    interface IVerConfiguraciones
    {
        [Get("/fact_configuraciones_obtener.php?vendedor={vendedor}")]
        Task<RespuestaServerConfiguraciones > verConfiguraciones(int vendedor);
    }
    interface IVerDevolucionesTotales
    {
        [Get("/fact_verDevolucionesTotales_obtener.php?vendedor={vendedor}&desde={desde}&hasta={hasta}")]
        Task<RespuestaEstadisticasDevolucion> verDevolucionesTotales(int vendedor, string desde, string hasta);
    }

    interface ISubirPedidoMaster
    {
        [Post("/fact_pedidos_insertar.php")]
        Task SubirPedidoMaster([Body]PedidosMasterServer pedidosMasterServer);
    }
    [Headers("Content-Type:application/json")]
    interface ISubirPedidoDetalle
    {
        [Post("/fact_pedidos_detalle_insertar.php")]
        Task SubirPedidoDetalle([Body] PedidosDetalleServer pedidosDetalleServer);
    }
}