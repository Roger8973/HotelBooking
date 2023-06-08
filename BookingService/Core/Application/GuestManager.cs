using Application.Guest.DTO;
using Application.Guest.Ports;
using Application.Guest.Requests;
using Application.Guest.Responses;
using Domain.Exceptions;
using Domain.Ports;

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

        public async Task<GuestResponse> GetGuest(int id)
        {
            var guest = await _guestRepository.Get(id);

            if (guest == null)
            {
                return new GuestResponse
                {
                    Success = false,
                    ErrorCode = ErrosCodes.GUEST_NOT_FOUND,
                    Message = "No Guest record was found with the given Id."
                };
            }

            return new GuestResponse
            {
                Success = true,
                Data = GuestDTO.MapToDTO(guest)
            };
        }
    }
}
