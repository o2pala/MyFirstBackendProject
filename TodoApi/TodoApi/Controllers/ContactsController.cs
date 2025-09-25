using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using NuGet.Packaging.Signing;
//using System;
//using System.Diagnostics;
//using System.Linq;
//using System.Xml.Linq;
//using TodoApi.Models;
using TodoApi.Repository;
using TodoApi.ViewModels;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TodoApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        //private readonly TodoContext _context;
        private readonly IContactRepository _contactRepo;
        private readonly ILogger<ContactsController> logger;

        public ContactsController(IContactRepository contactRepo, ILogger<ContactsController> logger)
        {
            //_context = context;
            _contactRepo = contactRepo;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] PagedDataQuery<ContactQueryRequest> request)
        {
            try
            {
                var result = await _contactRepo.Index(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message }); ;
            }
        }

        [HttpPost]
        public async Task<APIResponse<Contact>> CreateContact([FromBody] ContactCreateRequest contact)
        {
            try
            {
                var result = await _contactRepo.CreateContact(contact);
                return this.APIResult(APICode.Success, result);
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.ToString());
                return this.APIResult(APICode.Fail, new Contact());
            }
        }

        [HttpPost]
        public async Task<APIResponse<Contact>> UpdateContact([FromBody] ContactUpdateRequest contact)
        {
            try
            {
                var result = await _contactRepo.UpdateContact(contact);
                return this.APIResult(APICode.Success, result);
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.ToString());
                return this.APIResult(APICode.Fail, new Contact());
            }
        }

        [HttpGet]
        public async Task<APIResponse<ContactQueryResponse>> GetDetail([FromQuery] Guid id)
        {
            try
            {
                var result = await _contactRepo.GetDetail(id);
                return this.APIResult(APICode.Success, result);
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.ToString());
                return this.APIResult(APICode.Fail, new ContactQueryResponse());
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteContact([FromBody] Guid id)
        {
            try
            {
                await _contactRepo.DeleteContact(id);
                return Ok(this.APIResult(APICode.Success, id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message }); ;
            }
        }

        [HttpPost]
        public async Task<IActionResult> MultiDeleteContact([FromBody] Guid[] ids)
        {
            try
            {
                await _contactRepo.MultiDelete(ids);
                return Ok(this.APIResult(APICode.Success));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}

// CREATE TABLE IF NOT EXISTS contacts (
// id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
// name VARCHAR(100),
// position VARCHAR(100),
// description VARCHAR(200),
// dateCretae   DATETIME NOT NULL,
// userCreate UNIQUEIDENTIFIER NOT NULL
// );

// CREATE TABLE IF NOT EXISTS emails (
// id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
// value INT,
// contactId UNIQUEIDENTIFIER,
// FOREIGN KEY (contactId) REFERENCES contacts(id)
// );

// CREATE TABLE IF NOT EXISTS phones (
// id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
// value INT,
// contactId UNIQUEIDENTIFIER,
// FOREIGN KEY (contactId) REFERENCES contacts(id)
// );

// CREATE TABLE IF NOT EXISTS customers (
// id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
// name VARCHAR(100),
// customerStageId VARCHAR(100),
// dateCretae   DATETIME NOT NULL,
// userCreate UNIQUEIDENTIFIER NOT NULL,
// contactId UNIQUEIDENTIFIER,
// FOREIGN KEY (contactId) REFERENCES contacts(id)
// );

// CREATE TABLE IF NOT EXISTS customerEmails (
// id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
// value INT,
// contactId UNIQUEIDENTIFIER,
// FOREIGN KEY (contactId) REFERENCES contacts(id)
// );

// CREATE TABLE IF NOT EXISTS customerPhones (
// id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
// value INT,
// contactId UNIQUEIDENTIFIER,
// FOREIGN KEY (contactId) REFERENCES contacts(id)
// );

// ---------------------------------------------------------------------------
// SELECT * FROM contacts as _context

// SELECT * FROM contacts as _context WHERE _context.id = @new_id;


// INSERT INTO contacts
// SELECT 'name', 'emails' , 'phones', 'dateCreate' , 'userCreate', ....
// WHERE NOT EXISTS(SELECT 1 FROM YourTable WHERE id = @new_id);

// UPDATE contacts
// SET name = 'opal' , description= 'Hi, the world' , ....
// WHERE contacts.id = @new_id;

// DELETE FROM contacts WHERE contacts.id = @new_id

// -------------------------------------
// Map new customer UPDATE
//if (_customer != null && _customer.id != contact.customer_id)
//{
//    var myCustomer = _context.Customers
//            .Where(w => w.id == entity.customer_id)
//            .Select(s => new Customer
//            {
//                Name = s.Name,
//                id = s.id,
//                CustomerEmails = s.CustomerEmails.Select(e => new CustomerEmail
//                {
//                    id = e.id,
//                    value = e.value,
//                    customerId = e.customerId
//                }).ToList(),
//                CustomerPhones = s.CustomerPhones.Select(s => new CustomerPhone
//                {
//                    text = s.text,
//                    value = s.value,
//                    customerId = s.customerId
//                }).ToList(),
//                CustomerStageId = s.CustomerStageId
//            });

//    entity.Customer = (Customer?)myCustomer;
// -------------------------------------------------------------
// UPDATE customer phones
//_customer.CustomerPhones.Clear();
//foreach (var phone in )
//{
//    _customer.CustomerPhones.Add(new CustomerPhone { value = phone.value, text = phone.text });
//}

//_customer.CustomerEmails.Clear();
//foreach (var email in contact.customerInfo.customerEmails)
//{
//    _customer.CustomerEmails.Add(new CustomerEmail { value = email });
//}
//_context.Customers.Add((Customer)myCustomer);
//}
// ------------------------------------------------------------
// CREATE new customer
//entity.Customer = _context.Customers
//    .Where(w => w.id == entity.customer_id)
//    .Select(s => new Customer
//    {
//        Name = s.Name,
//        id = s.id,
//        CustomerEmails = s.CustomerEmails.Select(e => new CustomerEmail
//        {
//            id = e.id,
//            value = e.value,
//            customerId = e.customerId
//        }).ToList(),
//        CustomerPhones = s.CustomerPhones.Select(s => new CustomerPhone
//        {
//            text = s.text,
//            value = s.value,
//            customerId = s.customerId
//        }).ToList(),
//        CustomerStageId = s.CustomerStageId
//    }).FirstOrDefault();
