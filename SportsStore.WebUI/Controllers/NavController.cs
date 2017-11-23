using SportsStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IProductRepository productRepository;
        public NavController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        // GET: Nav
        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories = productRepository.Products.Select(c => c.Category).Distinct().OrderBy(c => c);
            return PartialView(categories);
        }
    }
}