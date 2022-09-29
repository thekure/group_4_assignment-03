using Assignment3.Core;

namespace Assignment3.Entities;

public class UserRepository : IUserRepository{

    private readonly KanbanContext _context;

    public UserRepository(KanbanContext context){
        _context = context;
    }

    public (Response Response, int UserId) Create(UserCreateDTO user)
    {
        // var entity = _context.Users.FirstOrDefault(u => u.Email == u.Email);
        // Response response;

        // if (entity is null){
        //     entity = new User();
                
        //         var t = _context.Tags
        //             .Where(t => t.Name == s)
        //             .FirstOrDefault();
                
            

           

        //     entity = new Task(){
        //         Title = task.Title, 
        //         Description = task.Description, 
        //         Tags = convertedTags,
        //         Created = DateTime.Now,
        //         StateUpdated = DateTime.Now
        //     };
             
            
        //     entity.State = State.New;
            

        //     _context.Tasks.Add(entity);
        //     _context.SaveChanges();

        //     response = Response.Created;
        // } else {
        //     response = Response.Conflict;
        // }
        // return (response, entity.Id);

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
