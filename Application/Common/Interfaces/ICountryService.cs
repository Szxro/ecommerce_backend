using Domain.Common.Request;

namespace Application.Common.Interfaces;

public interface ICountryService
{
    Task<ICollection<CountryRequest>> GetCountries();
}
