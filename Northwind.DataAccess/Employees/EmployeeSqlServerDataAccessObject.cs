using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;

namespace Northwind.DataAccess.Employees
{
    /// <summary>
    /// Represents a SQL Server-tailored DAO for Northwind products.
    /// </summary>
    public sealed class EmployeeSqlServerDataAccessObject : IEmployeeDataAccessObject
    {
        private readonly SqlConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeSqlServerDataAccessObject"/> class.
        /// </summary>
        /// <param name="connection">A <see cref="SqlConnection"/>.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public EmployeeSqlServerDataAccessObject(SqlConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentNullException("Must be greater than zero.", nameof(employeeId));
            }

            const string commandText =
@"DELETE FROM dbo.Employees WHERE EmployeeID = @employeeID
SELECT @@ROWCOUNT";

            using (var command = new SqlCommand(commandText, connection))
            {
                const string employeeIdParameter = "@employeeID";
                command.Parameters.Add(employeeIdParameter, SqlDbType.Int);
                command.Parameters[employeeIdParameter].Value = employeeId;

                var result = await command.ExecuteScalarAsync();
                return ((int)result) > 0;
            }
        }

        /// <inheritdoc/>
        public async Task<EmployeeTransferObject> FindEmployeeAsync(int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentNullException("Must be greater than zero.", nameof(employeeId));
            }

            const string commandText =
@"SELECT e.EmployeeID, e.LastName, e.FirstName, e.Title, e.TitleOfCourtesy, e.BirthDate, e.HireDate, e.Address, e.City, e.Region, e.PostalCode, e.Country, e.HomePhone, e.Extension, e.Photo, e.Notes, e.ReportsTo, e.PhotoPath FROM dbo.Employees as e
WHERE e.EmployeeID = @employeeID";

            using (var command = new SqlCommand(commandText, this.connection))
            {
                const string empoyeeIdParameter = "@employee.ID";
                command.Parameters.Add(empoyeeIdParameter, SqlDbType.Int);
                command.Parameters[empoyeeIdParameter].Value = employeeId;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (!await reader.ReadAsync())
                    {
                        throw new ArgumentException("Employee not found", nameof(employeeId));
                    }
                    
                    return CreateEmployee(reader);
                }
            }
        }

        /// <inheritdoc/>
        public async Task<int> InsertEmployeeAsync(EmployeeTransferObject employee)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            const string commandText =
