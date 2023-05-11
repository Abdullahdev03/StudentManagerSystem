using Domain.Dtos;
using Domain.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[EnableCors("MyCors")]

public class GroupController : ControllerBase
{
    private readonly SendMessageService _sendMessageService;

    public GroupController(SendMessageService sendMessageService)
    {
        _sendMessageService = sendMessageService;
    }

    [HttpPost("SendSMS")]
    [AllowAnonymous]
    public async Task<Response<SendMessageDto>> SendMessage(SendMessageDto sms)
    {
       return await _sendMessageService.SendMessage(sms);
    }

}