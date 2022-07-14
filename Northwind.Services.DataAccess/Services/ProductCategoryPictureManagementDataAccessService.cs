using System;
using System.Threading.Tasks;
using Northwind.DataAccess.Interfaces;
using Northwind.DataAccess.SqlServer;

namespace Northwind.Services.DataAccess.Services
{
    public class ProductCategoryPictureManagementDataAccessService : IProductCategoryPicturesDataAccessObject
    {
        private readonly IProductCategoryPicturesDataAccessObject dao;

        public ProductCategoryPictureManagementDataAccessService(NorthwindDataAccessFactory accessFactory)
        {
            this.dao = accessFactory is null ? throw new ArgumentNullException(nameof(accessFactory))
                : accessFactory.GetProductCategoryPicturesDataAccessObject();
        }

        public Task<bool> DeletePictureAsync(int productCategoryId)
        {
            return this.dao.DeletePictureAsync(productCategoryId);
        }

        public Task<byte[]> FindPictureAsync(int productCategoryId)
        {
            return this.dao.FindPictureAsync(productCategoryId);
        }

        public Task<bool> UpdatePictureAsync(int productCategoryId, byte[] picture)
        {
            return this.dao.UpdatePictureAsync(productCategoryId, picture);
        }
    }
}