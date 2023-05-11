using System.Net;

namespace Domain.Wrapper;

public class PagedResponce<T> : Response<T>
{
    public PagedResponce(T data) : base(data)
    {
    }

    public PagedResponce(HttpStatusCode statusCode, List<string> errors) : base(statusCode, errors)
    {
    }


    public PagedResponce(T data, int totalRecords, int pageNumber, int pageSize) : base(data)
    {
        TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        PageNumber = pageNumber;
        TotalRecords = totalRecords;
        PageSize = pageSize;

    }

    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public int PageSize { get; set; }
}