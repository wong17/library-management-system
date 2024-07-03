namespace LibraryManagementSystem.Entities.Dtos.Security
{
    public class UserDto
    {
        private int userId;
        private string? userName;
        private string? email;
        private bool active;
        private string? accessToken;
        private string? refreshToken;
        private DateTime refreshTokenExpiryTime;
        private bool lockoutEnabled;
        private int accessFailedCount;
        private List<RoleDto>? roles = [];

        public int UserId { get => userId; set => userId = value; }
        public string? UserName { get => userName; set => userName = value; }
        public string? Email { get => email; set => email = value; }
        public bool Active { get => active; set => active = value; }
        public string? AccessToken { get => accessToken; set => accessToken = value; }
        public string? RefreshToken { get => refreshToken; set => refreshToken = value; }
        public DateTime RefreshTokenExpiryTime { get => refreshTokenExpiryTime; set => refreshTokenExpiryTime = value; }
        public bool LockoutEnabled { get => lockoutEnabled; set => lockoutEnabled = value; }
        public int AccessFailedCount { get => accessFailedCount; set => accessFailedCount = value; }
        // Roles
        public List<RoleDto>? Roles { get => roles; set => roles = value; }
    }
}
