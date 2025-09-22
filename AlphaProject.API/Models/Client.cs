using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AlphaProject.API.Models;

[Index("Email", Name = "UQ__Clients__A9D10534EA9BE9BA", IsUnique = true)]
public partial class Client
{
    [Key]
    public int ClientId { get; set; }

    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [StringLength(100)]
    public string LastName { get; set; } = null!;

    [StringLength(255)]
    public string Email { get; set; } = null!;

    [StringLength(20)]
    public string? Phone { get; set; }

    public DateTime CreatedAt { get; set; }

    [InverseProperty("Client")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
