namespace HealthyLife.Models
{
    public interface IUser
    {
        public User GetUserDetails(string id);
        public void AddUser(User user);
        public void UpdateUser(User user);
        public User DeleteUser(string id);
        public bool CheckUser(string id);
    }
}
