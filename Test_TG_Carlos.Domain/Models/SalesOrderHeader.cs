namespace Test_TG_Carlos.Domain.Models;

using System;
using System.Collections.Generic;

public partial class SalesOrderHeader
{
    public SalesOrderHeader()
    {
        SalesOrderDetails = new List<SalesOrderDetail>();
    }

    public SalesOrderHeader(int Id, int CustomerId, DateTime OrderDate, DateTime? ShipDate, byte Status) 
        : this()
    {
        this.SalesOrderID = Id;
        this.CustomerID = CustomerId;
        this.OrderDate = OrderDate;
        this.ShipDate = ShipDate;
        this.Status = Status;
    }

    public int SalesOrderID { get; set; }

    public int CustomerID { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? ShipDate { get; set; }

    /// <summary>
    /// 1. Pending
    /// 2. Sent
    /// 3. Complete
    /// </summary>
    public byte Status { get; set; }

    // Ejercicio 4: Expresión de cuerpo de propiedad
    public double Total => Math.Round(SalesOrderDetails?.Sum(detail => detail.Price) ?? 0, 2);

    public virtual ICollection<SalesOrderDetail> SalesOrderDetails { get; set; }
}
