namespace E_Commerce.Abstractions;

public static class Permissions
{
    #region Properties
    public static string Type { get; set; } = "Permissions";
    #endregion

    #region Permissions

    #region Account Controller 
    public const string Info = "info";
    public const string UpdateInfo = "UpdateInfo";
    public const string ChangePassword = "ChangePassword";
    #endregion

    #region Category Controller
    public const string GetCategories = "Category:ReadAll";
    public const string GetByIDCategory = "Category:GetByid";
    public const string AddCategory = "Category:Add";
    public const string UpdateCategory = "Category:Update";
    public const string DeleteCategoey = "Category:Delete";
    #endregion

    #region Discount Controller
    public const string AddDiscount = "Discount:Add";
    public const string UpdateDiscount = "Discount:Update";
    public const string DeleteDiscount = "Discount:Delete";
    #endregion

    #region Item Controller
    public const string GetItems = "Item:ReadAll";
    public const string GetByIDItem = "Item:GetByid";
    public const string AddItem = "Item:Add";
    public const string UpdateItem = "Item:Update";
    public const string DeleteItem = "Item:Delete";
    #endregion

    #region OfferItem Controller
    public const string GetOfferItems = "OfferItem:ReadAll";
    public const string AddOfferItem = "OfferItem:Add";
    public const string UpdateOfferItem = "OfferItem:Update";
    #endregion

    #region Offers
    public const string GetOffers = "Offer:ReadAll";
    public const string GetByIDOffer = "Offer:GetByid";
    public const string AddOffer = "Offer:Add";
    public const string UpdateOffer = "Offer:Update";
    public const string DeleteOffer = "Offer:Delete";
    #endregion

    #endregion

    #region Method
    public static IList<string?> GetAllPermissions =>
        typeof(Permissions).GetFields().Select(x => x.GetValue(x) as string).ToList();
    #endregion
}
