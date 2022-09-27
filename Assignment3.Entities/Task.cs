namespace Assignment3.Entities;

public class Task
{

    public int Id {get; set;}

    [Required][StringLength(100)]
    public string ?Title {get; set;}

    public int ?AssignedToId {get{
        return UserId;
    }}

    [StringLength(int.MaxValue)]
    public string ?Description {get; set;}

    public int UserId {get; set;}

    public virtual User ?User {get; set;}

    [Required]
    public State State {get; set; }

    public virtual ICollection<Tag> Tags { get; set; }
    
    
}

