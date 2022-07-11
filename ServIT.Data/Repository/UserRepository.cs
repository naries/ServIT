using Microsoft.EntityFrameworkCore;
using ServIT.Business.GenericResponse;
using ServIT.Data.DTO.User;
using ServIT.Data.Repository.IRepository;
using ServIT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServIT.Data.Repository
{
    public class UserRepository 
    {
        private readonly ServITDBContext _db;
        public UserRepository(ServITDBContext db)
        {
            _db = db;   
        }
        public async Task<bool> CreateUser(User payload)
        {
            /*User user = new()
            {
                UserName = payload.UserName,
                Password = payload.Password
            };*/ //should go in an intermediate file.
            await _db.Users.AddAsync(payload);
            return await Save();
        }

        public async Task<bool> Update(User payload)
        {
            _db.Users.Update(payload);
            return await Save();
        }   

        public async Task<bool> Save() {
            return await _db.SaveChangesAsync() >= 0;
        }

        public bool IsUniqueUser(string userName)
        {
            var user = _db.Users.SingleOrDefault(x => x.UserName == userName);

            if (user == null)
                return false;

            return true;
        }

        public async Task<ICollection<User>> GetAllUsers()
        {
            ICollection<User> users = await _db.Users.Where(user => !user.IsDeactivated).OrderBy(user => user.UserName).ToListAsync();
            return users;
        }

        public async Task<User> GetUser(Guid id)
        {
            return await _db.Users.Where(user => user.Id == id && !user.IsDeactivated).SingleOrDefaultAsync();
        }
    }
}
