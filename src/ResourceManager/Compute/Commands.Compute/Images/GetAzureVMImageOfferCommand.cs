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

using Microsoft.Azure.Commands.Compute.Common;
using Microsoft.Azure.Commands.Compute.Models;
using Microsoft.Azure.Management.Compute;
using Microsoft.Azure.Management.Compute.Models;
using System.Linq;
using System.Management.Automation;

namespace Microsoft.Azure.Commands.Compute
{
    [Cmdlet(VerbsCommon.Get, ProfileNouns.VirtualMachineImageOffer)]
    [OutputType(typeof(PSVirtualMachineImageOffer))]
    public class GetAzureVMImageOfferCommand : VirtualMachineImageBaseCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true), ValidateNotNullOrEmpty]
        public string Location { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true), ValidateNotNullOrEmpty]
        public string PublisherName { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            ExecuteClientAction(() =>
            {
                var parameters = new VirtualMachineImageListOffersParameters
                {
                    Location = Location.Canonicalize(),
                    PublisherName = PublisherName
                };

                VirtualMachineImageResourceList result = this.VirtualMachineImageClient.ListOffers(parameters);

                var images = from r in result.Resources
                             select new PSVirtualMachineImageOffer
                             {
                                 RequestId = result.RequestId,
                                 StatusCode = result.StatusCode,
                                 Id = r.Id,
                                 Location = r.Location,
                                 Offer = r.Name,
                                 PublisherName = this.PublisherName
                             };

                WriteObject(images, true);
            });
        }
    }
}
