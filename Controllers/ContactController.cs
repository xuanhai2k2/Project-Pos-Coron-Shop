﻿using Microsoft.AspNetCore.Mvc;

namespace devpro_project.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
