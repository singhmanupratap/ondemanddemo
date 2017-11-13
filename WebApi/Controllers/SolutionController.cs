using Common.Interfaces;
using Common.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Mvc;
using Unity.Attributes;
namespace Common.Controllers
{
    public class SolutionController : Controller
    {
        public ISolutionBusinessLayer SolutionBusinessLayer { get; set; }
               
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Solution>> Index()
        {
            return await SolutionBusinessLayer.GetSolutions();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Solution> GetSolution(string id)
        {
            return await SolutionBusinessLayer.GetSolution(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> BuildSolution(string id)
        {
            return await SolutionBusinessLayer.BuildSolution(id);
        }
    }
}