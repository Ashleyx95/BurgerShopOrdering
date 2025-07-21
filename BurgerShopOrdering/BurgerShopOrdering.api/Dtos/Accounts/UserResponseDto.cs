namespace BurgerShopOrdering.api.Dtos.Accounts
{
    public class UserResponseDto
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsClient { get; set; }
    }
}
