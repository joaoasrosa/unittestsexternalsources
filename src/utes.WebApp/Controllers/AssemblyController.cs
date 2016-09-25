﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using utes.Domain;
using utes.Interfaces;
using utes.WebApp.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace utes.WebApp.Controllers
{
    public class AssemblyController : Controller
    {
        private readonly IAssemblyStorage _assemblyStorage;
        private readonly ILogger<AssemblyController> _logger;

        public AssemblyController(IAssemblyStorage assemblyStorage, ILogger<AssemblyController> logger)
        {
            this._assemblyStorage = assemblyStorage;
            this._logger = logger;
        }

        // GET: /Assembly/
        [Route("Assembly")]
        public IActionResult Index()
        {
            try
            {
                return View(this._assemblyStorage.GetAssemblies());
            }
            catch (Exception ex)
            {
                this._logger.LogError(EventId.GenericException, ex, ex.Message);
                throw;
            }
        }

        // GET: /Assembly/Upload
        [Route("Assembly/Upload")]
        public IActionResult Upload()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                this._logger.LogError(EventId.GenericException, ex, ex.Message);
                throw;
            }
        }

        // POST: /Assembly/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Assembly/Upload")]
        public async Task<IActionResult> UploadAssembly()
        {
            try
            {
                var assemblyUploadResult = new AssemblyUpload();

                try
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
                            this._logger.LogInformation(EventId.SucessfullyAssemblyUpload,
                                string.Format("Assembly '{0}' successfully uploaded.", assembly.Name));
                        }
                        assemblyUploadResult.Success = true;
                        assemblyUploadResult.RedirectTo = "/Assembly";
                    }
                    else
                    {
                        this._logger.LogInformation(EventId.ExceptionAssemblyUpload, "No assembly uploaded");
                        assemblyUploadResult.Success = false;
                        assemblyUploadResult.ErrorHeading = "No assembly uploaded.";
                        assemblyUploadResult.ErrorMessage = "Please upload a valid assembly file.";
                    }

                }
                catch (DataSourceAttributeNotFound dataSourceAttributeNotFoundException)
                {
                    this._logger.LogInformation(EventId.ExceptionAssemblyUpload, dataSourceAttributeNotFoundException,
                        dataSourceAttributeNotFoundException.Message);
                    assemblyUploadResult.Success = false;
                    assemblyUploadResult.ErrorHeading = "The uploaded file is not valid.";
                    assemblyUploadResult.ErrorMessage = "Please upload a valid assembly file.";
                }
                catch (BadImageFormatException badImageFormatException)
                {
                    this._logger.LogInformation(EventId.ExceptionAssemblyUpload, badImageFormatException,
                        badImageFormatException.Message);
                    assemblyUploadResult.Success = false;
                    assemblyUploadResult.ErrorHeading = "The uploaded file is not valid.";
                    assemblyUploadResult.ErrorMessage = "Please upload a valid assembly file.";
                }

                return Json(assemblyUploadResult);

            }
            catch (Exception ex)
            {
                this._logger.LogError(EventId.GenericException, ex, ex.Message);
                throw;
            }
        }
    }
}