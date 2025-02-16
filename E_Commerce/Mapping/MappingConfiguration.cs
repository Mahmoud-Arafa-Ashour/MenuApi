namespace E_Commerce.Mapping
{
    public class MappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Category, CategoryResponse>()
                .Map(dest => dest.CategoryImagePath, src => src.ImagePath);
            //config.NewConfig<RegisterRequest, ApplicationUser>()
            //    .Map(dest => dest.UserName, src => src.Email);
            //config.NewConfig<RegisterRequest, ApplicationUser>()
            //    .Map(dest => dest.Adress, src => src.Address);
            //config.NewConfig<RegisterRequest, ApplicationUser>()
            //    .Map(dest => dest.ResturnatName, src => src.ResturantName);
        }
    }
}
