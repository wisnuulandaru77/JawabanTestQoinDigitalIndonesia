namespace MainService.Services;

public interface ITest01Service
{
    Task<IActionResult> GetByIdAsync(int id);

    Task<IActionResult> CreateAsync(ReqCreateTest01DTO reqCreateTest01DTO);

    Task<IActionResult> UpdateAsync(ReqUpdateTest01DTO reqUpdateTest01DTO);

    Task<IActionResult> DeleteAsync(int id);

    Task<IActionResult> GetListAsync(int page_number, int page_size);
}

public class Test01Service(ApplicationDbContext applicationDbContext) : ITest01Service
{
    public async Task<IActionResult> CreateAsync(ReqCreateTest01DTO reqCreateTest01DTO)
    {
        var test01 = new Test01
        {
            Nama = reqCreateTest01DTO.Nama,
            Status = reqCreateTest01DTO.Status,
            Created = DateTime.Now,
            Updated = DateTime.Now
        };

        await applicationDbContext.AddAsync(test01);
        await applicationDbContext.SaveChangesAsync();

        return new OkObjectResult(new { test01.Id });
    }

    public async Task<IActionResult> DeleteAsync(int id)
    {
        var test01 = await applicationDbContext.Test01s
                                               .FirstOrDefaultAsync(item => item.Id == id);

        if (test01 is null)
            return new NotFoundObjectResult(test01);

        applicationDbContext.Test01s.Remove(test01);

        await applicationDbContext.SaveChangesAsync();

        return new OkObjectResult("Delete success");
    }

    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var test01 = await applicationDbContext.Test01s
                                               .FirstOrDefaultAsync(item => item.Id == id);

        if (test01 is null)
            return new NotFoundObjectResult(test01);

        return new OkObjectResult(test01);

    }

    public async Task<IActionResult> GetListAsync(int page_number, int page_size)
    {
        return new OkObjectResult(
                                   await applicationDbContext.Test01s
                                                             .OrderBy(item => item.Nama)
                                                             .Skip((page_number * page_size) - page_size)
                                                             .Take(page_size)
                                                             .ToListAsync()
                                 ); ;
    }

    public async Task<IActionResult> UpdateAsync(ReqUpdateTest01DTO reqUpdateTest01DTO)
    {
        var test01 = await applicationDbContext.Test01s
                                               .FirstOrDefaultAsync(item => item.Id == reqUpdateTest01DTO.Id);

        if (test01 is null)
            return new NotFoundObjectResult(test01);

        test01.Nama = reqUpdateTest01DTO.Nama;
        test01.Status = reqUpdateTest01DTO.Status;
        test01.Updated = DateTime.Now;

        await applicationDbContext.SaveChangesAsync();

        return new OkObjectResult("Update success");
    }
}
