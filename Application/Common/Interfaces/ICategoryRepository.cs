namespace Application.Common.Interfaces;

public interface ICategoryRepository
{
    Task AddDefaultCategoriesAsync();

    bool CheckHaveAnyData();
}
