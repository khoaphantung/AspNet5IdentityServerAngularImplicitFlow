﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace ResourceFileServer.Controllers
{
    using Microsoft.AspNet.Authorization;
    using Microsoft.Extensions.PlatformAbstractions;
    using Providers;
    [Authorize]
    [Route("api/[controller]")]
    public class FileExplorerController : Controller
    {
        private readonly IApplicationEnvironment _appEnvironment;
        private readonly ISecuredFileProvider _securedFileProvider;

        public FileExplorerController(ISecuredFileProvider securedFileProvider, IApplicationEnvironment appEnvironment)
        {
            _securedFileProvider = securedFileProvider;
            _appEnvironment = appEnvironment;
        }

        [Authorize("securedFilesUser")]
        [HttpGet]
        public IActionResult Get()
        {
            var adminClaim = User.Claims.FirstOrDefault(x => x.Type == "role" && x.Value == "securedFiles.admin");
            var files = _securedFileProvider.GetFilesForUser(adminClaim != null);

            return Ok(files);
        }
    }
}
