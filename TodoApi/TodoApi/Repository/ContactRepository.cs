using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
//using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using TodoApi.Models;
using TodoApi.ViewModels;

namespace TodoApi.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly TodoContext _context;
        public ContactRepository(TodoContext context)
        {
            _context = context;
        }
        public async Task<Contact> CreateContact(ContactCreateRequest contact) // frontend check input => check fields ที่รับข้อมูล => API http method => content type check JSON,URL => data...
        {
            var entity = new Contact // response => check ข้อมูลที่เข้ามาเพิ่ม from frontend
            {
                id = Guid.NewGuid(),
                name = contact.name,
                position = contact.position,
                description = contact.description,
                customerId = contact.customer_id,
                dateCreate = DateTime.UtcNow
            };
            await _context.Contacts.AddAsync(entity);

            if (contact.phones != null)
            {
                foreach (var item in contact.phones)
                {
                    var _phone = new Phone
                    {
                        id = Guid.NewGuid(),
                        value = item.value,
                        contactId = entity.id,
                    };
                    await _context.Phones.AddAsync(_phone);
                }
            }

            if (contact.emails != null)
            {
                foreach (var item in contact.emails)
                {
                    var _email = new Email
                    {
                        id = Guid.NewGuid(),
                        value = item.value,
                        contactId = entity.id,
                    };
                    await _context.Emails.AddAsync(_email);
                }
            }

            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteContact(Guid id)
        {
            var contactModel = await _context.Contacts.FirstOrDefaultAsync(f => f.id == id);

            var phoneModel = await _context.Phones
                .Where(w => w.contactId == id)
                .ToListAsync();

            var emailModel = await _context.Emails
               .Where(w => w.contactId == id)
               .ToListAsync();

            if (contactModel != null)
            {
                if (emailModel != null)
                {
                    _context.Emails.RemoveRange(emailModel);
                }
                if (phoneModel != null)
                {
                    _context.Phones.RemoveRange(phoneModel);
                }
                _context.Contacts.Remove(contactModel);
            }
            //if (entity != null)
            //{
            //    _context.Contacts.Remove(entity);
            //    _context.Phones.RemoveRange(_phone);
            //    _context.Emails.RemoveRange(_email);

            //}
            await _context.SaveChangesAsync();
            return;
        }

        public async Task<ContactQueryResponse> GetDetail(Guid id)
        {
            var contactModel = await _context.Contacts.FindAsync(id);
            var model = await _context.Contacts
                   .Where(w => w.id == id)
                   .Select(s => new ContactQueryResponse
                   {
                       contact_id = s.id,
                       name = s.name,
                       description = s.description,
                       position = s.position,
                       emails = s.emails.Select(s => new IdValueResult(s.id, s.value)
                       {
                           id = s.id,
                           value = s.value
                       }).ToList(),
                       phones = s.phones.Select(s => new IdValueResult(s.id, s.value)
                       {
                           id = s.id,
                           value = s.value
                       }).ToList(),
                       customerInfo = s.Customer == null ? null : new GetCustomerListResponse
                       {
                           customer_id = s.customerId,
                           name = s.Customer.Name,
                           customer_stage_id = s.Customer.CustomerStageId,
                           customerEmails = s.Customer.CustomerEmails.Select(e => e.value).ToList(),
                           customerPhones = s.Customer.CustomerPhones.Select(e => new customer_phone
                           {
                               value = e.value,
                               text = e.text
                           }).ToList()
                       }
                   }).SingleOrDefaultAsync();
            return model;
        }

        public async Task<PagedDataResult<ContactQueryResponse>> Index(PagedDataQuery<ContactQueryRequest> request) // Business logic
        {
            //IQueryable<Contact> iQueryable = _context.Contacts
            //.Include(c => c.phones) 
            //.Include(c => c.emails); //Include is LEFT JOIN 

            // If It be INNER JOIN, the result will be only contacts with matching phones are included.
            IQueryable<Contact> iQueryable = from c in _context.Contacts
                                                 //join p in _context.Phones on c.id equals p.contactId //INNER JOIN cause we filtering them
                                                 //join e in _context.Emails on c.id equals e.contactId
                                             select new Contact
                                             {
                                                 id = c.id,
                                                 name = c.name,
                                                 position = c.position,
                                                 description = c.description,
                                                 customerId = c.customerId,
                                                 phones = c.phones,
                                                 emails = c.emails
                                             }; // If you're not filtering them in join, This syntax = LEFT JOIN



            if (request.search != null)
            {
                ContactQueryRequest search = request.search;
                iQueryable = iQueryable
                    .Where(w =>
                    string.IsNullOrEmpty(search.text)
                    || w.name.ToLower().Contains(search.text.ToLower())
                    || w.phones.Any(p => !string.IsNullOrEmpty(p.value) && p.value.ToLower().Contains(search.text.ToLower()))
                    || w.emails.Any(p => !string.IsNullOrEmpty(p.value) && p.value.ToLower().Contains(search.text.ToLower()))
                    );
            }
            if (request.sortColumn != null)
            {
                if (request.sortColumn == "customer_name")
                {
                    iQueryable = request.sortDirection == "DESC" ? iQueryable.OrderByDescending(c => c.Customer.Name) : iQueryable.OrderBy(c => c.Customer.Name);

                }
                else
                {
                    iQueryable = iQueryable.SortWith(request.sortColumn, request.sortDirection == SortColumnType.ASC);
                }
            }

            var count = await iQueryable.CountAsync();
            var pageItems = await iQueryable
                                    .TakePage(request.pageIndex, request.pageSize, count, out int actualPageIndex)
                                    .ToListAsync();
            var customerIds = pageItems
                                .Select(c => c.customerId)
                                .Distinct()
                                .ToList();

            var customers = await _context.Customers
                            .Where(c => customerIds.Contains(c.id))
                            .Include(c => c.CustomerEmails)
                            .Include(c => c.CustomerPhones)
                            .ToListAsync();
            var contactQuery = new Dictionary<Guid, ContactQueryResponse>();
            var data = new List<ContactQueryResponse>();

            foreach (var item in pageItems)
            {
                var customer = customers.FirstOrDefault(c => c.id == item.customerId);
                ContactQueryResponse _contact;
                if (!contactQuery.TryGetValue(item.id, out _contact))
                {
                    _contact = new ContactQueryResponse
                    {
                        contact_id = item.id,
                        name = item.name,
                        position = item.position,
                        description = item.description,
                        emails = item.emails.Select(e => new IdValueResult(e.id, e.value)).ToList(),
                        phones = item.phones.Select(p => new IdValueResult(p.id, p.value)).ToList(),
                        customerInfo = new GetCustomerListResponse()
                        {
                            customer_id = customer.id,
                            name = customer.Name,
                            customer_stage_id = customer.CustomerStageId,
                            customerEmails = customer.CustomerEmails.Select(e => e.value).ToList(),
                            customerPhones = customer.CustomerPhones.Select(p => new customer_phone
                            {
                                text = p.text,
                                value = p.value
                            }).ToList()
                        }

                    };
                    data.Add(_contact);
                    contactQuery.Add(item.id, _contact);
                }
            }
            var entity = new PagedDataResult<ContactQueryResponse>()
            {
                pageSize = request.pageSize,
                pageIndex = actualPageIndex,
                rowCount = count,
                data = data
            };
            return entity;
        }

        public async Task MultiDelete(Guid[] ids)
        {
            foreach (var item in ids)
            {
                var entity = await _context.Contacts
                    .Where(w => w.id == item)
                    .SingleOrDefaultAsync(); // Pick up all data and find the match, if >1 throw, if 0 null 

                var _phone = await _context.Phones
                   .Where(w => w.contactId == item)
                   .ToListAsync();
                var _email = await _context.Emails
                   .Where(w => w.contactId == item)
                   .ToListAsync();
                if (entity != null)
                {
                    _context.Contacts.Remove(entity);
                    _context.Phones.RemoveRange(_phone);
                    _context.Emails.RemoveRange(_email);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Contact> UpdateContact(ContactUpdateRequest contact)
        {
            var entity = await (from c in _context.Contacts
                                where c.id == contact.contact_id
                                select c).FirstAsync();
            //var entity = await _context.Contacts
            //                .Where(w => w.id == contact.contact_id)
            //                .SingleAsync();

            entity.name = contact.name;
            entity.description = contact.description;
            entity.position = contact.position;
            entity.customerId = contact.customer_id;

            List<Guid> currentPhoneId = new List<Guid>();

            //var oldPhones = await _context.Phones
            //                    .Where(w => w.contactId == entity.id)
            //                    .ToListAsync();
            var oldPhones = await (from p in _context.Phones
                                   where p.contactId == entity.id
                                   select p).ToListAsync();
            var oldPhoneId = oldPhones.Select(s => s.id).ToHashSet();

            List<Guid> currentEmailId = new List<Guid>();
            var oldEmails = await _context.Emails
                            .Where(w => w.contactId == entity.id)
                            .ToListAsync();
            var oldEmailId = oldEmails.Select(s => s.id).ToHashSet();

            //var _customer = await _context.Customers
            //    .Include(c => c.CustomerPhones)
            //    .Include(c => c.CustomerEmails)
            //    .FirstOrDefaultAsync(s => s.id == contact.customer_id && entity.customerId == s.id);

            // collect existing customer
            //if (_customer != null && _customer.id == contact.customer_id)
            //{
            //    _customer = await _context.Customers
            //        .FirstOrDefaultAsync(c => c.id == _customer.id);
            //    entity.Customer = _customer;
            //}

            foreach (var item in contact.phones)
            {
                Phone _phone;
                if (item.id != Guid.Empty && oldPhoneId.Contains(item.id))
                {
                    _phone = oldPhones.Single(s => s.id == item.id && entity.id == s.contactId);
                    _phone.value = item.value;
                }
                else
                {
                    _phone = new Phone
                    {
                        contactId = entity.id,
                        id = Guid.NewGuid(),
                        value = item.value
                    };
                    await _context.Phones.AddAsync(_phone);
                }
                currentPhoneId.Add((Guid)_phone.id);

            }
            var phoneToDelete = oldPhones.Where(w => !currentPhoneId.Contains((Guid)w.id)).ToList();
            _context.Phones.RemoveRange(phoneToDelete);

            foreach (var item in contact.emails)
            {
                Email _email;
                if (item.id != null && oldEmailId.Contains(item.id))
                {
                    _email = oldEmails.Single(s => s.id == item.id && entity.id == s.contactId);
                    _email.value = item.value;
                }
                else
                {
                    _email = new Email
                    {
                        contactId = entity.id,
                        id = Guid.NewGuid(),
                        value = item.value
                    };
                    await _context.Emails.AddAsync(_email);
                }
                currentEmailId.Add((Guid)_email.id);
            }

            var emailToDelete = oldEmails.Where(w => !currentEmailId.Contains((Guid)w.id)).ToList();
            _context.Emails.RemoveRange(emailToDelete);

            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
