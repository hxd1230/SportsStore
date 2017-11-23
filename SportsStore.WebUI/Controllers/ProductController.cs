using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        public int PageSize = 4;
        private IProductRepository productRepository;
        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        // GET: Product
        public ViewResult List(string category, int page = 1)
        {
            //return View(productRepository.Products.OrderBy(c => c.ProductID).Skip((page - 1) * PageSize).Take(PageSize));

            ProductListViewModel model = new ProductListViewModel
            {
                Products = productRepository.Products.Where(c => category == null || c.Category == category).OrderBy(c => c.ProductID).Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItermsPerPage = PageSize,
                    TotalItems = category == null ? productRepository.Products.Count() : productRepository.Products.Where(c => c.Category == category).Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }
    }
}