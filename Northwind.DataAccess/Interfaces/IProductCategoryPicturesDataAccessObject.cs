using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.DataAccess.Interfaces
{
    public interface IProductCategoryPicturesDataAccessObject
    {
        //upload picture
        Task<bool> UpdatePictureAsync(int productCategoryId, byte[] picture);

        //get picture
        Task<byte[]> FindPictureAsync(int productCategoryId);

        //delete picture
        Task<bool> DeletePictureAsync(int productCategoryId);
    }
}
