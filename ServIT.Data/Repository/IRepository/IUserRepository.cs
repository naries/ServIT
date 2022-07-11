using ServIT.Business.GenericResponse;
using ServIT.Data.DTO.User;
using ServIT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ServIT.Data.DTO.User.UserDTO;

namespace ServIT.Data.Repository.IRepository
{
    internal interface IUserRepository
    {
        Task<ICollection<User>> GetAllUsers();
        Task<User> GetUser(Guid id);
        bool IsUniqueUser(string userName);
        Task<bool> CreateUser(User payload);
        Task<bool> Save();
        Task<bool> Update(User payload);
        //Task<BaseResponse> DeactivateUser(Guid id);

    }
}
