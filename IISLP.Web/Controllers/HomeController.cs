using IISLP.Core.Services;
using IISLP.Web.Extensions;
using IISLP.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IISLP.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(UploadFilesVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.Files != null && model.Files.Any())
                {
                    var resultVm = new ResultVm() { Logs = new List<ResultLogVm>() };

                    var analyzer = new Analyzer();
                    try
                    {
                        foreach (var file in model.Files)
                        {
                            try
                            {
                                var res = await analyzer.AnalyzeLog(file.OpenReadStream(), model.Format);

                                resultVm.Logs.Add(new ResultLogVm
                                {
                                    File = file.FileName,
                                    Clients = res.Select(r => new ResultClientVm
                                    {
                                        FQDN = r.FQDN,
                                        IP = r.IP.ToString(),
                                        RequestCount = r.RequestCount
                                    })
                                });
                            }
                            catch (Exception e)
                            {
                                throw new Exception($"Failed to analyze file {file.FileName}.", e);
                            }
                        }

                        TempData.Put("result", resultVm);
                        return this.RedirectToAction(nameof(Result));
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("Files", e.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No files provided");
                }
            }

            return View(model);
        }

        public ActionResult Result()
        {
            var model = TempData.Get<ResultVm>("result");

            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}