namespace MainService.Models.Dtos;

public class ReqCreateTest01DTO
{
    [MaxLength(100)]
    public string Nama { get; set; }

    public byte? Status { get; set; }   
}
