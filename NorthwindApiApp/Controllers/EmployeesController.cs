using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.DataAccess.Employees;

namespace NorthwindApiApp.Controllers
{
    [ApiController]
    [Route("employee")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeDataAccessObject service;
        
        /// <summary>
        /// asdfsagsa
        /// </summary>
        /// <param name="service"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public EmployeesController(IEmployeeDataAccessObject service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Get all employees.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetEmployees")]
        public async Task<ActionResult<IEnumerable<EmployeeTransferObject>>> GetAll(int offset = 2, int limit = 5)
        {
            return (List<EmployeeTransferObject>)await this.service.SelectEmployeeAsync(offset, limit);
        }

        [HttpGet("{id}", Name = "GetEmployeeById")]
        public async Task<ActionResult<EmployeeTransferObject>> Get(int id)
        {
            var employee = await this.service.FindEmployeeAsync(id);

            return employee is null ? this.NotFound() : employee;
        }

        [HttpPost(Name = "CreateEmployee")]
        public async Task<IActionResult> Create(EmployeeTransferObject employee)
        {
            if (employee is null)
            {
                return this.BadRequest();
            }

            await this.service.InsertEmployeeAsync(employee);
            return this.CreatedAtAction(nameof(Create), employee);
        }

        [HttpPut("{id}", Name = "UpdateEmployee")]
        public async Task<IActionResult> Update(int id, EmployeeTransferObject employee)
        {
            if (employee is null)
            {
                return this.NotFound();
            }

            return await this.service.UpdateEmployeeAsync(employee) ? this.NoContent() : this.NotFound();
        }

        [HttpDelete("{id}", Name = "DeleteEmployee")]
        public async Task<IActionResult> Delete(int id)
        {
            return await this.service.DeleteEmployeeAsync(id) ? this.NoContent() : this.NotFound();
        }
    }
}
