using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServIT.Data.DTO.SubCategory
{
    public class DeleteSubCategoryDTO
    {
        public Guid SubCategoryId { get; set; }
        public Guid DeletedByUserID { get; set; }
    }
}
