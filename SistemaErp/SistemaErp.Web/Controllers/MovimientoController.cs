using Microsoft.AspNetCore.Mvc;
using SistemaErp.Aplicacion.Servicio;
using SistemaErp.Dominio.Entidades;

namespace SistemaErp.Web.Controllers
{
    public class MovimientoController : Controller
    {
        private readonly MovimientoServicio _servicio;

        public MovimientoController(MovimientoServicio servicio)
        {
            _servicio = servicio;
        }

        public IActionResult Index(string campo, string texto, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var movimientos = _servicio.ListarFiltrado(campo, texto, fechaInicio, fechaFin);
            return View(movimientos);
        }


        [HttpGet]
        public IActionResult Crear() => PartialView("_MovimientoModal", new Movimiento());

        [HttpGet]
        public IActionResult Editar(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var movimiento = _servicio.ObtenerPorId(id);
            if (movimiento == null)
                return NotFound();

            return PartialView("_MovimientoModal", movimiento);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guardar(Movimiento model, string EsEdicion)
        {
            bool esEdicion = EsEdicion == "1";

            _servicio.Guardar(model);

            return Json(new
            {
                success = true,
                message = esEdicion
                    ? "Movimiento actualizado correctamente"
                    : "Movimiento registrado correctamente"
            });
        }

        // ELIMINAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(string id)
        {
            _servicio.Eliminar(id);
            return Json(new { success = true, message = "Movimiento eliminado correctamente" });
        }
    }
}
