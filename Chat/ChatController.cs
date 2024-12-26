using Microsoft.AspNetCore.Mvc;
using Z1.Auth.Models;
using Z1.Chat.Interfaces;
using Z1.Core;

namespace Z1.Chat
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ChatController: ControllerBase
    {
        private IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("get-chat-history")]
        public async Task<IActionResult> GetChatHistory(int matchId, int page = 1, int pageSize = 20)
        {
            var skip = (page - 1) * pageSize;
            var messages = await _chatService.GetChatHistory(matchId, skip, pageSize);
            return Ok(messages);
        }

        [HttpGet("get-all-chats")]
        public async Task<IActionResult> GetAllChats()
        {
            var user = (User)HttpContext.Items["User"];
            var messages = await _chatService.GetAllChats(user);
            return Ok(messages);
        }

        [HttpGet("get-chat-list")]
        public IActionResult GetChatList()
        {
            var user = (User)HttpContext.Items["User"];
            var messages = _chatService.GetChatList(user);
            return Ok(messages);
        }

        [HttpGet("get-current-match-chat")]
        public IActionResult GetCurrentMatchChat()
        {
            var user = (User)HttpContext.Items["User"];
            var messages = _chatService.GetCurrentMatchChat(user);
            return Ok(messages);
        }

        [HttpPost("mark-read/{messageId}")]
        public IActionResult MarkAsRead(long messageId)
        {
            var user = (User)HttpContext.Items["User"];
            var messages = _chatService.MarkAsReadAsync(user, messageId);
            return Ok(messages);
        }

        [HttpPost("delete-history/{matchId}/{lastMessageId}")]
        public IActionResult DeleteHistory([FromRoute]int matchId,[FromRoute]int lastMessageId)
        {
            var user = (User)HttpContext.Items["User"];
            var messages = _chatService.DeleteHistory(user, matchId, lastMessageId);
            return Ok(messages);
        }
    }
}
