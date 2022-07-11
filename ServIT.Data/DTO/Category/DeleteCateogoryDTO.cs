using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServIT.Data.DTO.Category
{
    public class DeleteCategoryDTO
    {
        public long CategoryId { get; set; }
        public long DeletedByUserID { get; set; }
    }
}
