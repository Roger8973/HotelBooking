using Domain.Exceptions;
using Domain.Ports;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Guest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public PersonId DocumentId { get; set; }

        private void ValidadeState()
        {
            if (DocumentId == null ||
                DocumentId.IdNumber.Length <= 3 ||
                DocumentId.DocumentType == 0)
            {
                throw new InvalidPersonDocumentIdException();
            }

            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(SurName) || string.IsNullOrEmpty(Email))
            {
                throw new MissingRequiredInformation();
            }

            if (Utils.ValidadeEmail(this.Email) == false)
            {
                throw new InvalidEmailException();
            }
        }
        
        public async Task Save(IGuestRepository guestRepository)
        {
            this.ValidadeState();

            if (this.Id == 0)
            {
                this.Id = await guestRepository.Create(this);
            }
            else
            {
                 //await guestRepository.Update(this);
            }
        }
    }
}
