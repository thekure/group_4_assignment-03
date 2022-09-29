using Assignment3.Core;

namespace Assignment3.Entities;

public class UserRepository : IUserRepository{

    private readonly KanbanContext _context;

    public UserRepository(KanbanContext context){
        _context = context;
    }

    public (Response Response, int UserId) Create(UserCreateDTO user)
    {
        var entity = _context.Users.FirstOrDefault(u => u.Email == u.Email);
        Response response;

        if (entity is null){
            entity = new User(){
                Name = user.Name,
                Email = user.Email
            };
                        
            
            _context.Users.Add(entity);
            _context.SaveChanges();

            response = Response.Created;
        } else {
            response = Response.Conflict;
        }
        return (response, entity.Id);

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
