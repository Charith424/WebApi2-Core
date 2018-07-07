using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_Core.IContracts;
using WebApi_Core.Models;

namespace WebApi_Core.Controllers
{
    [Produces("application/json")]
    [Route("api/Customers")]
    public class CustomersController : Controller
    {
        ///use for Directly call the EF to Retrive data 
        //private readonly H_Plus_SportsContext _context;

        private readonly ICustomerRepository _customerRepository;
        ///use for Directly call the EF to Retrive data 
        //public CustomersController(H_Plus_SportsContext context)
        //{
        //    _context = context;
        //}

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        //Get All Customer List

        [HttpGet]
        public IActionResult GetCustomer()
        {
            var resulSet = new ObjectResult(_customerRepository.GetAll())
            {
                StatusCode = (int)HttpStatusCode.OK
                // StatusCode =
            };
            Request.HttpContext.Response.Headers.Add("Customer-Total-Count", _customerRepository.GetAll().Count().ToString());

            return resulSet;
        }
        //Get One Cus Detail by Cus Id
        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<IActionResult> GetCustomer([FromRoute] int id)
        {
            //Check Customer Is Exists on that ID if not Show NotFound
            if (await CustomerExists(id))
            {

                var customer = await _customerRepository.Find(id);// SingleOrDefaultAsync(m => m.CustomerId == id);



                return Ok(customer);
            }
            else
            {
                return NotFound();
            }

        }
        // Check CUstomer Exist or not
        private async Task<bool> CustomerExists(int id)
        {

            //Check Customer Exists in DB if that perticular record is there "Any" will return true
            //return _context.Customer.Any(c => c.CustomerId == id);

            return await _customerRepository.Exist(id);

        }

        //Insert Data To DB
        [HttpPost]
        public async Task<IActionResult> PostCustomer([FromBody] Customer customer)
        {
            //Validating the Model State Before APi user data insert to DB
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            await _customerRepository.Add(customer);
            // await _context.SaveChangesAsync();
            return CreatedAtAction("getCustomer", new { id = customer.CustomerId });
        }
        //Update Data
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer([FromRoute] int id, [FromBody] Customer customer)
        {
            //  _context.Entry(customer).State = EntityState.Modified;
            //Handle a Try Catch Block
            try
            {
                await _customerRepository.Update(customer);
                //handle in Dependency repo Pattern
                //   await _customerRepository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("getCustomer", new { id = customer.CustomerId });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DelCustomer([FromRoute] int id)
        {
            //  var customer = _context.Customer.SingleOrDefault(m => m.CustomerId == id);
            //_context.Customer.Remove(customer);
            // await _context.SaveChangesAsync();
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (!await CustomerExists(id))
            {
                return NotFound();
            }

            var customer = _customerRepository.DeleteAsync(id);
            return Ok(customer);
        }
    }
}