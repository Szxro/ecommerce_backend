namespace Application.Common.Interfaces;

public interface ICountryRepository
{
   Task AddDefaultCountriesAsnyc();

   bool CheckHaveAnyData();
}
