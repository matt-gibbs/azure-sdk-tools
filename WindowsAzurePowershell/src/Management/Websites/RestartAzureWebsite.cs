﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

namespace Microsoft.WindowsAzure.Management.Websites
{
    using System.Management.Automation;
    using Microsoft.WindowsAzure.Management.Utilities.Websites;
    using Microsoft.WindowsAzure.Management.Utilities.Websites.Common;
    using Microsoft.WindowsAzure.Management.Utilities.Websites.Services;


    [Cmdlet(VerbsLifecycle.Restart, "AzureWebsite"), OutputType(typeof(bool))]
    public class RestartAzureWebsiteCommand : WebsiteContextBaseCmdlet
    {
        public IWebsitesClient WebsitesClient { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter PassThru { get; set; }

        /// <summary>
        /// Initializes a new instance of the RestartAzureWebsiteCommand class.
        /// </summary>
        public RestartAzureWebsiteCommand()
            : this(null)
        {
        }

        public RestartAzureWebsiteCommand(IWebsitesServiceManagement channel)
        {
            Channel = channel;
        }

        public override void ExecuteCmdlet()
        {
            WebsitesClient = WebsitesClient ?? new WebsitesClient(CurrentSubscription, WriteDebug);
            WebsitesClient.RestartAzureWebsite(Name);

            if (PassThru.IsPresent)
            {
                WriteObject(true);
            }
        }
    }
}
