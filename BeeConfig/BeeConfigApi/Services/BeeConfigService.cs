using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using BeeConfigApi.Models;

namespace BeeConfigApi.Services
{
    public class BeeConfigService
    {
        private const string cacheKey = "Bee_Config_AllCache";
        private const string cacheAppDataKey = "Bee_Config_AppCache";

        private readonly IConfiguration configuration;
        private readonly IMemoryCache memoryCache;

        public BeeConfigService(IConfiguration configuration, IMemoryCache memoryCache)
        {
            this.configuration = configuration;
            this.memoryCache = memoryCache;
        }

        protected IDbConnection GetCon()
        {
            return new SqlConnection(configuration["ConnectionString"]);
        }

        public async Task<int> SaveRequest(EntityReq entityReq)
        {
            const string strSql =
                @" if exists(select Id from ReqLogsTB where [AppEnv]=@AppEnv and [AppId]=@AppId and [ClientIp]=@ClientIp)
                  begin
                    update [ReqLogsTB] set [LastDate] = @LastDate,[LastConfigDate]=@LastConfigDate,
                                        [ReqTimes]=[ReqTimes]+1 
                    where [AppEnv]=@AppEnv and [AppId] = @AppId and [ClientIp]=@ClientIp
                  end
            else
                  begin
                    insert [ReqLogsTB] values(@ClientIp, @AppId, @AppEnv, @LastDate, @LastDate,1,@LastConfigDate)
                  end";
            try
            {
                using (var con = GetCon())
                {
                    return await con.ExecuteAsync(strSql, entityReq);
                }
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// 获取缓存数据里面所有的配置，如果没有缓存，则获取的是数据库里面的全部数据
        /// </summary>
        /// <returns></returns>

        public async Task<IList<EntityPublish>> GetAllPublishs()
        {
            IList<EntityPublish> publishes;
            if (!memoryCache.TryGetValue(cacheKey, out publishes))
            {
                using (var con = GetCon())
                {
                    publishes = await GetDataFromDB();
                    memoryCache.Set(cacheKey, publishes,
                        new MemoryCacheEntryOptions {Priority = CacheItemPriority.NeverRemove});
                }
            }
            return publishes;
        }

        public async Task<string> GetAppSign(string appId)
        {
            IList<EntityApp> apps;
            if (!memoryCache.TryGetValue(cacheAppDataKey, out apps))
            {
                using (var con = GetCon())
                {
                    apps = await GetAppDataFromDB();
                    memoryCache.Set(cacheAppDataKey, apps,
                        new MemoryCacheEntryOptions { Priority = CacheItemPriority.NeverRemove });
                }
            }
            var query = from item in apps
                        where item.AppId == appId
                        select item;
            if (query.Count() == 1)
                return query.FirstOrDefault().Secret.ToString().ToUpper();
            return string.Empty;
        }

        /// <summary>
        /// 获取数据库的配置数据
        /// </summary>
        /// <param name="refresh">true:全部获取数据，false:只获取最新更新的1小时数据</param>
        /// <returns></returns>
        public async Task<IList<EntityPublish>> GetDataFromDB()
        {
            string strSql = @"
                            Select  [PublishTB].* from [PublishTB]
                            inner join 
                            (SELECT 
                                  [AppId]
                                  ,[EnvId]
                                  ,max([PublishTimeSpan]) [PublishTimeSpan]
                              FROM [beeconfigdb].[dbo].[PublishTB] group by AppId,EnvId) [temp] 
                              on [temp].AppId= [PublishTB].AppId 
                              and [temp].EnvId= [PublishTB].EnvId 
                              and [temp].PublishTimeSpan= [PublishTB].PublishTimeSpan";
            using (var con = GetCon())
            {
                return (IList<EntityPublish>)(await con.QueryAsync<EntityPublish>(strSql));
            }
        }

        /// <summary>
        /// 更新缓存
        /// 包括发布信息 以及 所有app信息
        /// </summary>
        /// <returns></returns>
        public async Task UpdateCache()
        {
            try
            {
                var configs = await GetDataFromDB();
                if (configs != null && configs.Any())
                {
                    memoryCache.Set(cacheKey, configs,
                        new MemoryCacheEntryOptions {Priority = CacheItemPriority.NeverRemove});
                }

                var apps = await GetAppDataFromDB();
                if(apps!=null && apps.Any())
                {
                    memoryCache.Set(cacheAppDataKey, apps, new MemoryCacheEntryOptions { Priority = CacheItemPriority.NeverRemove });
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 获取所有app信息
        /// </summary>
        /// <returns></returns>
        public async Task<IList<EntityApp>> GetAppDataFromDB()
        {
            const string strSql = @"
                            SELECT [Id]
                              ,[AppId] 
                              ,[Secret] 
                          FROM [AppTB] Where [Status]=0";
            using (var con = GetCon())
            {
                return (IList<EntityApp>)(await con.QueryAsync<EntityApp>(strSql));
            }
        }
    }
}