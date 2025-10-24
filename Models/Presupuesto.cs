namespace MisProductos;
public class Presupuesto{
    public int IdPresupuesto {get;set;}
    public string NombreDestinatario {get;set;}
    public DateTime FechaCreacion {get;set;}
    public List<PresupuestoDetalle> PresupuestoDetalle {get;set;}

    /*
    public float montoPresupuesto(){

    }

    public float montoPresupuestoConIva(){

    }

    public int cantidadProductos(){

    }
    */

}