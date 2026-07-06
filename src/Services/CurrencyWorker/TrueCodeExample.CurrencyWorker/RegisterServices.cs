using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using Refit;
using TrueCodeExample.CurrencyWorker.Cbr;
using TrueCodeExample.CurrencyWorker.Jobs;
using TrueCodeExample.CurrencyWorker.Options;

namespace TrueCodeExample.CurrencyWorker;

public static class RegisterServices
{
    public static IServiceCollection AddCurrencyWorker(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CurrencyWorkerOptions>(configuration.GetSection(CurrencyWorkerOptions.SectionName));

        services.AddRefitClient<ICbrApi>()
            .ConfigureHttpClient((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<CurrencyWorkerOptions>>().Value;
                client.BaseAddress = new Uri(options.CbrBaseUrl);
            });

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
