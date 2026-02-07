using SistemaErp.Dominio.Entidades;
using SistemaErp.Dominio.Interfaces;
using SistemaErp.Infraestructura.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace SistemaErp.Infraestructura.Repositorio
{
    public class MovimientoRepositorio : IMovimientoRepositorio
    {
        private readonly StoredProcedureHelper _spHelper;

        public MovimientoRepositorio(StoredProcedureHelper spHelper)
        {
            _spHelper = spHelper;
        }

        public List<Movimiento> Listar()
        {
            var dt = _spHelper.EjecutarSPTabla("USP_Listado_Movimiento");
            var lista = new List<Movimiento>();

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new Movimiento
                {
                    CodCia = row["COD_CIA"]?.ToString(),
                    CompaniaVenta3 = Convert.ToString(row["COMPANIA_VENTA_3"]),
                    AlmacenVenta = Convert.ToString(row["ALMACEN_VENTA"]),
                    AlmacenDestino = Convert.ToString(row["ALMACEN_DESTINO"]),
                    TipoMovimiento = Convert.ToString(row["TIPO_MOVIMIENTO"]),
                    TipoDocumento = Convert.ToString(row["TIPO_DOCUMENTO"]),
                    NroDocumento = Convert.ToString(row["NRO_DOCUMENTO"]),
                    CodItem2 = Convert.ToString(row["COD_ITEM_2"]),
                    Proveedor = Convert.ToString(row["PROVEEDOR"]),
                    Cantidad = row["CANTIDAD"] == DBNull.Value ? 0 : Convert.ToInt32(row["CANTIDAD"]),
                    DocRef1 = Convert.ToString(row["DOC_REF_1"]),
                    DocRef2 = Convert.ToString(row["DOC_REF_2"]),
                    DocRef3 = Convert.ToString(row["DOC_REF_3"]),
                    DocRef4 = Convert.ToString(row["DOC_REF_4"]),
                    DocRef5 = Convert.ToString(row["DOC_REF_5"]),
                    FechaTransaccion = row["FECHA_TRANSACCION"] == DBNull.Value
                        ? DateTime.MinValue
                        : Convert.ToDateTime(row["FECHA_TRANSACCION"])
                });
            }

            return lista;
        }

        public Movimiento? ObtenerPorId(string codCia)
        {
            var dt = _spHelper.EjecutarSPTabla(
                "USP_Buscar_Movimiento",
                new Dictionary<string, object> { { "@Cod_Cia", codCia } }
            );

            if (dt.Rows.Count == 0)
                return null;

            var row = dt.Rows[0];

            return new Movimiento
            {
                CodCia = row["COD_CIA"]?.ToString(),
                CompaniaVenta3 = row["COMPANIA_VENTA_3"]?.ToString(),
                AlmacenVenta = row["ALMACEN_VENTA"]?.ToString(),
                AlmacenDestino = row["ALMACEN_DESTINO"]?.ToString(),
                TipoMovimiento = row["TIPO_MOVIMIENTO"]?.ToString(),
                TipoDocumento = row["TIPO_DOCUMENTO"]?.ToString(),
                NroDocumento = row["NRO_DOCUMENTO"]?.ToString(),
                CodItem2 = row["COD_ITEM_2"]?.ToString(),
                Proveedor = row["PROVEEDOR"]?.ToString(),
                Cantidad = row["CANTIDAD"] == DBNull.Value ? 0 : Convert.ToInt32(row["CANTIDAD"]),
                DocRef1 = row["DOC_REF_1"]?.ToString(),
                DocRef2 = row["DOC_REF_2"]?.ToString(),
                DocRef3 = row["DOC_REF_3"]?.ToString(),
                DocRef4 = row["DOC_REF_4"]?.ToString(),
                DocRef5 = row["DOC_REF_5"]?.ToString(),
                FechaTransaccion = row["FECHA_TRANSACCION"] == DBNull.Value
                    ? DateTime.MinValue
                    : Convert.ToDateTime(row["FECHA_TRANSACCION"])
            };
        }

        public int Guardar(Movimiento movimiento)
        {
            var result = _spHelper.EjecutarSPScalar(
                "USP_Registrar_Movimiento",
                new Dictionary<string, object>
                {
                    { "@Cod_Cia", movimiento.CodCia },
                    { "@Compania_Venta_3", movimiento.CompaniaVenta3 },
                    { "@Almacen_Venta", movimiento.AlmacenVenta },
                    { "@Tipo_Movimiento", movimiento.TipoMovimiento },
                    { "@Tipo_Documento", movimiento.TipoDocumento },
                    { "@Nro_Documento", movimiento.NroDocumento },
                    { "@Cod_Item_2", movimiento.CodItem2 },
                    { "@Proveedor", movimiento.Proveedor },
                    { "@Almacen_Destino", movimiento.AlmacenDestino },
                    { "@Cantidad", movimiento.Cantidad },
                    { "@Doc_Ref_1", movimiento.DocRef1 },
                    { "@Doc_Ref_2", movimiento.DocRef2 },
                    { "@Doc_Ref_3", movimiento.DocRef3 },
                    { "@Doc_Ref_4", movimiento.DocRef4 },
                    { "@Doc_Ref_5", movimiento.DocRef5 },
                    { "@Fecha_Transaccion", movimiento.FechaTransaccion }
                });

            return result == null ? 0 : Convert.ToInt32(result);
        }

        public void Eliminar(string id)
        {
            _spHelper.EjecutarSP("USP_Eliminar_Movimiento", new Dictionary<string, object>
            {
                { "@Cod_Cia", id }
            });
        }

        public List<Movimiento> ListarFiltrado(string campo, string texto, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var parametros = new Dictionary<string, object>
            {
                { "@Campo", string.IsNullOrEmpty(campo) ? null : campo },
                { "@Texto", string.IsNullOrEmpty(texto) ? null : texto },
                { "@FechaInicio", fechaInicio },
                { "@FechaFin", fechaFin }
            };

            var dt = _spHelper.EjecutarSPTabla("USP_Listar_Movimiento_Filtro", parametros);
            var lista = new List<Movimiento>();

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new Movimiento
                {
                    CodCia = row["COD_CIA"]?.ToString(),
                    CompaniaVenta3 = Convert.ToString(row["COMPANIA_VENTA_3"]),
                    AlmacenVenta = Convert.ToString(row["ALMACEN_VENTA"]),
                    AlmacenDestino = Convert.ToString(row["ALMACEN_DESTINO"]),
                    TipoMovimiento = Convert.ToString(row["TIPO_MOVIMIENTO"]),
                    TipoDocumento = Convert.ToString(row["TIPO_DOCUMENTO"]),
                    NroDocumento = Convert.ToString(row["NRO_DOCUMENTO"]),
                    //CodItem2 = Convert.ToString(row["COD_ITEM_2"]),
                    Proveedor = Convert.ToString(row["PROVEEDOR"]),
                    Cantidad = row["CANTIDAD"] == DBNull.Value ? 0 : Convert.ToInt32(row["CANTIDAD"]),
                    DocRef1 = Convert.ToString(row["DOC_REF_1"]),
                    //DocRef2 = Convert.ToString(row["DOC_REF_2"]),
                    //DocRef3 = Convert.ToString(row["DOC_REF_3"]),
                    //DocRef4 = Convert.ToString(row["DOC_REF_4"]),
                    //DocRef5 = Convert.ToString(row["DOC_REF_5"]),
                    FechaTransaccion = row["FECHA_TRANSACCION"] == DBNull.Value
                        ? DateTime.MinValue
                        : Convert.ToDateTime(row["FECHA_TRANSACCION"])
                });
            }

            return lista;
        }
    }
}
