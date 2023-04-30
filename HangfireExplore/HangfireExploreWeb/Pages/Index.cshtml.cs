using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HangfireExploreWeb.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        BackgroundJob.Enqueue(() => new SomeStupidJob());
    }
}

public class SomeStupidJob
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int RunTye { get; set; } = 1;
    public bool RunOption1 { get; set; } = false;

}