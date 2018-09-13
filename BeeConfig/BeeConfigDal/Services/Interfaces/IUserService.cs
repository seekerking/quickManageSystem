using System.Threading.Tasks;
using BeeConfigModels.Dto;
using BeeConfigModels.Filter;
using BeeConfigModels.ViewModels;

namespace BeeConfigDal.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> AddOrUpdate(ParamUserInfo param);

        Task<ResultPageData<RpsUser>> GetPageList(ParamPage<ParamUserSearchFilter> param);
        Task<bool> ResetPassWord(ParamUsetRestPwdInfo param);


        Task<bool> DeleteUser(ParamUserInfo param);


        Task<bool> IsUserExist(string userId);


        Task<RpsUser> GetUser(string userId, string pwd);

        Task<ValidateUserDto> GetUser(int id);

        Task<RpsUser> GetUserInfo(int id);


    }
}