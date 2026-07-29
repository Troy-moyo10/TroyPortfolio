using Microsoft.AspNetCore.Mvc;
using TroyPortfolio.Models;
using TroyPortfolio.Services;

namespace TroyPortfolio.Controllers
{
    public class ContactController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<ContactController> _logger;

        public ContactController(IEmailService emailService, ILogger<ContactController> logger)
        {
            _emailService = emailService;
            _logger = logger;
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send contact form email");
                ModelState.AddModelError("", "Sorry, something went wrong while sending your message. Please try again.");
                return View(model);
            }
        }

        //public ContactController(IEmailService emailService)
        //{
        //    _emailService = emailService;
        //}

        [HttpGet]
        public IActionResult Index()
        {
            return View(new ContactFormModel());
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Index(ContactFormModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    try
        //    {
        //        await _emailService.SendEmailAsync(model);

        //        TempData["Success"] = "Your message has been sent successfully!";

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception)
        //    {
        //        ModelState.AddModelError("", "Sorry Something went wrong while sending your message. Please try again.");
        //        return View(model);
        //    }
        //}
    }
}