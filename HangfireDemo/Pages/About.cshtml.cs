using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace HangfireDemo.Pages
{
    public class AboutModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "Your application description page.";
        }

        public async Task<IActionResult> OnPostMailNowAsync()
        {
            string started = DateTime.Now.ToString();
            await MailAsync("direct", started);
            return Page();
        }

        public async Task<IActionResult> OnPostMailJobAsync()
        {
            string started = DateTime.Now.ToString();
            BackgroundJob.Enqueue(() => MailAsync("Enqueue", started));
            return Page();
        }

        public async Task<IActionResult> OnPostDelayJobAsync()
        {
            string started = DateTime.Now.ToString();
            BackgroundJob.Schedule(() => MailAsync("Schedule", started), TimeSpan.FromMinutes(1));
            return Page();
        }

        public async Task<IActionResult> OnPostScheduleJobAsync()
        {
            string started = DateTime.Now.ToString();
            RecurringJob.AddOrUpdate(() => MailAsync("Recurring", started), Cron.MinuteInterval(3));
            return Page();
        }

        public async Task<IActionResult> OnPostContinueJobAsync()
        {
            string started = DateTime.Now.ToString();
            var id = BackgroundJob.Enqueue(() => MailAsync("Enqueue with", started));
            BackgroundJob.ContinueWith(id, () => MailAsync("Continue", started), JobContinuationOptions.OnlyOnSucceededState);
            return Page();
        }

        public async Task MailAsync(string type, string Started)
        {
            Mailer m = new Mailer();
            await m.SendHelloMailAsync("hello@mail.org", "newuser@hangfire.net", $"Welcome to Hangfire Job {type}", $"I am so glad you are here at {Started}");
        }
    }
}
