namespace MainService.Models.Dtos;

public class ReqUpdateTest01DTO : BaseTest01DTO
{
    [MaxLength(100)]
    public string Nama { get; set; }

    public byte Status { get; set; }
}
