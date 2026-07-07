using Microsoft.Extensions.Options;
using Quartz;
using TrueCodeExample.Finance.CurrencyWorker.Jobs;
using TrueCodeExample.Finance.CurrencyWorker.Options;

namespace TrueCodeExample.Finance.CurrencyWorker;

public static class RegisterServices
{
    public static IServiceCollection AddCurrencyWorker(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CurrencyWorkerOptions>(configuration.GetSection(CurrencyWorkerOptions.SectionName));

        var workerOptions = configuration.GetSection(CurrencyWorkerOptions.SectionName).Get<CurrencyWorkerOptions>()
            ?? new CurrencyWorkerOptions();

        services.AddQuartz(quartz =>
        {
            var jobKey = JobKey.Create(nameof(CurrencySyncJob));

            quartz.AddJob<CurrencySyncJob>(jobKey);
            quartz.AddTrigger(trigger => trigger
                .ForJob(jobKey)
                .WithIdentity($"{nameof(CurrencySyncJob)}-trigger")
                .StartNow()
                .WithSimpleSchedule(schedule => schedule
                    .WithIntervalInMinutes(workerOptions.SyncIntervalMinutes)
                    .RepeatForever()));
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        return services;
    }
}
