using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServIT.Data.DTO.User
{
    public class DeleteUserDTO
    {
        public Guid UserId { get; set; }
        public Guid DeletedByUserID { get; set; }
    }
}
