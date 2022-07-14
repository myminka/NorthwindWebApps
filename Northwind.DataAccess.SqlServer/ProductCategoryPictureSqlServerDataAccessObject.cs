using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Northwind.DataAccess.Interfaces;
using Northwind.DataAccess.Products;

namespace Northwind.DataAccess.SqlServer
{
    public sealed class ProductCategoryPictureSqlServerDataAccessObject : IProductCategoryPicturesDataAccessObject
    {
        private readonly SqlConnection connection;
        public ProductCategoryPictureSqlServerDataAccessObject(SqlConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public async Task<bool> DeletePictureAsync(int productCategoryId)
        {
            if (productCategoryId <= 0)
            {
                throw new ArgumentException("Must be greater than zero.", nameof(productCategoryId));
            }

            const string commandText =
@"DELETE FROM dbo.Categories.Picture WHERE CategoryID = @categoryID
SELECT @@ROWCOUNT";

            using (var command = new SqlCommand(commandText, this.connection))
            {
                const string categoryId = "@categoryID";
                command.Parameters.Add(categoryId, SqlDbType.Int);
                command.Parameters[categoryId].Value = productCategoryId;

                var result = await command.ExecuteScalarAsync();

                return (int)result > 0;
            }
        }

        public async Task<byte[]> FindPictureAsync(int productCategoryId)
        {
            if (productCategoryId <= 0)
            {
                throw new ArgumentException("Must be greater than zero", nameof(productCategoryId));
            }

            const string commandText =
@"SELECT c.Picture FROM dbo.Categories as c
WHERE c.CategoryID = @categoryId";

            using (var command = new SqlCommand(commandText, this.connection))
            {
                const string categoryId = "@categoryId";
                command.Parameters.Add(categoryId, SqlDbType.Int);
                command.Parameters[categoryId].Value = productCategoryId;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (!reader.Read())
                    {
                        throw new ProductCategoryNotFoundException(productCategoryId);
                    }

                    const string pictureColumnName = "Picture";
                    byte[] picture = null;

                    if (reader[pictureColumnName] != DBNull.Value)
                    {
                        picture = (byte[])reader["Picture"];
                    }

                    return picture;
                }
            }
        }

        public async Task<bool> UpdatePictureAsync(int productCategoryId, byte[] picture)
        {
            if (productCategoryId <= 0)
            {
                throw new ArgumentException(nameof(productCategoryId));
            }

            if (picture is null)
            {
                throw new ArgumentNullException(nameof(picture));
            }

            const string commandText =
@"UPDATE dbo.Categories.Picture SET Picture = @picture
WHERE CategoryID = @categoryId
SELECT @@ROWCOUNT";

            using (var command = new SqlCommand(commandText, this.connection))
            {
                const string pictureParameter = "@picture";
                command.Parameters.Add(pictureParameter, SqlDbType.Image);
                command.Parameters[pictureParameter].IsNullable = true;

                if (picture is null)
                {
                    command.Parameters[pictureParameter].Value = DBNull.Value;
                }
                else
                {
                    command.Parameters[pictureParameter].Value = picture;
                }

                const string categoryId = "@categoryId";
                command.Parameters.Add(categoryId, SqlDbType.Int);
                command.Parameters[categoryId].Value = productCategoryId;

                var result = await command.ExecuteScalarAsync();
                return (int)result > 0;
            }
        }
    }
}
