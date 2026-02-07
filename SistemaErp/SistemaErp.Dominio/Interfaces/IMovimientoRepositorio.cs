using SistemaErp.Dominio.Entidades;

namespace SistemaErp.Dominio.Interfaces;


public interface IMovimientoRepositorio
{
    List<Movimiento> Listar();
    Movimiento ObtenerPorId(string id);
    int Guardar(Movimiento cliente);
    void Eliminar(string id);
    List<Movimiento> ListarFiltrado(
        string campo,
        string texto,
        DateTime? fechaInicio,
        DateTime? fechaFin
    );
}

