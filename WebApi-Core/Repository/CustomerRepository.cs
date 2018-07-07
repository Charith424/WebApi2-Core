using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_Core.IContracts;
using WebApi_Core.Models;

namespace WebApi_Core.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private H_Plus_SportsContext _context;
        public CustomerRepository(H_Plus_SportsContext content)
        {
            _context = content;
        }
        public async Task<Customer> Add(Customer customer)
        {
            await _context.Customer.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> DeleteAsync(int id)
        {
            var Customer = await _context.Customer.SingleAsync(a => a.CustomerId == id);
            _context.Customer.Remove(Customer);
            await _context.SaveChangesAsync();
            return Customer;
        }

        public async Task<bool> Exist(int id)
        {
            return await _context.Customer.AnyAsync(c => c.CustomerId == id);

        }

        public async Task<Customer> Find(int id)
        {
            return await _context.Customer.Include(customer => customer.Order).SingleOrDefaultAsync(a => a.CustomerId == id);
            //return await _context.Customer.Include(customer => customer.CustomerId == id).SingleOrDefaultAsync();
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customer;
        }

        public async Task<Customer> Update(Customer customer)
        {
            _context.Customer.Update(customer);
            await _context.SaveChangesAsync();
            return customer;
        }
    }
}
