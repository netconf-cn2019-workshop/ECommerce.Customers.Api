using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorrelationId;
using ECommerce.Customers.Api.Model;
using ECommerce.Customers.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ECommerce.Customers.Api.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(
            ICustomerRepository customerRepository,
            ICorrelationContextAccessor correlationContextAccessor,
            ILogger<CustomersController> logger)
        {
            _customerRepository = customerRepository;
            _correlationContextAccessor = correlationContextAccessor;
            _logger = logger;
        }

        // GET api/customers
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            _logger.LogDebug("返回所有顾客信息");
            return _customerRepository.GetAll();
        }

        [Route("Search")]
        [HttpPost]
        public IEnumerable<Customer> Get([FromBody]Customer query)
        {
            _logger.LogDebug("查询顾客信息");
            return _customerRepository.GetAll(query);
        }

        // GET api/customers/5
        [HttpGet("{id}")]
        public Customer Get(int id)
        {
            var correlationId = _correlationContextAccessor.CorrelationContext.CorrelationId;

            _logger.LogDebug($"返回 Id 为'{id}' 的顾客信息");

            return _customerRepository.GetByID(id);
        }

        // POST api/customers
        [HttpPost]
        public void Post([FromBody]Customer value)
        {
            _customerRepository.Add(value);
        }

        // PUT api/customers/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Customer value)
        {
            _customerRepository.Update(value);
        }

        // DELETE api/customers/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _customerRepository.Delete(id);
        }
    }
}
