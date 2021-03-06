﻿using System;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;
using System.Net;
using Newtonsoft.Json;
using BeeConfigClientStandardNF4.Common;

namespace BeeConfigClientStandardNF4
{
    public static class BeeConfigManager
    {
        private static Timer _Timer;

        private static object _TimerObject;

        private static string _CacheFilePath;

        private static string _BeeApiUrl;

        private static string _BeeConfigAppId;

        private static string _BeeConfigEnv;

        private static string _BeeSecret;

        private static ConcurrentDictionary<string, string> _BeeConfigKeyValues = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 初始化配置管理 
        /// 指定默认存储文件
        /// 指定远程访问地址
        /// </summary>
        /// <param name="beeApiUrl"></param>
        /// <param name="cacheFilePath">完整地址，默认当前目录下的 BeeConfig.json 文件</param>         
        /// <param name="period">默认每分钟获取一次最新配置</param>
        public static void Init(string beeApiUrl,string beeAppId,string beeEnv,string beeSecret, string cacheFilePath= "BeeConfig.json",int period=60000)
        {
            if (string.IsNullOrWhiteSpace(beeApiUrl))
            {
                throw new ArgumentNullException("beeApiUrl");
            }
            if (string.IsNullOrWhiteSpace(beeAppId))
            {
                throw new ArgumentNullException("beeAppId");
            }
            if (string.IsNullOrWhiteSpace(beeEnv))
            {
                throw new ArgumentNullException("beeEnv");
            }
            if (string.IsNullOrWhiteSpace(beeSecret))
            {
                throw new ArgumentNullException("beeSecret");
            }

            var tempUrl = beeApiUrl.ToLower();
            if (!tempUrl.StartsWith("http://") && !tempUrl.StartsWith("https://"))
            {
                throw new ArgumentException("不是有效的URL格式", "beeApiUrl");
            }

            _BeeConfigEnv = beeEnv;
            _BeeConfigAppId = beeAppId;
            _BeeApiUrl = beeApiUrl;
            _BeeSecret = beeSecret;
            _TimerObject = new object();
            _CacheFilePath = cacheFilePath;
            //立即启动 获取一次配置
            Refresh(_TimerObject);
            //延时一分钟，然后每隔一分钟执行一次
            _Timer = new Timer(Refresh, _TimerObject, 60 * 1000, period);
        }
         
        /// <summary>
        /// 每分钟执行一次刷新配置信息
        /// </summary>
        /// <param name="obj"></param>
        private static void Refresh(object obj)
        {
            var cacheContent = string.Empty;
            if (!File.Exists(_CacheFilePath))
            {
                using (File.Create(_CacheFilePath)) { }
            }
            else
            {
                using(var reader=new StreamReader(_CacheFilePath, System.Text.Encoding.UTF8))
                {
                    cacheContent = reader.ReadToEnd();
                }
            }
            DtoBeeConfig localConfig = new DtoBeeConfig
            {
                BeeConfigAppId = _BeeConfigAppId,
                BeeConfigEnvironment = _BeeConfigEnv,
                BeeConfigLastUpdate = 0
            };
            if (!string.IsNullOrWhiteSpace(cacheContent))
            {
                localConfig = JsonConvert.DeserializeObject<DtoBeeConfig>(cacheContent);
                _BeeConfigKeyValues = JsonConvert.DeserializeObject<ConcurrentDictionary<string, string>>(JsonConvert.SerializeObject(localConfig.BeeConfigData));
            }

            var tempCurrent = DateTime.Now.ToUniversalTime().ToString("yyyyMMddhhmmss");
            var sign = BeeUtils.GetMD5($"{localConfig.BeeConfigAppId}_{localConfig.BeeConfigEnvironment}_{localConfig.BeeConfigLastUpdate}_{_BeeSecret}_{tempCurrent}");

            var tempApiUrl = $"{_BeeApiUrl}?appid={localConfig.BeeConfigAppId}&env={localConfig.BeeConfigEnvironment}&lastupdate={localConfig.BeeConfigLastUpdate}&sign={sign}&current={tempCurrent}";

            try
            {
            
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(tempApiUrl);
                    request.Timeout = 5000;
                    request.BeginGetResponse(new AsyncCallback(FinishWebRequest), new AsyncRequestState(){ Request=request,Config=localConfig });
            }
            catch
            {
                //不处理异常
            }
        }

        /// <summary>
        /// 获取配置,如果值空，标识获取失败
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetBeeConfig(string key)
        {
            var value = string.Empty;
            if(!_BeeConfigKeyValues.TryGetValue(key,out value))
            {
                value = string.Empty;
            }
            return value;
        }

        private static void FinishWebRequest(IAsyncResult result)
        {
            AsyncRequestState requestState = (result.AsyncState as AsyncRequestState);

            var updateContent = string.Empty;
            HttpWebResponse response = requestState.Request.EndGetResponse(result) as HttpWebResponse;
          
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                using (Stream stream= response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        updateContent = reader.ReadToEnd();
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(updateContent))
            {
                var update = JsonConvert.DeserializeObject<DtoBeeConfig>(updateContent);
                if (update.BeeConfigLastUpdate > requestState.Config.BeeConfigLastUpdate)
                {
                    using (var writer = new StreamWriter(_CacheFilePath, false, System.Text.Encoding.UTF8))
                    {
                        writer.WriteLine(updateContent);
                    }
                    _BeeConfigKeyValues = JsonConvert.DeserializeObject<ConcurrentDictionary<string, string>>(JsonConvert.SerializeObject(update.BeeConfigData));
                }
            }
        }

        private class  AsyncRequestState
        {
          public  HttpWebRequest Request { get; set; }
          public  DtoBeeConfig Config { get; set; }
        }
    }
}
