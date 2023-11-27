namespace Application.Common.Interfaces;

public interface ICategoryRepository
{
    Task AddDefaultCategories();

    bool CheckHaveAnyData();
}
