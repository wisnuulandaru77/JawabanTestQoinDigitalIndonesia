namespace MainService.Models.Dtos;

public class ReqPagingDTO
{
    [Range(1, int.MaxValue)]    
    
    public int page_number { get; set; } = 1;

    [Range(1,20)]
    public int page_size { get; set; } = 20;
}
