using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Northwind.DataAccess.Employees;
using Northwind.DataAccess.SqlServer;

namespace Northwind.Services.DataAccess.Services
{
    public class EmployeeManagementDataAccessService : IEmployeeDataAccessObject
    {
        private readonly IEmployeeDataAccessObject dao;

        public EmployeeManagementDataAccessService(NorthwindDataAccessFactory accessFactory)
        {
            this.dao = accessFactory is null ? throw new ArgumentNullException(nameof(accessFactory))
                : accessFactory.GetEmployeeDataAccessObject();
        }

        public Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            return this.dao.DeleteEmployeeAsync(employeeId);
        }

        public Task<EmployeeTransferObject> FindEmployeeAsync(int employeeId)
        {
            return this.dao.FindEmployeeAsync(employeeId);
        }

        public Task<int> InsertEmployeeAsync(EmployeeTransferObject employee)
        {
            return this.dao.InsertEmployeeAsync(employee);
        }

        public Task<IList<EmployeeTransferObject>> SelectEmployeeAsync(int offset, int limit)
        {
            return this.dao.SelectEmployeeAsync(offset, limit);
        }

        public Task<IList<EmployeeTransferObject>> SelectEmployeeByNameAsync(ICollection<string> employeeLastNames)
        {
            return this.dao.SelectEmployeeByNameAsync(employeeLastNames);
        }

        public Task<bool> UpdateEmployeeAsync(EmployeeTransferObject employee)
        {
            return this.dao.UpdateEmployeeAsync(employee);
        }
    }
}