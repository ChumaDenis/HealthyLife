namespace HealthyLife.Models.Auth
{
    public interface IUser
    {
        public User GetUserDetails(string id);

        public void AddUser(UserRegisterRequest request);

        public void VerifyUser(string token);

        public string PasswordResetAcces(string email);

        public void PasswordReset(ResetPasswordRequest request);

        public User DeleteUser(string id);

        public bool CheckUser(string UserName);

        public bool CheckUser(UserLoginRequest request);

        public bool CheckUser(UserRegisterRequest request);


        public bool CheckToken(string value);

        public string CreateToken(UserLoginRequest userDTO);


    }
}
