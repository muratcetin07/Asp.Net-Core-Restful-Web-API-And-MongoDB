using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Abstract;
using Data.Core;
using Data.Repos;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace DotNetCoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IData<Product> _productDataRepo;

        public ProductController(IData<Product> productDataRepo)
        {
            _productDataRepo = productDataRepo;
        }

        [HttpGet]
        [Route("GetProductById/{id}")]
        public ActionResult<Product> GetProductById(string id)
        {
            return _productDataRepo.GetByKey(id);
        }

        [HttpPost]
        [Route("SaveProduct")]
        public ActionResult<DataResult<Product>> SaveProduct([FromBody] Product product)
        {
            return _productDataRepo.Insert(product);
        }


        [HttpDelete]
        [Route("DeleteProduct")]
        public ActionResult<DataResult<bool>> DeleteProduct([FromBody] Product product)
        {
            return _productDataRepo.Delete(s => s.Id == product.Id);
        }

    }
}
