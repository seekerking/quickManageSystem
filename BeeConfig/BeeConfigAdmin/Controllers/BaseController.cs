using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeeConfigModels.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BeeConfigAdmin.Controllers
{
    [Route("api/[controller]")]
    public class BaseController : Controller
    {

        public async Task<IActionResult> ActionWrapAsync(Func<Task<ResultBase>> func)
        {
            ResultBase result = new ResultBase();
            try
            {
                if (!ModelState.IsValid)
                {
                    var error = string.Empty;
                    ModelState.Values.AsEnumerable().ToList().ForEach(it => {
                        if (it.Errors.Count > 0)
                        {
                            error += it.Errors[0].ErrorMessage;
                        }
                    });
                    throw new ArgumentException(error);
                }

                result =await func();
                result.Status = 0;
                result.Msg = string.Empty;
            }
            catch (Exception ex)
            {
                result.Status = 1;
                result.Msg = ex.Message;
            }
            return Ok(result);
        }

        public  IActionResult ActionWrap(Func<ResultBase> func)
        {
            ResultBase result = new ResultBase();
            try
            {
                if (!ModelState.IsValid)
                {
                    var error = string.Empty;
                    ModelState.Values.AsEnumerable().ToList().ForEach(it => {
                        if (it.Errors.Count > 0)
                        {
                            error += it.Errors[0].ErrorMessage;
                        }
                    });
                    throw new ArgumentException(error);
                }

                result =  func();
                result.Status = 0;
                result.Msg = string.Empty;
            }
            catch (Exception ex)
            {
                result.Status = 1;
                result.Msg = ex.Message;
            }
            return Ok(result);
        }
    }
}
