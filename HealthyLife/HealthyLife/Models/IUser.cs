namespace HealthyLife.Models
{
    public interface IUser
    {
        public User GetUserDetails(string id);

        public void AddUser(UserDTO request);

        public void UpdateUser(User user);

        public User DeleteUser(string id);

        public bool CheckUser(string UserName);

        public bool CheckUser(UserDTO userDTO);

        public bool CheckToken(string value);

        public string CreateToken(UserDTO userDTO);


    }
}
