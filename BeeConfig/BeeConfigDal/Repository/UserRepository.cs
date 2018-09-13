using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeConfigDal.Entity;
using BeeConfigDal.Exntesion;
using BeeConfigModels;
using BeeConfigModels.Dto;
using BeeConfigModels.Filter;
using BeeConfigModels.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BeeConfigDal.Repository
{
    public class UserRepository : BaseRepository<User>
    {
        public async Task<bool> AddOrUpdate(ParamUserInfo param)
        {

        
            var entity = param.Id <= 0
                ? Context.Users.Attach(new User()).Entity
                : await Context.Users.SingleAsync(x => x.UserId == param.UserId &&x.Id==param.Id&& x.Status !=2);
            if (param.Id <= 0)
            {
                entity.Name = param.Name;
                entity.Pwd = BeeUtils.GetMD5(param.Pwd);
                entity.UserId = param.UserId;
                entity.CreateDate = DateTime.Now;
                entity.LastUpdate = DateTime.Now;
                Context.Users.Add(entity);
            }
            else
            {
                entity.Name = param.Name;
                entity.Status = param.Status;
                entity.LastUpdate = DateTime.Now;
                Context.Users.Update(entity);
            }


           
            return (await Context.SaveChangesAsync()) > 0;
        }


        public async Task<ResultPageData<RpsUser>> GetPageList(ParamPage<ParamUserSearchFilter> param)
        {

            var query = Context.Users.AsNoTracking().Where(x => x.Status !=2)
                .WhereIf(!string.IsNullOrEmpty(param.Filter.Name), x => x.Name.Contains(param.Filter.Name))
                .WhereIf(param.Filter.Status.HasValue,x=>x.Status==param.Filter.Status)
                .WhereIf(!string.IsNullOrEmpty(param.Filter.UserId), x => x.UserId.Contains(param.Filter.UserId));
            var list = await query
                .OrderBy(x => x.CreateDate.ToString("yyyy-MMMM-dd HH:mm:ss", CultureInfo.InvariantCulture))
                .Select(x=>new RpsUser()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UserId = x.UserId,
                    Status = x.Status,
                    CreateDate = x.CreateDate,
                    LastUpdate = x.LastUpdate
                }).
                GetPageList(param.Index, param.Size);
          return new ResultPageData<RpsUser>()
          {
              Data = list,
              Total =await query.CountAsync(),
              Pageindex = param.Index,
              Pagesize = param.Size

          };

        }



        public async Task<bool> ResetPassWord(ParamUsetRestPwdInfo param)
        {
            if (param.Id <= 0) return false;
            string sourcePassword = BeeUtils.GetMD5(param.SourcePassword);
            string resetPassword = BeeUtils.GetMD5(param.ResetPassword);
            var entity = await Context.Users.SingleAsync(x => x.UserId == param.UserId && x.Pwd == sourcePassword && x.Status ==0);
            entity.Pwd = resetPassword;
            entity.LastUpdate = DateTime.Now;
            Context.Users.Update(entity);
            return (await Context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> DeleteUser(ParamUserInfo param)
        {
            if (param.Id <= 0) return false;
            var entity =
                await Context.Users.SingleAsync(x => x.UserId == param.UserId && x.Id==param.Id && x.Status !=2);
            entity.Status = 2;
            return (await Context.SaveChangesAsync()) > 0;

        }

        public async Task<bool> IsUserExist(string userId)
        {
            return await Context.Users.AnyAsync(x => x.UserId == userId && x.Status == 0);
        }

        public async Task<RpsUser> GetUser(string userId, string pwd)
        {
            string passWord = BeeUtils.GetMD5(pwd);

            var entity = await Context.Users.SingleAsync(x => x.UserId == userId && x.Pwd == passWord && x.Status == 0);
            return new RpsUser()
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Name = entity.Name
            };

        }

        public async Task<ValidateUserDto> GetUser(int id)
        {
            var entity = await Context.Users.SingleAsync(x =>x.Id==id&&x.Status==0);
            return new ValidateUserDto()
            {
                Id = entity.Id,
                UserId = entity.UserId,
               PassWord = entity.Pwd
            };

        }
        public async Task<RpsUser> GetUserInfo(int id)
        {
            var entity = await Context.Users.SingleAsync(x => x.Id == id && x.Status == 0);
            return new RpsUser()
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Name = entity.Name
            };

        }
    }
}
