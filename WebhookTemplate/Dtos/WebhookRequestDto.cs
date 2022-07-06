//==============================================================================
// Copyright (c) Profisee Corporation. All Rights Reserved.
//==============================================================================

using System.Runtime.Serialization;

namespace Profisee.WebhookTemplate.Dtos
{
    /// <summary>
    /// Simple Data Transfer Object (dto) for receiving a typed webhook request payload.
    /// </summary>
    [DataContract]
    public class WebhookRequestDto
    {
        /// <summary>
        /// For the workflow configured for this example, the webhook activity sends a payload with the code of the product entity.
        /// </summary>
        [DataMember]
        public string Code { get; set; }
    }
}
