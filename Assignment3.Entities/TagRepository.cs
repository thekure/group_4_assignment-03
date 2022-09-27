using Assignment3.Core;
namespace Assignment3.Entities;

public class TagRepository : ITagRepository
{
    private readonly KanbanContext _context;

    public TagRepository(KanbanContext context){
        _context = context;
    }

    public (Response Response, int TagId) Create(TagCreateDTO tag)
    {
        var entity = _context.Tags.FirstOrDefault(t => t.Name == tag.Name);
        Response Response;

        if (entity is null){
            entity = new Tag(){Name = tag.Name};

            _context.Tags.Add(entity);
            _context.SaveChanges();

            Response = Response.Created;
        } else {
            Response = Response.Conflict;
        }

        return (Response, entity.Id);
        
    }

    public Response Delete(int tagId, bool force = false)
    {
        var tag = _context.Tags.Include(t => t.Tasks).FirstOrDefault(t => t.Id == tagId);
        Response response;

        if (tag is null)
        {
            response = Response.NotFound;
        }
        else if (tag.Tasks.Any() && !force)
        {
            response = Response.Conflict;
        }
        else
        {
            _context.Tags.Remove(tag);
            _context.SaveChanges();

            response = Response.Deleted;
        }

        return response;
    }

    public TagDTO Read(int tagId)
    {
        var tags = from t in _context.Tags
                     where t.Id == tagId
                     select new TagDTO(t.Id, t.Name);

        return tags.FirstOrDefault();
    }

    public IReadOnlyCollection<TagDTO> ReadAll()
    {
        var tags =  from t in _context.Tags
                    orderby t.Name
                    select new TagDTO(t.Id, t.Name);

        return tags.ToArray();
    }

    public Response Update(TagUpdateDTO tag)
    {
        var entity = _context.Tags.Include(t => t.Tasks).FirstOrDefault(t => t.Id == tag.Id);
        Response response;

        if (entity is null)
        {
            response = Response.NotFound;
        }
        else if (_context.Tags.FirstOrDefault(t => t.Id != tag.Id && t.Name == tag.Name) != null)
        {
            response = Response.Conflict;
        }
        else
        {
            entity.Name = tag.Name;
            _context.SaveChanges();

            response = Response.Updated;
        }

        return response;
        }
    }

