using APIS12.Models;
using APIS12.Request;
using APIS12.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIS12.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {

        private readonly InvoiceContext _invoiceContext;

        public CustomersController(InvoiceContext invoiceContext)
        {
            _invoiceContext = invoiceContext;
        }

        [Authorize]
        [HttpGet(Name = "Get2")]
        public IEnumerable<PersonResponse> Get2()
        {
            List<PersonResponse> response = new List<PersonResponse>();

            for (int i = 1; i <= 100; i++)
            {
                PersonResponse personResponse = new PersonResponse();
                personResponse.FirstName = "Persona" + i;
                personResponse.LastName = "Apellido" + i;

                response.Add(personResponse);
            }

            return response;
        }

        [Authorize("Administrador")]
        [HttpGet(Name = "Get")]
        public IEnumerable<PersonResponse> Get()
        {
            List<PersonResponse> personas = new List<PersonResponse>();

            for (int i = 1; i <= 100; i++)
            {
                PersonResponse personResponse = new PersonResponse();
                personResponse.FirstName = "Persona" + i;
                personResponse.LastName = "Apellido" + i;

                personas.Add(personResponse);
            }

            return personas;
        }



        [HttpGet]
        public List<Customer> getAllCustomer()
        {
            return _invoiceContext.Customers.Where(x => x.Active == true).ToList();
        }

        [HttpPost]
        public void InsertCustomer(CustomerRequestI request)
        {
            Customer customer = new Customer()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                DocumentNumber = request.DocumentNumber,
                Active = true
            };

            _invoiceContext.Customers.Add(customer);
            _invoiceContext.SaveChanges();
        }

        [HttpPost]
        public void deleteCustomer(CustomerRequestD request)
        {
            _invoiceContext.Customers.Find(request.CustomerID).Active = false;
            _invoiceContext.SaveChanges();
        }

        [HttpPut]
        public void UpdateNumberDoc(RequestUpdateDoc  request)
        {
            Customer customer = _invoiceContext.Customers.Where(x => x.CustomerID == request.CustomerID).FirstOrDefault();

            customer.DocumentNumber = request.DocumentNumber;
            customer.Email = request.Email;

            _invoiceContext.Entry(customer).State = EntityState.Modified;
            _invoiceContext.SaveChanges();
        }


        [HttpPost]
        public List<Customer> getCustomerByCriteria(CustomerRequestV1 customerRequest)
        {
            var query = _invoiceContext.Customers.Where(x => x.Active);

            if (!string.IsNullOrEmpty(customerRequest.firstName))
            {
                query = query.Where(x => x.FirstName.Contains(customerRequest.firstName));
            }

            if (!string.IsNullOrEmpty(customerRequest.lastName))
            {
                query = query.Where(x => x.LastName.Contains(customerRequest.lastName));
            }

            if (!string.IsNullOrEmpty(customerRequest.email))
            {
                query = query.Where(x => x.Email.Contains(customerRequest.email));
            }
            
            return query.OrderByDescending(x => x.LastName).ToList();

        }



    }
}
