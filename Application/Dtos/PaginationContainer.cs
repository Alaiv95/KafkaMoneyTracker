namespace Application.Dtos;

public class PaginationContainer<T>
{
    public List<T> Data { get; set; }
    
    public int PageNumber { get; set; }

    public int TotalPages { get; set; }
}