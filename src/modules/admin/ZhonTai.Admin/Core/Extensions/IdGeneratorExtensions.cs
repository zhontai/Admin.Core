using FreeRedis;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yitter.IdGenerator;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Tools.Cache;

namespace ZhonTai.Admin.Core.Extensions;

public static class IdGeneratorExtensions
{
    private static bool _isSet = false;
    private static readonly object _locker = new();

    /// <summary>
    /// 添加Id生成器
    /// </summary>
    /// <param name="services"></param>
    public static void AddIdGenerator(this IServiceCollection services)
    {
        var idGeneratorConfig = AppInfo.GetOptions<AppConfig>().IdGenerator;

        if (_isSet)
            throw new InvalidOperationException("只允许添加一次Id生成器");

        lock (_locker)
        {
            if (_isSet)
                throw new InvalidOperationException("只允许添加一次Id生成器");

            Task.Delay(new Random().Next(10, 100)).Wait();

            SetIdGenerator(idGeneratorConfig);

            _isSet = true;
        }
    }

    private static void SetIdGenerator(IdGeneratorConfig idGeneratorConfig)
    {
        var redisProvider = AppInfo.GetRequiredService<IRedisClient>(false);
        using var lockController = redisProvider.Lock($"{idGeneratorConfig.CachePrefix}:lock", 5);

        if (lockController == null)
        {
            Task.Delay(new Random().Next(100, 1000)).Wait();
            SetIdGenerator(idGeneratorConfig);
        }

        try
        {
            var hostName = ":host:";
            var cache = AppInfo.GetRequiredService<ICacheTool>(false);
            var keys = cache.GetKeysByPattern($"{idGeneratorConfig.CachePrefix}{hostName}*");

            var maxWorkerId = (short)(Math.Pow(2.0, idGeneratorConfig.WorkerIdBitLength) - 1);
            var workerIdList = new List<ushort>();
            for (ushort i = 0; i < maxWorkerId; i++)
            {
                workerIdList.Add(i);
            }

            foreach (var key in keys)
            {
                var workerId = key[(key.LastIndexOf(':') + 1)..];
                workerIdList.Remove(Convert.ToUInt16(workerId));
            }

            var workerIdKey = string.Empty;
            foreach (var workerId in workerIdList)
            {
                workerIdKey = $"{idGeneratorConfig.CachePrefix}{hostName}{AppInfo.HostInfo.ShortName}:{workerId}";
                var exists = cache.Exists(workerIdKey);
                if (exists)
                {
                    workerIdKey = string.Empty;
                    continue;
                }

                Console.WriteLine($"{Environment.NewLine}自动注册的机器码 WorkerId = {workerId}");

                idGeneratorConfig.WorkerId = workerId;
                YitIdHelper.SetIdGenerator(idGeneratorConfig);

                cache.Set(workerIdKey, string.Empty, TimeSpan.FromSeconds(15));

                break;
            }

            if (workerIdKey.IsNull())
            {
                throw new Exception("自动注册机器码WorkerId已全被占用，请增加机器码位长WorkerIdBitLength后再重新启动");
            }

            //每隔 10 秒刷新 WorkerId 占用有效期
            Task.Run(() =>
            {
                while (true)
                {
                    redisProvider.Expire(workerIdKey, TimeSpan.FromSeconds(15));
                    Task.Delay(10000).Wait();
                }
            });
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
        finally
        {
            lockController.Unlock();
        }
    }
}
