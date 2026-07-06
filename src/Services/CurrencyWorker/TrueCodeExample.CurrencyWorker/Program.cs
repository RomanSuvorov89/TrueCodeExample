using System.Text;
using TrueCodeExample.Common.Configuration;
using TrueCodeExample.CurrencyWorker;
using TrueCodeExample.Finance.Application;
using TrueCodeExample.Finance.DataAccess;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = Host.CreateApplicationBuilder(args);

builder.AddTrueCodeYamlConfiguration();

builder.Services.AddFinanceApplication();
builder.Services.AddFinanceDataAccess(builder.Configuration);
builder.Services.AddCurrencyWorker(builder.Configuration);

var host = builder.Build();
host.Run();
