namespace MisProductos;
public class Presupuesto {
    private int _idPresupuesto;
    private string _nombreDestinatario;
    private DateTime _fechaCreacion;

    private List<PresupuestoDetalle> _presupuestoDetalle;

    public int IdPresupuesto
    {
        get { return _idPresupuesto; }
        set { _idPresupuesto = value; }
    }

    public string NombreDestinatario
    {
        get { return _nombreDestinatario; }
        set { _nombreDestinatario = value; }
    }

    public DateTime FechaCreacion
    {
        get { return _fechaCreacion; }
        set { _fechaCreacion = value; }
    }
    
    public List<PresupuestoDetalle> PresupuestoDetalle
    {
        get { return _presupuestoDetalle; }
        set { _presupuestoDetalle = value; }
    }

    public double montoPresupuesto(){
        return PresupuestoDetalle.Sum(d => d.Cantidad * d.Producto.Precio);
    }

    public double montoPresupuestoConIva(){
        double monto = montoPresupuesto();
        return monto + (monto * 0.21);
    }

    public int cantidadProductos(){
        return PresupuestoDetalle.Sum(d => d.Cantidad);
    }
    
}