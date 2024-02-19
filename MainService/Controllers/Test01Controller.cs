namespace MainService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class Test01Controller(ITest01Service test01Service) : ControllerBase
{
    [HttpGet]
    [Route(GlobalVariable.RoutingForId)]
    public  async Task<IActionResult> GetByIdAsync(int id)
    {
        return await test01Service.GetByIdAsync(id);
    }

    [HttpGet]
    [Route(GlobalVariable.RoutingForPaging)]
    public async Task<IActionResult> GetListAsync([FromRoute] ReqPagingDTO reqPagingDTO)
    {
        return await test01Service.GetListAsync(reqPagingDTO.page_number, reqPagingDTO.page_size);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] ReqCreateTest01DTO reqCreateTest01DTO)
    {
        return await test01Service.CreateAsync(reqCreateTest01DTO);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] ReqUpdateTest01DTO reqUpdateTest01DTO)
    {
        return await test01Service.UpdateAsync(reqUpdateTest01DTO);
    }

    [HttpDelete]
    [Route(GlobalVariable.RoutingForId)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        return await test01Service.DeleteAsync(id);
    }
}
