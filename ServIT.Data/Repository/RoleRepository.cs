using ServIT.Business.GenericResponse;
using ServIT.Data.Repository.IRepository;
using ServIT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServIT.Data.Repository
{
    public class RoleRepository
    {

        private readonly ServITDBContext _db;
        public RoleRepository(ServITDBContext db)
        {
            _db = db;
        }
    }
}
