using SistemaErp.Dominio.Entidades;
using SistemaErp.Dominio.Interfaces;
using System.Collections.Generic;

namespace SistemaErp.Aplicacion.Servicio
{
    public class MovimientoServicio
    {
        private readonly IMovimientoRepositorio _repositorio;

        public MovimientoServicio(IMovimientoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public List<Movimiento> Listar() => _repositorio.Listar();
        public Movimiento ObtenerPorId(string id) => _repositorio.ObtenerPorId(id);
        public int Guardar(Movimiento cliente) => _repositorio.Guardar(cliente);
        public void Eliminar(string id) => _repositorio.Eliminar(id);

        public List<Movimiento> ListarFiltrado(
            string campo,
            string texto,
            DateTime? fechaInicio,
            DateTime? fechaFin)
                {
                    return _repositorio.ListarFiltrado(
                        campo,
                        texto,
                        fechaInicio,
                        fechaFin
                    );
                }

            }
}

