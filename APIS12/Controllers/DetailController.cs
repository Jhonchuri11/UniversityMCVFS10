using APIS12.Models;
using APIS12.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIS12.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DetailController : ControllerBase
    {

        private readonly InvoiceContext _invoiceContext;

        public DetailController(InvoiceContext invoiceContext)
        {
            _invoiceContext = invoiceContext;
        }

        [HttpGet]
        public List<Detail> getAllDetail()
        {
            return _invoiceContext.Details.Where(x => x.Active == true).ToList();
        }

        [HttpPost]
        public void InsertDetail(DetailRequest deta)
        {
            var productID = _invoiceContext.Products.Find(deta.ProductID);
            var invoiceID = _invoiceContext.Invoices.Find(deta.InvoiceID);

            var detail = new Detail
            {
                Amount = deta.Amount,
                Price = deta.Price,
                SubTotal = deta.SubTotal,
                Active = deta.Active,
                productID = deta.ProductID,
                invoiceID = deta.InvoiceID
            };
            _invoiceContext.Details.Add(detail);

            _invoiceContext.SaveChanges();
        }

        [HttpPost]
        public IEnumerable<Detail> getDetailByInvoiceNumber(DetailRequestV2 deta)
        {
            var details = _invoiceContext.Details
                .Include(x => x.invoice)
                .Include(x => x.product)
                .Where(x => x.invoice.InvoiceNumber == deta.InvoiceNumber)
                .OrderByDescending(x => x.invoice.InvoiceNumber)
                .ToList();
            var invoiceDetails = _invoiceContext.Details
           .Include(i => i.product)
           .Include(i => i.invoice)
           .Where(i => i.invoice.InvoiceNumber == deta.InvoiceNumber)
           .Select(i => new InvoiceDetailDTO
           {
               InvoiceNumber = i.invoice.InvoiceNumber,
               Date = i.invoice.Date,
               Total = i.invoice.Total
           }).ToList(); 

            return details;
        }

    }
}
