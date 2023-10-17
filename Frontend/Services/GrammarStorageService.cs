using Blazored.LocalStorage;
using Katheryne.Abstractions;

namespace Frontend.Services;

public class GrammarStorageService
{
    private const string GrammarTextKey = "GrammarText";
    
    private readonly ILocalStorageService _localStorage;
    private readonly ILogger<GrammarStorageService> _logger;
    private readonly IChatRobotFactory _robotFactory;

    public GrammarStorageService(ILocalStorageService localStorage,
        ILogger<GrammarStorageService> logger,
        IChatRobotFactory robotFactory)
    {
        _localStorage = localStorage;
        _logger = logger;
        _robotFactory = robotFactory;
    }

    /// <summary>
    /// 尝试从LocalStorage中恢复之前设置的文法
    /// </summary>
    /// <returns>恢复文法是否成功</returns>
    public async Task<bool> RestoreGrammar()
    {
        _logger.LogDebug("Try to restore grammar text.");

        string result = await _localStorage.GetItemAsync<string>(GrammarTextKey);

        if (result == default)
        {
            return false;
        }
        
        _logger.LogDebug("Restore grammar text successfully.");
        _robotFactory.SetGrammar(result);
        return true;
    }

    /// <summary>
    /// 保存当前设置使用的文法
    /// </summary>
    public async Task StoreGrammar()
    {
        if (!string.IsNullOrEmpty(_robotFactory.GrammarText))
        {
            _logger.LogDebug("Store current grammar text.");
            await _localStorage.SetItemAsync(GrammarTextKey, _robotFactory.GrammarText);
        }
    }

    /// <summary>
    /// 清除当前设置的文法
    /// </summary>
    public async Task RemoveGrammar()
    {
        await _localStorage.RemoveItemAsync(GrammarTextKey);
    }
}