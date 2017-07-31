using System;
using System.Reflection;
using System.Web.Http;

namespace NorthWind.WebApi.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("About")]
    public class AboutController : ApiController
    {
        static AboutController()
        {
            ApiVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static string ApiVersion { get; private set; }

        [HttpGet]
        [Route("Version")]
        public string GetVersion()
        {
            return ApiVersion;
        }

        [HttpGet]
        [Route("ServerName")]
        public string GetServerName()
        {
            return Environment.MachineName;
        }

        [HttpGet]
        [Route("Status")]
        public IHttpActionResult Status()
        {
            return Ok("OnLine");
        }
    }
}
