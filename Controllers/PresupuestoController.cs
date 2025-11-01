namespace MisProductos;

using MisProductos;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]

public class PresupuestoController : ControllerBase
{
    private PresupuestoRepository presupuestoRepository;

    public PresupuestoController()
    {
        presupuestoRepository = new PresupuestoRepository();
    }
    [HttpPost("AltaPresupuesto")]
    public ActionResult<string> AltaPresupuesto(Presupuesto nuevoPre)
    {
        int id = presupuestoRepository.Alta(nuevoPre);
        return Ok($"Presupuesto dado de alta exitosamente con ID = {id}");
    }

    [HttpPost("AltaDetalles")]
    public ActionResult<string> AltaDetalles([FromRoute]int id, [FromBody] PresupuestoDetalle detalle)
    {
        presupuestoRepository.AgregarDetalle(id, detalle.Producto.IdProducto, detalle.Cantidad);

        return Ok("Producto agregado al presupuesto correctamente");
    }

    [HttpGet("GetAllPresupuestos")]
    public ActionResult<List<Presupuesto>> GetAllPresupuestos()
    {
        List<Presupuesto> listPresupuesto;
        listPresupuesto = presupuestoRepository.GetAll();
        return Ok(listPresupuesto);
    }

    [HttpGet("GetDetalles")]
    public ActionResult<Presupuesto> GetDetalles(int id)
    {
        Presupuesto presupuesto = presupuestoRepository.Detalles(id);
        if (presupuesto == null)
        {
            return NotFound("No se encontró el producto");
        }
        else
        {
            return Ok(presupuesto);
        }
    }
    

    /*
    [HttpDelete("BajaPresupuesto")]
    public ActionResult<string> BajaPresupuesto([FromRoute]int id)
    {
        int resultado = presupuestoRepository.Eliminar(id);
        if (resultado == 0)
        {
            return NotFound("No se encontró el presupuesto");
        }
        else
        {
            return Ok("Presupuesto dado de baja exitosamente");
        }
    }*/

    [HttpDelete("Presupuesto/{id}")]
        public IActionResult Eliminar(int id)
        {
            return presupuestoRepository.Eliminar(id) ? Ok() : BadRequest();
        }

}