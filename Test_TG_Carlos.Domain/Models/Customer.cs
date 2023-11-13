namespace Test_TG_Carlos.Domain.Models;

using Newtonsoft.Json;
using System.Collections.Generic;

public partial class Customer
{
    public Customer() { }

    public Customer(int Id, string Name, string EmailAddress)
    {
        this.CustomerID = Id;
        this.Name = Name;
        this.EmailAddress = EmailAddress;
        //mio para que no salga null Products
        //this.Products = new List<Product>();
    }

    public int CustomerID { get; set; }

    public string? Name { get; set; }

    public string? EmailAddress { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public virtual ICollection<SalesOrderHeader>? SalesOrderHeader { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public virtual ICollection<Product>? Products { get; set; }
}
