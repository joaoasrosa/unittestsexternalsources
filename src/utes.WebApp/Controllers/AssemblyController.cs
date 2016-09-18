using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using utes.Domain;
using utes.Interfaces;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace utes.WebApp.Controllers
{
    public class AssemblyController : Controller
    {
        private readonly IAssemblyStorage _assemblyStorage;

        public AssemblyController(IAssemblyStorage assemblyStorage)
        {
            this._assemblyStorage = assemblyStorage;
        }

        // GET: /Assembly/
        [Route("Assembly")]
        public IActionResult Index()
        {
            return View(this._assemblyStorage.GetAssemblies());
        }

        // GET: /Assembly/Upload
        [Route("Assembly/Upload")]
        public IActionResult Upload()
        {
            return View();
        }

        // POST: /Assembly/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Assembly/Upload")]
        public async Task<IActionResult> UploadAssembly()
        {
            if (this.Request.Form.Files.Any())
            {
                var assemblyFile = this.Request.Form.Files.GetFile("assemblyFile");
                using (var memoryStream = new MemoryStream())
                {
                    await assemblyFile.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    var assembly = new Assembly
                    {
                        Name = Path.GetFileName(assemblyFile.FileName),
                        ContentBytes = memoryStream.ToArray()
                    };
                    this._assemblyStorage.SaveAssembly(assembly);
                }
                return Json(new { sucess = true, redirectTo = "/Assembly" });
            }

            // TODO if not happy path return more info to the view...
            return Json("Ups");
        }
    }
}