@"INSERT INTO dbo.Employees (LastName, FirstName, Title, TitleOfCourtesy, BirthDate, HireDate, Address, City, Region, PostalCode, Country, HomePhone, Extension, Photo, Notes, ReportsTo, PhotoPath) OUTPUT Inserted.EmployeeID
VALUES (@lastName, @firstName, @title, @titleOfCourtesy, @birthDate, @hireDate, @address, @city, @region, @postalCode, @country, @homePhone, @extension, @photo, @notes, @reportsTo, @photoPath)";

            using (var command = new SqlCommand(commandText, this.connection))
            {
                AddSqlParameters(employee, command);

                var id = await command.ExecuteScalarAsync();
                return (int)id;
            }
        }

        /// <inheritdoc/>
        public async Task<IList<EmployeeTransferObject>> SelectEmployeeAsync(int offset, int limit)
        {
            if (offset < 0)
            {
                throw new ArgumentException("Must be greater than zero or equal zero.", nameof(offset));
            }

            if (limit < 1)
            {
                throw new ArgumentException("Must be greater than one");
            }

            const string CommandTemplate =
@"SELECT e.EmployeeID, e.LastName, e.FirstName, e.Title, e.TitleOfCourtesy, e.BirthDate, e.HireDate, e.Address, e.City, e.Region, e.PostalCode, e.Country, e.HomePhone, e.Extension, e.Photo, e.Notes, e.ReportsTo, e.PhotoPath FROM dbo.Employees as e
ORDER BY e.EmployeeID
OFFSET {0} ROWS
FETCH FIRST {1} ROWS ONLY";

            string commandText = string.Format(CultureInfo.CurrentCulture, CommandTemplate, offset, limit);
    
            return await this.ExecuteReaderAsync(commandText);
        }

        /// <inheritdoc/>
        public async Task<IList<EmployeeTransferObject>> SelectEmployeeByNameAsync(ICollection<string> employeeLastNames)
        {
            if (employeeLastNames is null)
            {
                throw new ArgumentNullException(nameof(employeeLastNames));
            }

            if (employeeLastNames.Count < 1)
            {
                throw new ArgumentException("Collection is empty.", nameof(employeeLastNames));
            }

            const string commandTemplate =
@"SELECT e.EmployeeID, e.LastName, e.FirstName, e.Title, e.TitleOfCourtesy, e.BirthDate, e.HireDate, e.Address, e.City, e.Region, e.PostalCode, e.Country, e.HomePhone, e.Extension, e.Photo, e.Notes, e.ReportsTo, e.PhotoPath FROM dbo.Employees as e
WHERE e.EmployeeID in ('{0}')
ORDER BY e.EmployeeID";

            string commandText = string.Format(CultureInfo.CurrentCulture, commandTemplate, string.Join("','", employeeLastNames));

            return await this.ExecuteReaderAsync(commandText);
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateEmployeeAsync(EmployeeTransferObject employee)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            const string commandText =
@"UPDATE dbo.Employees
SET LastName = @lastName, FirstName = @firstName, Title = @title, TitleOfCourtesy = @titleOfCourtesy, BirthDate = @birthDate, HireDate = @hireDate, Address = @address, City = @city, Region = @region, PostalCode = @postalCode, Country = @country, HomePhone = @homePhone, Extension = @extension, Photo = @photo, Notes = @notes, ReportsTo = @reportsTo, PhotoPath = @photoPath
WHERE EmployeeID = @employeeID
SELECT @@ROWCOUNT";

            using (var command = new SqlCommand(commandText, this.connection))
            {
                AddSqlParameters(employee, command);

                const string employeeId = "@employeeID";
                command.Parameters.Add(employeeId, SqlDbType.Int);
                command.Parameters[employeeId].Value = employee.EmployeeID;

                var result = await command.ExecuteScalarAsync();
                return ((int)result) > 0;
            }
        }

        private static EmployeeTransferObject CreateEmployee(SqlDataReader reader)
        {
            var id = (int)reader["EmployeeID"];
            var firstName = (string)reader["FirstName"];
            var lastName = (string)reader["LastName"];

            const string titleColumnName = "Title";
            string title = reader[titleColumnName] != DBNull.Value ? (string)reader[titleColumnName] : null;

            const string titleOfCourtesyColumnName = "TitleOfCourtesy";
            string titleOfCourtesy = reader[titleOfCourtesyColumnName] != DBNull.Value ? (string)reader[titleOfCourtesyColumnName] : null;

            const string birthDateColumnName = "BirthDate";
            DateTime? birthDate = reader[birthDateColumnName] != DBNull.Value ? (DateTime)reader[birthDateColumnName] : null;

            const string hireDateColumnName = "HireDate";
            DateTime? hireDate = reader[hireDateColumnName] != DBNull.Value ? (DateTime)reader[hireDateColumnName] : null;

            const string addressColumnName = "Address";
            string address = reader[addressColumnName] != DBNull.Value ? (string)reader[addressColumnName] : null;

            const string cityColumnName = "City";
            string city = reader[cityColumnName] != DBNull.Value ? (string)reader[cityColumnName] : null;

            const string regionColumnName = "Region";
            string region = reader[regionColumnName] != DBNull.Value ? (string)reader[regionColumnName] : null;

            const string postalCodeColumnName = "PostalCode";
            string postalCode = reader[postalCodeColumnName] != DBNull.Value ? (string)reader[postalCodeColumnName] : null;

            const string countryColumnName = "Country";
            string country = reader[countryColumnName] != DBNull.Value ? (string)reader[countryColumnName] : null;

            const string homePhoneColumnName = "HomePhone";
            string homePhone = reader[homePhoneColumnName] != DBNull.Value ? (string)reader[homePhoneColumnName] : null;

            const string extensionColumnName = "Extension";
            string extension = reader[extensionColumnName] != DBNull.Value ? (string)reader[extensionColumnName] : null;

            const string photoColumnName = "Photo";
            byte[] photo = reader[photoColumnName] != DBNull.Value ? (byte[])reader[photoColumnName] : null;

            const string notesColumnName = "Notes";
            string notes = reader[notesColumnName] != DBNull.Value ? (string)reader[notesColumnName] : null;

            const string reportsToColumnName = "ReportsTo";
            int? reportsTo = reader[reportsToColumnName] != DBNull.Value ? (int)reader[reportsToColumnName] : null;

            const string photoPathColumnName = "PhotoPath";
            string photoPath = reader[photoPathColumnName] != DBNull.Value ? (string)reader[photoPathColumnName] : null;

            return new EmployeeTransferObject
            {
                EmployeeID = id,
                FirstName = firstName,
                LastName = lastName,
                Title = title,
                TitleOfCourtesy = titleOfCourtesy,
                BirthDate = birthDate,
                HireDate = hireDate,
                Address = address,
                City = city,
                Region = region,
                PostalCode = postalCode,
                Country = country,
                HomePhone = homePhone,
                Extension = extension,
                Photo = photo,
                Notes = notes,
                ReportsTo = reportsTo,
                PhotoPath = photoPath,
            };
        }

        private static void AddSqlParameters(EmployeeTransferObject employee, SqlCommand command)
        {
            const string lastNameNameParameter = "@lastName";
            command.Parameters.Add(lastNameNameParameter, SqlDbType.NVarChar, 20);
            command.Parameters[lastNameNameParameter].Value = employee.LastName;

            const string firstNameNameParameter = "@firstName";
            command.Parameters.Add(firstNameNameParameter, SqlDbType.NVarChar, 10);
            command.Parameters[firstNameNameParameter].Value = employee.FirstName;

            const string titleNameParameter = "@title";
            command.Parameters.Add(titleNameParameter, SqlDbType.NVarChar, 30);
            command.Parameters[titleNameParameter].IsNullable = true;
            command.Parameters[titleNameParameter].Value = employee.Title != null ? employee.Title : DBNull.Value;

            const string titleOfCourtesyNameParameter = "@titleOfCourtesy";
            command.Parameters.Add(titleOfCourtesyNameParameter, SqlDbType.NVarChar, 25);
            command.Parameters[titleOfCourtesyNameParameter].IsNullable = true;
            command.Parameters[titleOfCourtesyNameParameter].Value = employee.TitleOfCourtesy != null ? employee.TitleOfCourtesy : DBNull.Value;

            const string birthDateNameParameter = "@birthDate";
            command.Parameters.Add(birthDateNameParameter, SqlDbType.DateTime);
            command.Parameters[birthDateNameParameter].IsNullable = true;
            command.Parameters[birthDateNameParameter].Value = employee.BirthDate != null ? employee.BirthDate : DBNull.Value;

            const string hireDateNameParameter = "@hireDate";
            command.Parameters.Add(birthDateNameParameter, SqlDbType.DateTime);
            command.Parameters[hireDateNameParameter].IsNullable = true;
            command.Parameters[hireDateNameParameter].Value = employee.HireDate != null ? employee.HireDate : DBNull.Value;

            const string addressNameParameter = "@address";
            command.Parameters.Add(addressNameParameter, SqlDbType.NVarChar, 60);
            command.Parameters[addressNameParameter].IsNullable = true;
            command.Parameters[addressNameParameter].Value = employee.Address != null ? employee.Address : DBNull.Value;

            const string cityNameParameter = "@city";
            command.Parameters.Add(cityNameParameter, SqlDbType.NVarChar, 15);
            command.Parameters[cityNameParameter].IsNullable = true;
            command.Parameters[cityNameParameter].Value = employee.City != null ? employee.City : DBNull.Value;

            const string regionNameParameter = "@region";
            command.Parameters.Add(regionNameParameter, SqlDbType.NVarChar, 15);
            command.Parameters[regionNameParameter].IsNullable = true;
            command.Parameters[regionNameParameter].Value = employee.Region != null ? employee.Region : DBNull.Value;

            const string postalCodeNameParameter = "@postalCode";
            command.Parameters.Add(postalCodeNameParameter, SqlDbType.NVarChar, 10);
            command.Parameters[postalCodeNameParameter].IsNullable = true;
            command.Parameters[postalCodeNameParameter].Value = employee.PostalCode != null ? employee.PostalCode : DBNull.Value;

            const string countryNameParameter = "@country";
            command.Parameters.Add(countryNameParameter, SqlDbType.NVarChar, 15);
            command.Parameters[countryNameParameter].IsNullable = true;
            command.Parameters[countryNameParameter].Value = employee.Country != null ? employee.Country : DBNull.Value;

            const string homePhoneNameParameter = "@homePhone";
            command.Parameters.Add(homePhoneNameParameter, SqlDbType.NVarChar, 24);
            command.Parameters[homePhoneNameParameter].IsNullable = true;
            command.Parameters[homePhoneNameParameter].Value = employee.HomePhone != null ? employee.HomePhone : DBNull.Value;

            const string extensionNameParameter = "@extension";
            command.Parameters.Add(extensionNameParameter, SqlDbType.NVarChar, 4);
            command.Parameters[extensionNameParameter].IsNullable = true;
            command.Parameters[extensionNameParameter].Value = employee.Extension != null ? employee.Extension : DBNull.Value;

            const string photoNameParameter = "@photo";
            command.Parameters.Add(photoNameParameter, SqlDbType.Image);
            command.Parameters[photoNameParameter].IsNullable = true;
            command.Parameters[photoNameParameter].Value = employee.Photo != null ? employee.Photo : DBNull.Value;

            const string notesNameParameter = "@notes";
            command.Parameters.Add(notesNameParameter, SqlDbType.NText);
            command.Parameters[notesNameParameter].IsNullable = true;
            command.Parameters[photoNameParameter].Value = employee.Notes != null ? employee.Notes : DBNull.Value;

            const string reportsToNameParameter = "@reportsTo";
            command.Parameters.Add(reportsToNameParameter, SqlDbType.Int);
            command.Parameters[reportsToNameParameter].IsNullable = true;
            command.Parameters[reportsToNameParameter].Value = employee.ReportsTo != null ? employee.ReportsTo : DBNull.Value;

            const string photoPathNameParameter = "@photoPath";
            command.Parameters.Add(photoPathNameParameter, SqlDbType.NVarChar, 255);
            command.Parameters[photoPathNameParameter].IsNullable = true;
            command.Parameters[photoPathNameParameter].Value = employee.PhotoPath != null ? employee.PhotoPath : DBNull.Value;
        }

        private async Task<IList<EmployeeTransferObject>> ExecuteReaderAsync(string commandText)
        {
            var employees = new List<EmployeeTransferObject>();

            using (var command = new SqlCommand(commandText, this.connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    employees.Add(CreateEmployee(reader));
                }
            }

            return employees;
        }
    }
}