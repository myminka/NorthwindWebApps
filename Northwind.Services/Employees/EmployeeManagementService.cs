using System.Collections.Generic;
using System.Linq;

namespace Northwind.Services.Employees
{
    /// <summary>
    /// Represents a management service for employees.
    /// </summary>
    public class EmployeeManagementService : IEmployeeManagementService
    {
        private readonly EmployeeContext context;
        public EmployeeManagementService()
        {
            this.context = new EmployeeContext();
        }

        /// <inheritdoc/>
        public int CreateEmploee(Employee employee)
        {
            employee.EmployeeID = this.context.EmployeeId++;
            this.context.Employees.Add(employee);
            return employee.EmployeeID;
        }

        /// <inheritdoc/>
        public bool DestroyEmployee(int employeeId)
        {
            var c = this.context.Employees.RemoveAll(p => p.EmployeeID == employeeId);
            return c != 0;
        }

        /// <inheritdoc/>
        public IList<Employee> LookupEmployeesByName(IList<string> names)
        {
            return this.context.Employees.Where(e => names.Contains(e.LastName)).ToList();
        }

        /// <inheritdoc/>
        public IList<Employee> ShowEmployees(int offset, int limit)
        {
            return this.context.Employees.ToList();
        }

        /// <inheritdoc/>
        public bool TryShowEmployee(int employeeId, out Employee employee)
        {
            var res = this.context.Employees.Any(p => p.EmployeeID == employeeId);
            employee = res ? this.context.Employees.First(p => p.EmployeeID == employeeId) : null;
            return res;
        }

        /// <inheritdoc/>
        public bool UpdateEmployee(int employeeId, Employee employee)
        {
            var index = this.context.Employees.FindIndex(e => e.EmployeeID == employeeId);

            if (index != -1)
            {
                employee.EmployeeID = this.context.EmployeeId++;
                this.context.Employees[index] = employee;
            }

            return index != -1;
        }

        private class EmployeeContext
        {
            public List<Employee> Employees { get; } = new List<Employee>();

            public int EmployeeId { get; set; } = 0;
        }
    }
}
