using Microsoft.AspNetCore.Mvc;
using UEFA.UCL.Classic.Contracts.Common;
using System;
using UEFA.UCL.Classic.Contracts.Settings;

namespace UEFA.UCL.Classic.API.Controllers.Settings
{
    public partial class SettingsController
    {
        /// <summary>
        /// Gets the list of subscription
        /// </summary>
        /// <param name="optType">1</param>
        /// <param name="guid">User GUID from Cookie</param>
        /// <param name="backdoor">Debug param</param>
        /// <returns></returns>
        [Route("user/{guid}/subscribe-info")]
        [HttpGet]
        public ActionResult<HTTPResponse> SubscribeInfo(Int64 optType, String guid, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {
                    HTTPResponse httpResponse = _NewsletterContext.SubscribeInfo(optType);

                    return Ok(httpResponse);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// Updates a user's subscription
        /// </summary>
        /// <param name="subscribe">payload</param>
        /// <param name="backdoor">Debug param</param>
        /// <returns></returns>
        [Route("user/subscribe")]
        [HttpPost]
        public ActionResult<HTTPResponse> Subscription(UserSubscribe subscribe, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {
                    HTTPResponse httpResponse = _NewsletterContext.Subscription(subscribe);

                    return Ok(httpResponse);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }
    }
}