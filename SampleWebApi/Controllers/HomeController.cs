﻿using Microsoft.AspNetCore.Mvc;

namespace SampleWebApi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}