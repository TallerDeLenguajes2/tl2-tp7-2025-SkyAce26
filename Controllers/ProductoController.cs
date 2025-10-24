namespace MisProductos;

using MisProductos;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ProductoController : ControllerBase{
    private ProductoRepository productoRepository;
    public ProductoController(){
        productoRepository = new ProductoRepository();
    }

    [HttpPost("AltaProducto")]
    public ActionResult<string> AltaProducto(Producto nuevoProducto){
        productoRepository.Alta(nuevoProducto);
        return Ok("Producto dado de alta exitosamente");
    }
}