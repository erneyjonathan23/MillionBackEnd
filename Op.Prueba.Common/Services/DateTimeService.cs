using OP.Prueba.Application.Interfaces;

namespace OP.Prueba.Common.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.Now;
    }
}
