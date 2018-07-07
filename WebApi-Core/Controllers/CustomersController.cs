using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_Core.Models;

namespace WebApi_Core.Controllers
{
    [Produces("application/json")]
    [Route("api/Customers")]
    public class CustomersController : Controller
    {
        private readonly H_Plus_SportsContext _context;
        public CustomersController(H_Plus_SportsContext context)
        {
            _context = context;
        }
        //Get All Customer List

        [HttpGet]
        public IActionResult GetCustomer()
        {
            var resulSet = new ObjectResult(_context.Customer)
            {
                StatusCode = (int)HttpStatusCode.OK
                // StatusCode =
            };
            Request.HttpContext.Response.Headers.Add("Customer-Total-Count", _context.Customer.Count().ToString());

            return resulSet;
        }
        //Get One Cus Detail by Cus Id
        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<IActionResult> GetCustomer([FromRoute] int id)
        {
            //Check Customer Is Exists on that ID if not Show NotFound
            if (CustomerExists(id))
            {
                var customer = await _context.Customer.SingleOrDefaultAsync(m => m.CustomerId == id);
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }
        // Check CUstomer Exist or not
        private bool CustomerExists(int id)
        {
            //Check Customer Exists in DB if that perticular record is there "Any" will return true
            return _context.Customer.Any(c => c.CustomerId == id);
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
            _context.Customer.Add(customer);
            await _context.SaveChangesAsync();
            return CreatedAtAction("getCustomer", new { id = customer.CustomerId });
        }
        //Update Data
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer([FromRoute] int id, [FromBody] Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
            //Handle a Try Catch Block
            try
            {
                _context.Customer.Add(customer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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
            var customer = _context.Customer.SingleOrDefault(m => m.CustomerId == id);
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
            return Ok(customer);
        }
    }
}