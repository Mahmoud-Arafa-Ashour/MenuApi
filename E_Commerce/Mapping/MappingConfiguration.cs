using E_Commerce.Contracts.Categories;

namespace E_Commerce.Mapping
{
    public class MappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Category, CategoryResponse>()
                .Map(dest => dest.CategoryImagePath, src => src.ImagePath);
        }
    }
}
