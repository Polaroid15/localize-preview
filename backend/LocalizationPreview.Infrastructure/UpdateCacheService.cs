using LocalizationPreview.Core.Dto;
using LocalizationPreview.Core.Interfaces;
using LocalizationPreview.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LocalizationPreview.Infrastructure;

public class UpdateCacheService : BackgroundService 
{
    private readonly ILogger<UpdateCacheService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private CancellationTokenSource _wakeupCts = new();

    public UpdateCacheService(ILogger<UpdateCacheService> logger, IServiceScopeFactory scopeFactory) 
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public void UpdateOutstandingCacheItems() => _wakeupCts.Cancel();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) 
    {
        while (!stoppingToken.IsCancellationRequested) 
        {
            await UpdateOutstandingCacheItems(stoppingToken);
        }
    }

    private async Task UpdateOutstandingCacheItems(CancellationToken stoppingToken) 
    {
        try 
        {
            while (!stoppingToken.IsCancellationRequested) 
            {
                using var scope = _scopeFactory.CreateScope();
                var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
                var localizationService = scope.ServiceProvider.GetRequiredService<ILocalizationService>();
                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                var outboxItems = await outboxRepository.GetAllAsync(nameof(TranslationServiceDto));
                foreach (var item in outboxItems) {
                    var translationModel = JsonConvert.DeserializeObject<TranslationServiceDto>(item.Data);
                    if (translationModel == null) {
                        continue;
                    }

                    var localization = new Localization() 
                    {
                        EntityId = translationModel.EntityId,
                        EntityName = translationModel.EntityName,
                        LanguageCode = translationModel.LanguageCode,
                        TranslationFields = translationModel.TranslationFields
                    };
                    await localizationService.SetAsync(localization);
                    await outboxRepository.SetDeletedAsync(item.Id);
                }

                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_wakeupCts.Token, stoppingToken);
                try 
                {
                    var time = config.GetSection("SyncTimeInSeconds").Value;
                    await Task.Delay(TimeSpan.FromSeconds(Convert.ToDouble(time)), linkedCts.Token);
                }
                catch (OperationCanceledException) 
                {
                    if (_wakeupCts.Token.IsCancellationRequested) 
                    {
                        var tmp = _wakeupCts;
                        _wakeupCts = new CancellationTokenSource();
                        tmp.Dispose();
                    }
                }
                catch (AggregateException ae) 
                {
                    foreach (var inner in ae.InnerExceptions) 
                    {
                        _logger.LogError(inner.Message + " " + inner.Source);
                    }
                }
            }
        }
        catch (Exception e) 
        {
            _logger.LogError(e.Message + " " + e.Source);
        }
    }
}