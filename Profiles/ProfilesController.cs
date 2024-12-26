using Microsoft.AspNetCore.Mvc;
using Z1.Auth.Models;
using Z1.Core;
using Z1.Profiles.Dtos;
using Z1.Profiles.Interfaces;

namespace Z1.Auth.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProfilesController : ControllerBase
    {
        private IProfileService _profileService;

        public ProfilesController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("get-interests")]
        public IActionResult GetInterests()
        {
            return Ok(_profileService.GetInterests());
        }

        [HttpGet("get-languages")]
        public IActionResult GetLanguages()
        {
            return Ok(_profileService.GetLanguages());
        }

        [HttpPost("create/")]
        public async Task<IActionResult> Create(CreateProfileRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = (User)HttpContext.Items["User"];

            return Ok(await _profileService.CreateProfile(model, user));
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile image)
        {
            if (image == null)
                return BadRequest("Images are required.");

            var user = (User)HttpContext.Items["User"];
            return Ok(await _profileService.UploadImage(image, user));
        }

        [HttpPost("upload-bulk-images")]
        public async Task<IActionResult> UploadBulkImages([FromForm] BulkImageUploadDTO model)
        {
            if (model.Images == null || model.Images.Count == 0)
            {
                return BadRequest("No files uploaded.");
            }

            var user = (User)HttpContext.Items["User"];
            return Ok(await _profileService.UploadBulkImages(model, user));
        }

        [HttpPatch("update/")]
        public async Task<IActionResult> Update([FromBody] UpdateProfileRequestDto patch)
        {
            if(patch == null)
            {
                return BadRequest("Data is required");
            }
            var user = (User)HttpContext.Items["User"];
            return Ok(await _profileService.UpdateProfile(patch, user));
        }

        [HttpPost("delete/")]
        public async Task<IActionResult> Delete()
        {
            return Ok();
        }

        [HttpGet("get-partial-chat-profile/{matchId}")]
        public async Task<IActionResult> GetPartialChatProfileAsync(int matchId)
        {
            var user = (User)HttpContext.Items["User"];
            return Ok(await _profileService.GetPartialChatProfileAsync(matchId, user));
        }

        [HttpGet("get-chat-profile/{matchId}")]
        public async Task<IActionResult> GetChatProfileAsync([FromRoute]int matchId)
        {
            var user = (User)HttpContext.Items["User"];
            return Ok(await _profileService.GetChatProfileAsync(matchId, user));
        }

        [HttpGet("get-profile/{userId}")]
        public async Task<IActionResult> GetProfileAsync([FromRoute] int userId)
        {
            var user = (User)HttpContext.Items["User"];
            return Ok(await _profileService.GetProfileAsync(userId, user));
        }

        [HttpDelete("delete-image/{fileId}")]
        public async Task<IActionResult> DeleteImageAsync([FromRoute] string fileId)
        {
            var user = (User)HttpContext.Items["User"];
            return Ok(await _profileService.DeleteImageAsync(fileId, user));
        }

        [HttpPost("update-image-order/")]
        public async Task<IActionResult> UpdateImageOrder(UpdateImageOrderDto request)
        {
            var user = (User)HttpContext.Items["User"];
            return Ok(await _profileService.UpdateImageOrder(request.NewOrder, user));
        }

        // helper methods

        private string ipAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
