using BusinessLayer;
using Common.Models;
using Interfaces;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.Controllers
{
    [RoutePrefix("api/solutions")]
    public class SolutionController : ApiController
    {
        public ISolutionBusinessLayer SolutionBusinessLayer { get; set; }

        public SolutionController()
        {
            SolutionBusinessLayer = new SolutionBusinessLayer();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var result = await SolutionBusinessLayer.GetSolutionsAsync();

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> Get(string id)
        {
            var solution = await SolutionBusinessLayer.GetSolutionAsync(id);
            return Ok(solution);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="solution"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(Solution solution)
        {
            var result = await SolutionBusinessLayer.AddSolutionAsync(solution);
            return Ok(content: result);
        }

        
    }
}