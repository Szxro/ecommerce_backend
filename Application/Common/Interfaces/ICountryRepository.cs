namespace Application.Common.Interfaces;

public interface ICountryRepository
{
   Task AddDefaultCountries();

   bool CheckHaveAnyData();
}
