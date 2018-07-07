using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_Core.Models;

namespace WebApi_Core.IContracts
{
    public interface ICustomerRepository
    {
        Task<Customer> Add(Customer customer);

        IEnumerable<Customer> GetAll();

        Task<Customer> Find(int id);

        Task<Customer> Update(Customer customer);

        Task<Customer> DeleteAsync(int id);

        Task<bool> Exist(int id);

    }
}
