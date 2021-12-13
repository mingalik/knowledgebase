using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentWebApi.Controllers
{
    public class DocumentValueControllerV2 : Controller
    {
        private readonly IMediator mediator;

        public DocumentValueControllerV2(IMediator mediator) => mediator = mediator;

        // POST: DocumentControllerV2/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                await mediator.Send(, HttpContext.RequestAborted);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
