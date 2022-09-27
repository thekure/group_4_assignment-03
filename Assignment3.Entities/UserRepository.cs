using Assignment3.Core;

namespace Assignment3.Entities;

public class UserRepository : IUserRepository
{
    public (Response Response, int UserId) Create(UserCreateDTO user)
    {
        throw new NotImplementedException();
    }

    public Response Delete(int userId, bool force = false)
    {
        throw new NotImplementedException();
    }

    public UserDTO Read(int userId)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<UserDTO> ReadAll()
    {
        throw new NotImplementedException();
    }

    public Response Update(UserUpdateDTO user)
    {
        throw new NotImplementedException();
    }
}
