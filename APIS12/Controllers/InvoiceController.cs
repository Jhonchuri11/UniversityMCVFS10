using APIS12.Models;
using APIS12.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIS12.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceContext _invoiceContext;

        public InvoiceController(InvoiceContext invoiceContext)
        {
            _invoiceContext = invoiceContext;
        }

        [HttpGet]
        public List<Invoice> getAllInvoice()
        {
            return _invoiceContext.Invoices.Where(x => x.Active == true).ToList();
        }

        [HttpPost]
        public void InsertInvoice(InvoiceRequestI request)
        {
            Invoice invoice = new Invoice()
            {
                Date = request.Date,
                InvoiceNumber = request.InvoiceNumber,
                Total = request.Total,
                customerID = request.customerID,
                Active = true
            };

            _invoiceContext.Invoices.Add(invoice);

            _invoiceContext.SaveChanges();
        }

        [HttpPost]
        public void InsertListInvoices(InvoiceRequestList request)
        {
            var customer = _invoiceContext.Customers.Where(x =>
                x.CustomerID == request.CustomerID).FirstOrDefault();

            var invoices = request.Invoices.Select(dto => new Invoice
            {
                Date = dto.Date,
                InvoiceNumber = dto.InvoiceNumber,
                Total = dto.Total,
                Active = true,
                customerID = request.CustomerID
            }).ToList();

            _invoiceContext.Invoices.AddRange(invoices);
            _invoiceContext.SaveChanges();
        }

        [HttpPost]
        public void InsertInvoiceDetail(DetailRequestList request)
        {
            var invoice = _invoiceContext.Invoices.Where(x =>
                x.InvoiceID == request.InvoiceID).FirstOrDefault();

            var product = _invoiceContext.Products.Where(x =>
                x.ProductID == request.InvoiceID).FirstOrDefault();

            var details = request.Details.Select(dto => new Detail
            {
                Amount = dto.Amount,
                Price = dto.Price,
                SubTotal = dto.SubTotal,
                Active = true,
                productID = request.InvoiceID,
                invoiceID = request.InvoiceID
            }).ToList();

            _invoiceContext.Details.AddRange(details);
            _invoiceContext.SaveChanges();
        }

        [HttpPost]
        public IEnumerable<Invoice> getInvoiceByNameCustomer(InvoiceRequest1 invoice)
        {
            var invoices = _invoiceContext.Invoices
                .Include(x => x.customer)
                .Where(x => EF.Functions.Like(x.customer.FirstName, $"%{invoice.FirstName}%"))
                .OrderByDescending(x => x.customer.FirstName) 
                .ToList();

            return invoices;
        }





    }
}
