using Microsoft.AspNetCore.Mvc;
using webapi.Models.Response;

namespace webapi.Controllers {
    public class CustomeControllerBase : ControllerBase {
        public ActionResult HandleRes (ResponseData res) {
            if (res.BadRequest != null) {
                return BadRequest (res.BadRequest);
            }
            if (res.NotFound != null) {
                return NotFound (res.NotFound);
            }
            if (res.Forbid != "") {
                return Forbid (res.Forbid);
            }

            if (res.Data != null) {
                return Ok (res.Data);
            }

            return Ok (new { success = true });
        }
    }
}