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
using SistemaPedidos.Resources.DataHelper;
using SistemaPedidos.Resources.Model;

namespace SistemaPedidos.Resources
{
    public class FuncionesGlobales
    {
        public string CalcularPrecioLista(string precio, string utilidad, string utilidad1, string utilidad2, string iva, int idLista, int moneda)
        {
            List<ListasPrecio> listasPrecios = new List<ListasPrecio>();
            ConsultasTablas dbuser = new ConsultasTablas();
            double cotizacion = 0;
            if (moneda == 1)
            {
                cotizacion = 1;
            }
            else if (moneda==2) 
            {
                cotizacion = VariablesGlobales.CotizacionDolar;
            }
            string Precios = "";

            if (iva == "") { iva = "0"; }
            if (precio == "") { precio = "0"; }
            if (utilidad == "") { utilidad = "0"; }
            if (utilidad1 == "") { utilidad1 = "0"; }
            if (utilidad2 == "") { utilidad2 = "0"; }

            precio = precio.Replace(",", ".");
            iva = iva.Replace(",", ".");
            utilidad = utilidad.Replace(",", ".");
            utilidad1 = utilidad1.Replace(",", ".");
            utilidad2 = utilidad2.Replace(",", ".");

            double LISTA =0;
            double precioFinal=0;

            double IVA= Convert.ToDouble(iva);
            double PRECIO = Convert.ToDouble(precio);
            double GANANCIA = Convert.ToDouble(utilidad);
            double GANANCIA1 = Convert.ToDouble(utilidad1);
            double GANANCIA2 = Convert.ToDouble(utilidad2);

            IVA = (IVA + 100) / 100;
            GANANCIA = (GANANCIA + 100) / 100;
            GANANCIA1 = (GANANCIA1 + 100) / 100;
            GANANCIA2 = (GANANCIA2 + 100) / 100;

            if (idLista == 0)
            {
                listasPrecios = dbuser.VerListaPrecio();
                
                for (int i = 0; i <= listasPrecios.Count() - 1; i++)
                {
                    string modo = listasPrecios[i].utilidad;
                    int auxcol = listasPrecios[i].auxcol;
                    if (modo.Contains("%"))
                    {
                        LISTA = Convert.ToDouble(listasPrecios[i].utilidad.Replace(",", ".").Replace("%",""));
                        LISTA = (LISTA + 100) / 100;
                        
                        precioFinal = Math.Round(PRECIO * IVA * LISTA * cotizacion, 2);
                        Precios = Precios + listasPrecios[i].nombre + " : $" + precioFinal+ "\n";
                    }
                    else
                    {
                        LISTA = Convert.ToDouble(listasPrecios[i].utilidad.Replace(",", "."));
                        LISTA = (LISTA + 100) / 100;

                        switch (auxcol)
                        {
                            case 0:
                                precioFinal = Math.Round(PRECIO * GANANCIA * IVA * LISTA * cotizacion, 2);
                                Precios = Precios + listasPrecios[i].nombre + " : $" + precioFinal + "\n";
                                break;
                            case 1:
                                precioFinal = Math.Round(PRECIO * GANANCIA1 * IVA * LISTA * cotizacion, 2);
                                Precios = Precios + listasPrecios[i].nombre + " : $" + precioFinal + "\n";
                                break;
                            case 2:
                                precioFinal = Math.Round(PRECIO * GANANCIA2 * IVA * LISTA * cotizacion, 2);
                                Precios = Precios + listasPrecios[i].nombre + " : $" + precioFinal + "\n"; 
                                break;
                        }                                                                             
                    }
                }
                return Precios;
            }
            else
            {
                listasPrecios = dbuser.VerListaPrecioId(idLista);
                string modo = listasPrecios[0].utilidad;
                int auxcol = listasPrecios[0].auxcol;
                if (modo.Contains("%"))
                {
                    LISTA = Convert.ToDouble(listasPrecios[0].utilidad.Replace(",", ".").Replace("%", ""));
                    LISTA = (LISTA + 100) / 100;

                    precioFinal = Math.Round(PRECIO * IVA * LISTA * cotizacion, 2);
                    Precios = precioFinal.ToString();
                    return Precios;
                }
                else
                {
                    LISTA = Convert.ToDouble(listasPrecios[0].utilidad.Replace(",", "."));
                    LISTA = (LISTA + 100) / 100;

                    switch (auxcol)
                    {
                        case 0:
                            precioFinal = Math.Round(PRECIO * GANANCIA * IVA * LISTA * cotizacion, 2);
                            Precios = precioFinal.ToString() ;
                            break;
                        case 1:
                            precioFinal = Math.Round(PRECIO * GANANCIA1 * IVA * LISTA * cotizacion, 2);
                            Precios = precioFinal.ToString();
                            break;
                        case 2:
                            precioFinal = Math.Round(PRECIO * GANANCIA2 * IVA * LISTA * cotizacion, 2);
                            Precios = precioFinal.ToString();
                            break;
                    }
                    return Precios;
                }
            }
        }
        public string CalcularPromosDescuentos(int idproducto)
        {
            List<PromocionesDescuentos> promocionesDescuentos = new List<PromocionesDescuentos>();
            ConsultasTablas dbuser = new ConsultasTablas();
            promocionesDescuentos = dbuser.VerPromocionesDescuentosIdProd(idproducto);
            string calculo = "";
            
            for (int i = 0; i <= promocionesDescuentos.Count() - 1; i++)
            {
                calculo += "x" + promocionesDescuentos[i].compra_min + ": " + promocionesDescuentos[i].descuento_porc + "% -"; 
            }
            return calculo;    
        }
        public string AplicarPromosDescuentos(int idproducto, string cantidad)
        {
            List<PromocionesDescuentos> promocionesDescuentos = new List<PromocionesDescuentos>();
            ConsultasTablas dbuser = new ConsultasTablas();
            promocionesDescuentos = dbuser.VerPromocionesDescuentosCant (idproducto,int.Parse(cantidad));

            double porcdesc = 0, actualVal = 0, maxVal = 0;
            if (promocionesDescuentos.Count() == 0)
            {
                return "1";
            }
            else
            { 
                for (int i = 0; i <= promocionesDescuentos.Count() - 1; i++)
                {
                    actualVal = double.Parse(promocionesDescuentos[i].compra_min);
                    if (actualVal > maxVal)
                    {
                    maxVal = actualVal;
                    porcdesc = (double.Parse(promocionesDescuentos[i].descuento_porc) + 100) / 100;
                    }
                }
                return porcdesc.ToString();
            }
        }
        public double SumarItems (string precio, string cantidad)
        {
            double precioTotal;
            double Precio = Convert.ToDouble(precio);
            double Cantidad = Convert.ToDouble(cantidad);

            precioTotal = Precio * Cantidad;
            return precioTotal;           
        }
    }       
}