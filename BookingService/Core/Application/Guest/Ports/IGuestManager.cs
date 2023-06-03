using Application.Guest.Requests;
using Application.Guest.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Guest.Ports
{
    public interface IGuestManager
    {
        Task<GuestResponse> CreateGuest(CreateGuestRequest request);
    }
}
