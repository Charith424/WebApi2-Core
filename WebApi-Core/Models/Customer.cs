using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi_Core.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Order = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        //Model Validating
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        //Type Validating
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }

        public ICollection<Order> Order { get; set; }
    }
}
