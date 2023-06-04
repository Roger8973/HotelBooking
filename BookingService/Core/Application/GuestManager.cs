using Application.Guest.DTO;
using Application.Guest.Ports;
using Application.Guest.Requests;
using Application.Guest.Responses;
using Domain.Exceptions;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class GuestManager : IGuestManager
    {
        private readonly IGuestRepository _guestRepository;
        public GuestManager(IGuestRepository guestRepository)
        {
            _guestRepository = guestRepository;
        }
        public async Task<GuestResponse> CreateGuest(CreateGuestRequest request)
        {
            try
            {
                var guest = GuestDTO.MapToEntity(request.Data);

                await guest.Save(_guestRepository);

                request.Data.Id = guest.Id;

                return new GuestResponse
                {
                    Data = request.Data,
                    Success = true,
                };
            }
            catch (InvalidPersonDocumentIdException)
            {
                return new GuestResponse
                {
                    Success = false,
                    Message = "The ID passed is not valid.",
                    ErrorCode = ErrosCodes.INVALID_PERSON_ID
                };
            }
            catch (MissingRequiredInformation)
            {
                return new GuestResponse
                {
                    Success = false,
                    Message = "Missing required information passed.",
                    ErrorCode = ErrosCodes.MISSING_REQUIRED_INFORMATION
                };
            }
            catch (InvalidEmailException)
            {
                return new GuestResponse
                {
                    Success = false,
                    Message = "The given email is not valid.",
                    ErrorCode = ErrosCodes.INVALID_EMAIL
                };
            }
            catch (Exception)
            {
                return new GuestResponse
                {
                    Success = false,
                    Message = "There was an error when saving to DB",
                    ErrorCode = ErrosCodes.COULD_NOT_STORE_DATA
                };
            }         
        }
    }
}
