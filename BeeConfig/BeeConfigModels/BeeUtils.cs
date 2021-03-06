﻿using Microsoft.Extensions.Configuration;
using System;
using System.Text;

namespace BeeConfigModels
{
    public static class BeeUtils
    {
        /// <summary>
        /// 这里为了获取额外的配置，可以保留，用于区分AppSetting.json文件夹下的配置，一个是公用配置，一个是私有配置
        /// </summary>
        public static string SignSecret = AppSettings["SecretKey"];


        private static IConfigurationRoot config = null;

        public static IConfigurationRoot AppSettings
        {
            get
            {
                if (config == null)
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                        .AddJsonFile("BeeConfig.json");
                    config = builder.Build();
                }
                return config;
            }
        }

        public static string Get(string key)
        {
            return config[key];
        }
        /// <summary>
        /// 生成加密sign
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5(string str)
        {

            var md5 = System.Security.Cryptography.MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        /// <summary>
        /// 日期转换为时间戳（时间戳单位秒）
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ConvertToTimeStamp(DateTime time)
        {
            DateTime startTime= TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            return (int) (time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 时间戳转换为日期（时间戳单位秒）
        /// </summary>
        /// <param name="TimeStamp"></param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(long timeStamp)
        {
            DateTime dtStart = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1),TimeZoneInfo.Local);
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
    }
}
