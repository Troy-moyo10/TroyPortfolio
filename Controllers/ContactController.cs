using Microsoft.AspNetCore.Mvc;
using TroyPortfolio.Models;
using TroyPortfolio.Services;

namespace TroyPortfolio.Controllers
{
    public class ContactController : Controller
    {
        private readonly IEmailService _emailService;

        public ContactController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new ContactFormModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactFormModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _emailService.SendEmailAsync(model);

                TempData["Success"] = "Your message has been sent successfully!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Sorry Something went wrong while sending your message. Please try again.");

                return View(model);
            }
        }
    }
}