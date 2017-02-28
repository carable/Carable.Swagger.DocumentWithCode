using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SampleWebApi.Dto;
using SampleWebApi.Models;
using SampleWebApi.Models.Examples;

namespace SampleWebApi.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        /// <summary>
        /// Get all customers
        /// </summary>
        /// <returns>an array of all customers</returns>
        [HttpGet]
        [ExampleCustomers]
        [Produces(typeof(Customer[]))]
        public IEnumerable<Customer> Get()
        {
            return new[] {"value1", "value2"}.Select(s=>new Customer(s));
        }

        /// <summary>
        /// Get a single customer with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ExampleCustomer]
        [Produces(typeof(Customer))]
        public Customer Get(int id)
        {
            return new Customer("value");
        }

        // POST api/values
        [HttpPost]
        [ExampleInputCustomer]
        public void Post([FromBody] Customer customer)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [ExampleInputCustomer]
        public void Put(int id, [FromBody] Customer customer)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}