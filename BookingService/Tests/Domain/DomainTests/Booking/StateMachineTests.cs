using Domain.Entities;
using Domain.Enums;
using Action = Domain.Enums.Action;

namespace DomainTests.Bookings
{
    public class StateMachineTests
    {
        [Fact]
        public void AlwaysStartWithCreatedStatus()
        {
            var booking = new Booking();

            Assert.Equal(Status.Created, booking.CurrentStatus);
        }

        [Fact]
        public void MustChangeStatusToPaidWhenStatusIsCreated()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Pay);

            Assert.Equal(Status.Paid, booking.CurrentStatus);
        }

        //IMPLEMENTAR MAIS TESTES
    }
}