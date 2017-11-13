using BusinessLayer;
using Common.Models;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.Controllers
{
    [RoutePrefix("api/builds")]
    public class BuildController : ApiController
    {

        public ISolutionBusinessLayer SolutionBusinessLayer { get; set; }

        public BuildController()
        {
            SolutionBusinessLayer = new SolutionBusinessLayer();
        }

        // GET api/<controller>
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
           var result = await SolutionBusinessLayer.GetBuildsAsync();

            return Ok(result);
        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> Get(string id)
        {
            Build result = await SolutionBusinessLayer.GetBuildAsync(id);

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Build([FromBody]Build build)
        {
            var result = await SolutionBusinessLayer.BuildSolutionAsync(build);
            return Ok(result);
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<IHttpActionResult> Update([FromBody]Build build, string id)
        {
            var result = await SolutionBusinessLayer.UpdateBuildAsync(build, id);
            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="solution"></param>
        /// <returns></returns>
        /// 
        [HttpPut]
        [Route("complete")]
        public async Task<IHttpActionResult> Complete([FromBody]Build build, string buildId)
        {
            var result = await SolutionBusinessLayer.CompleteBuildAsync(build, buildId);
            return Ok(result);
        }

    }
}