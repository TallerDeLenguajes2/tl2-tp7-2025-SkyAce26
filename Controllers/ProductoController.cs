namespace MisProductos;

using MisProductos;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ProductosController : ControllerBase{
    private ProductoRepository productoRepository;
    public ProductosController(){
        productoRepository = new ProductoRepository();
    }

    [HttpPost("AltaProducto")]
    public ActionResult<string> AltaProducto(Producto nuevoProducto)
    {
        int id = productoRepository.Alta(nuevoProducto);
        return Ok($"Producto dado de alta exitosamente con ID = {id}");
    }

    [HttpGet("GetAllProductos")]
    public ActionResult<List<Producto>> GetProductos()
    {
        List<Producto> listProductos;
        listProductos = productoRepository.GetAll();
        return Ok(listProductos);
    }

    [HttpDelete("BajaProducto")]
    public ActionResult<string> BajaProducto(int id)
    {
        int resultado = productoRepository.Baja(id);
        if (resultado == 0)
        {
            return NotFound("No se encontró el producto");
        }
        else
        {
            return Ok("Producto dado de baja exitosamente");
        }
    }

    [HttpPut("ModificarProducto")]
    public ActionResult<string> ModificarProducto(int id, Producto producto)
    {
        int resultado = productoRepository.ModificarProducto(id, producto);
        if (resultado == 0)
        {
            return NotFound("No se encontró el producto");
        }
        else
        {
            return Ok("Producto modificado exitosamente");
        }
    }

    [HttpGet("Detalles")]
    public ActionResult<Producto> Detalles(int id)
    {
        Producto producto = productoRepository.Detalles(id);
        if(producto == null)
        {
            return NotFound("No se encontró el producto");
        }
        else
        {
            return Ok(producto);
        }
    }
}