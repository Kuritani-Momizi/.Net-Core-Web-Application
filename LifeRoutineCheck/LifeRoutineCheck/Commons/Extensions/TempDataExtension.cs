using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Commons.Extensions
{
    public static class TempDataExtension
    {
        /// <summary>
        /// TempDataにデータを詰め込むJsonシリアライズ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tempData"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
            tempData.Peek(key);
        }

        /// <summary>
        /// TempDataからデータを取り出すJsonデシリアライズ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tempData"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            tempData.TryGetValue(key, out var obj);
            tempData.Peek(key);
            return obj == null ? default : JsonConvert.DeserializeObject<T>(obj.ToString());
        }

        /// <summary>
        /// TempDataに該当のデータがあるか確認
        /// </summary>
        /// <param name="tempData"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Any(this ITempDataDictionary tempData, string key)
        {
            tempData.TryGetValue(key, out var obj);

            return obj != null;
        }
    }
}
