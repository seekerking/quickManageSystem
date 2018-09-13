using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BeeConfigDal.Repository;
using BeeConfigDal.Services.Interfaces;
using BeeConfigModels.Dto;
using BeeConfigModels.Filter;
using BeeConfigModels.ViewModels;

namespace BeeConfigDal.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository=new UserRepository();
        public async Task<bool> AddOrUpdate(ParamUserInfo param)
        {
            return await _userRepository.AddOrUpdate(param);
        }

        public async Task<ResultPageData<RpsUser>> GetPageList(ParamPage<ParamUserSearchFilter> param)
        {
            return await _userRepository.GetPageList(param);
        }


        public async Task<bool> DeleteUser(ParamUserInfo param)
        {
            return await _userRepository.DeleteUser(param);
        }

        public async Task<RpsUser> GetUser(string userId, string pwd)
        {
            return await _userRepository.GetUser(userId, pwd);
        }

        public async Task<ValidateUserDto> GetUser(int id)
        {
            return await _userRepository.GetUser(id);
        }

        public async Task<bool> IsUserExist(string userId)
        {
            return await _userRepository.IsUserExist(userId);
        }

        public async Task<bool> ResetPassWord(ParamUsetRestPwdInfo param)
        {
            return await _userRepository.ResetPassWord(param);
        }

        public async Task<RpsUser> GetUserInfo(int id)
        {
            return await _userRepository.GetUserInfo(id);
        }
    }
}
