namespace Tutorial5.DTOs;

public class BookWithAuthorsDto_template
{
    public string Name { get; set; }
    public double Price { get; set; }
    public List<AuthorDto> Authors { get; set; }
}

public class AuthorDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}