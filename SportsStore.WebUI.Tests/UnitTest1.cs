using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SportsStore.WebUI.Extensions;
namespace SportsStore.WebUI.Tests
{
    [TestClass]
    public class UnitTest1
    {
        public Mock IProductRepository { get; private set; }

        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(c => c.Products).Returns(new Product[]
            {
                new Product{Name="P1",ProductID=1 },
                new Product{Name="P2",ProductID=2 },
                new Product{Name="P3",ProductID=3 },
                new Product{Name="P4",ProductID=4 },
                new Product{Name="P5",ProductID=5 }
            }.AsQueryable());
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            ProductListViewModel result = (ProductListViewModel)controller.List(null, 2).Model;
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                ItermsPerPage = 4,
                TotalItems = 10
            };
            Func<int, string> pageUrlDelegate = i => "Page" + i;
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);
            Assert.AreEqual(result.ToString(), @"<a href=""Page1"">1</a>" + @"<a class=""selected"" href=""Page2"">2</a>" + @"<a href=""Page3"">3</a>");
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(c => c.Products).Returns(new Product[]
            {
                new Product{Name="P1",ProductID=1 },
                new Product{Name="P2",ProductID=2 },
                new Product{Name="P3",ProductID=3 },
                new Product{Name="P4",ProductID=4 },
                new Product{Name="P5",ProductID=5 }
            }.AsQueryable());
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            ProductListViewModel result = (ProductListViewModel)controller.List(null, 2).Model;
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItermsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            Mock<IProductRepository> mock = new Mock<Domain.Abstract.IProductRepository>();
            mock.Setup(c => c.Products).Returns(new Product[]
            {
                new Product{Name="P1",ProductID=1,Category="Cat1" },
                new Product{Name="P2",ProductID=2,Category="Cat2" },
                new Product{Name="P3",ProductID=3,Category="Cat1"},
                new Product{Name="P4",ProductID=4,Category="Cat2"},
                new Product{Name="P5",ProductID=5,Category="Cat3" }
                }.AsQueryable()
                );
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            Product[] result = ((ProductListViewModel)controller.List("Cat2", 1).Model).Products.ToArray();
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[0].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            Mock<IProductRepository> mock = new Mock<Domain.Abstract.IProductRepository>();
            mock.Setup(c => c.Products).Returns(new Product[]
            {
                new Product{Name="P1",ProductID=1,Category="Apples" },
                new Product{Name="P2",ProductID=2,Category="Apples" },
                new Product{Name="P3",ProductID=3,Category="Plums"},
                new Product{Name="P4",ProductID=4,Category="Oranges"}
                }.AsQueryable()
                );
            NavController target = new NavController(mock.Object);
            string[] results = ((IEnumerable<string>)target.Menu().Model).ToArray();
            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "Apples");
            Assert.AreEqual(results[1], "Oranges");
            Assert.AreEqual(results[2], "Plums");
        }
        [TestMethod]
        public void Indicates_Selected_Category()
        {
            Mock<IProductRepository> mock = new Mock<Domain.Abstract.IProductRepository>();
            mock.Setup(c => c.Products).Returns(new Product[]
            {
                new Product{Name="P1",ProductID=1,Category="Apples" },
                new Product{Name="P2",ProductID=4,Category="Oranges"}
                }.AsQueryable()
                );
            NavController target = new NavController(mock.Object);
            string categoryToSelect = "Apples";
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;
            Assert.AreEqual(categoryToSelect, result);
        }
        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            Mock<IProductRepository> mock = new Mock<Domain.Abstract.IProductRepository>();
            mock.Setup(c => c.Products).Returns(new Product[]
            {
               new Product{Name="P1",ProductID=1,Category="Cat1" },
                new Product{Name="P2",ProductID=2,Category="Cat2" },
                new Product{Name="P3",ProductID=3,Category="Cat1"},
                new Product{Name="P4",ProductID=4,Category="Cat2"},
                new Product{Name="P5",ProductID=5,Category="Cat3" }
                }.AsQueryable()
                );
            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;
            int res1 = ((ProductListViewModel)target.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((ProductListViewModel)target.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((ProductListViewModel)target.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((ProductListViewModel)target.List(null).Model).PagingInfo.TotalItems;
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}
