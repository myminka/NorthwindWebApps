using System.Collections.Generic;

namespace Northwind.Services.Employees
{
    /// <summary>
    /// Represents a management service for employees.
    /// </summary>
    public interface IEmployeeManagementService
    {
        /// <summary>
        /// Show a list of employees
        /// </summary>
        /// <param name="offset">An offset of the first element to return.</param>
        /// <param name="limit">A limit of elements to return.</param>
        /// <returns>A <see cref="IList{T}"/> of <see cref="Employee"/>.</returns>
        IList<Employee> ShowEmployees(int offset, int limit);

        /// <summary>
        /// Try to show a employee with specified identifier.
        /// </summary>
        /// <param name="employeeId">A product category identifier.</param>
        /// <param name="employee">A product category to return.</param>
        /// <returns>Returns true if a product category is returned; otherwise false.</returns>
        bool TryShowEmployee(int employeeId, out Employee employee);

        /// <summary>
        /// Creates a new employee.
        /// </summary>
        /// <param name="employee">A <see cref="Employee"/> to create.</param>
        /// <returns>An identifier of a created emploee.</returns>
        int CreateEmploee(Employee employee);

        /// <summary>
        /// Destroys an existed employee.
        /// </summary>
        /// <param name="categoryId">A employee identifier.</param>
        /// <returns>True if a product category is destroyed; otherwise false.</returns>
        bool DestroyEmployee(int employeeId);

        /// <summary>
        /// Looks up for employees with specified names.
        /// </summary>
        /// <param name="names">A list of emploee names.</param>
        /// <returns>A list of emploee with specified names.</returns>
        IList<Employee> LookupEmployeesByName(IList<string> names);

        /// <summary>
        /// Updates a employee.
        /// </summary>
        /// <param name="employee">A employee identifier.</param>
        /// <returns>True if a employee is updated; otherwise false.</returns>
        bool UpdateEmployee(int employeeId, Employee employee);
    }
}