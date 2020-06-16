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

namespace SistemaPedidos.Resources.Interface
{
    [Headers("User-Agent: :request:")]

    interface IObtenerEstadoVendedor
    {

        [Get("/webservicesltosariel/fact_EstadoVendedor_obtener.php")]
        Task<RespuestaServerUsuarios> GetServerUsuarios();
    }
    interface IObtenerClientesServer
    {
        
        [Get("/webservicesltosariel/fact_clientes_obtener.php")]
        Task<RespuestaServerClientes> GetServerClientes();
    }
    interface IModificarClienteServer
    {
        [Post("/webservicesltosariel/fact_clientes_modificar.php")]
        Task ModificarClienteServer([Body]ClienteServer clienteServer);
    }
    interface IEliminarClienteServer
    {
        [Post("/webservicesltosariel/fact_clientes_eliminar.php")]
        Task EliminarClienteServer([Body]ClienteServer clienteServer);
    }
    interface IObtenerProductosServer
    {
        [Get("/webservicesltosariel/fact_productos_obtener.php")]
        Task<RespuestaServerProductos> GetServerProductos();
    }
    interface IObtenerListaPrecio
    {
        [Get("/webservicesltosariel/fact_ListasPrecio_obtener.php")]
        Task<RespuestaServerListasPrecio> GetServerListasPrecio();
    }
    interface IObtenerPromocionesDescuentos
    {
        [Get("/webservicesltosariel/fact_PromocionesDescuentos_obtener.php")]
        Task<RespuestaServerPromocionesDescuentos> GetServerPromocionesDescuentos();
    }
    interface IObtenerCategoriaProductos
    {
        [Get("/webservicesltosariel/fact_CategoriaProductos_obtener.php")]
        Task<RespuestaServerCategProductos> GetServerCategProductos();
    }
    interface ISubirPedidoMaster
    {
        [Post("/webservicesltosariel/fact_pedidos_insertar.php")]
        Task SubirPedidoMaster([Body]PedidosMasterServer pedidosMasterServer);
    }
    [Headers("Content-Type:application/json")]
    interface ISubirPedidoDetalle
    {
        [Post("/webservicesltosariel/fact_pedidos_detalle_insertar.php")]
        Task SubirPedidoDetalle([Body] PedidosDetalleServer pedidosDetalleServer);
    }
}