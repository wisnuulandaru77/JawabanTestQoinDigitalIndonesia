namespace MainService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessageBrokerController(IMessageProducerService messageProducerService) : ControllerBase
{
    [HttpPost]
    public IActionResult CreateAsync([FromBody] ReqCreateTest01DTO reqCreateTest01DTO)
    {
        var messageBrokerDTO = new MessageBrokerDTO<ReqCreateTest01DTO>
        {
            command = "create",
            data = new ReqCreateTest01DTO
            {
                Nama = reqCreateTest01DTO.Nama,
                Status = reqCreateTest01DTO.Status
            }
        };

        return messageProducerService.SendingMessage(messageBrokerDTO);
    }

    [HttpPut]
    public IActionResult UpdateAsync([FromBody] ReqUpdateTest01DTO reqUpdateTest01DTO)
    {
        var messageBrokerDTO = new MessageBrokerDTO<ReqUpdateTest01DTO>
        {
            command = "update",
            data = new ReqUpdateTest01DTO
            {
                Id = reqUpdateTest01DTO.Id,
                Nama = reqUpdateTest01DTO.Nama,
                Status = reqUpdateTest01DTO.Status
            }
        };

        return messageProducerService.SendingMessage(messageBrokerDTO);
    }

    [HttpDelete]
    [Route(GlobalVariable.RoutingForId)]
    public IActionResult DeleteAsync(int id)
    {
        var messageBrokerDTO = new MessageBrokerDTO<BaseTest01DTO>
        {
            command = "delete",
            data = new BaseTest01DTO
            {
                Id = id
            }
        };

        return messageProducerService.SendingMessage(messageBrokerDTO);
    }
}
