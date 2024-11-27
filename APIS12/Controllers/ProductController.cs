using APIS12.Models;
using APIS12.Request;
using APIS12.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIS12.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        
        private readonly InvoiceContext _invoiceContext;

        public ProductController(InvoiceContext invoiceContext)
        {
            _invoiceContext = invoiceContext;
        }

        
        [HttpGet]
        public List<Product> getAllProduct()
        {
            return _invoiceContext.Products.Where(x => x.Active == true).ToList();
        }

        [HttpPost]
        public void InsertProduct(ProductRequestV2 request)
        {
            Product product1 = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Active = true
            };

            _invoiceContext.Products.Add(product1);
            _invoiceContext.SaveChanges();
        }

      
        [HttpPost]
        public void DeleteProduct(ProductoRequestD request)
        {
            _invoiceContext.Products.Find(request.ProductID).Active = false;
            _invoiceContext.SaveChanges();
        }

        [HttpPut]
        public void UpdatePriceProduct(RequestUdpatePrecioPro request)
        {
            Product product = _invoiceContext.Products.Where(x => x.ProductID == request.ProductID).FirstOrDefault();

            product.Price = request.Price;

            _invoiceContext.Entry(product).State = EntityState.Modified;
            _invoiceContext.SaveChanges();
        }

        [HttpPost]
        public void DeleteListProduct(ProductListDelete request)
        {
            var productID = request.Products.Select(x => x.ProductID).ToList();

            var productsDelete = _invoiceContext.Products.Where(p => 
                                 productID.Contains(p.ProductID)).ToList();

            productsDelete.ForEach(p => p.Active = false);
            _invoiceContext.SaveChanges();
        }

        [HttpGet]
        public List<Product> getByName(String name)
        {
            return _invoiceContext.Products.Where(x => x.Name == name).ToList();
        }

      
        [HttpGet]
        public Product getById(int id)
        {
            return _invoiceContext.Products.Where(x => x.ProductID == id).FirstOrDefault();
        }


        [HttpGet]
        public List<ProductResponseV1> getAllPrice()
        {

            List<Product> products = _invoiceContext.Products.ToList();

            var response = products.Select(x => new ProductResponseV1
            {
                Id = x.ProductID,
                Price = x.Price
            }).ToList();


            return response;
        }

        [HttpGet]
        public List<Product> getAllProductsCategory()
        {
            return _invoiceContext.Products.Where(x => x.Active == true).ToList();
        }

    }
}
