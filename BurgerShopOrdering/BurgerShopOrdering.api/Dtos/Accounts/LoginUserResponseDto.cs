namespace BurgerShopOrdering.api.Dtos.Accounts
{
    public class LoginUserResponseDto
    {
        public string Token { get; set; }
        public UserResponseDto User { get; set; }
    }
}
