﻿namespace Microsoft.WindowsAzure.Commands.ExpressRoute.Test
{
    using System;
    using System.Collections.Generic;
    using Commands.ExpressRoute;
    using VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Utilities.ExpressRoute;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Commands.Test.Utilities.Common;
    using Microsoft.WindowsAzure.Management.ExpressRoute;
    using Microsoft.WindowsAzure.Management.ExpressRoute.Models;

    [TestClass]
    public class AzureDedicatedCircuitServiceProviderTests : TestBase
    {
        private const string SubscriptionId = "foo";

        private static Mock<ExpressRouteManagementClient> InitExpressRouteManagementClient()
        {
            return
                (new Mock<ExpressRouteManagementClient>(
                    new CertificateCloudCredentials(SubscriptionId, new X509Certificate2(new byte[] { })),
                    new Uri("http://someValue")));
        }

        [TestMethod]
        public void ListAzureDedicatedCircuitServiceProviderSuccessful()
        {
            // Setup

            var serviceProviderName = "TestServiceProvider1";
            var serviceProviderName2 = "TestServiceProvier2";
            var type1 = "IXP";
            var type2 = "Telco";
           

            MockCommandRuntime mockCommandRuntime = new MockCommandRuntime();
            Mock<ExpressRouteManagementClient> client = InitExpressRouteManagementClient();
            var dcsMock = new Mock<IDedicatedCircuitServiceProviderOperations>();

            List<AzureDedicatedCircuitServiceProvider>
                dedicatedCircuitServiceProviders = new List
                    <AzureDedicatedCircuitServiceProvider>()
                {
                    new AzureDedicatedCircuitServiceProvider()
                    {
                        DedicatedCircuitBandwidths =
                            new DedicatedCircuitBandwidth[1]
                            {
                                new DedicatedCircuitBandwidth()
                                {
                                    Bandwidth = 10,
                                    Label = "T1"
                                }
                            },
                        DedicatedCircuitLocations = "us-west",
                        Name = serviceProviderName,
                        Type = type1
                    },
                    new AzureDedicatedCircuitServiceProvider()
                    {
                        DedicatedCircuitBandwidths =
                            new DedicatedCircuitBandwidth[1]
                            {
                                new DedicatedCircuitBandwidth()
                                {
                                    Bandwidth = 10,
                                    Label = "T1"
                                }
                            },
                        DedicatedCircuitLocations = "us-west",
                        Name = serviceProviderName2,
                        Type = type2
                    }
                };
            DedicatedCircuitServiceProviderListResponse expected =
                new DedicatedCircuitServiceProviderListResponse()
                {
                    DedicatedCircuitServiceProviders = dedicatedCircuitServiceProviders,
                    StatusCode = HttpStatusCode.OK
                };

            var t = new Task<DedicatedCircuitServiceProviderListResponse>(() => expected);
            t.Start();

            dcsMock.Setup(f => f.ListAsync(It.IsAny<CancellationToken>())).Returns((CancellationToken cancellation) => t);
            client.SetupGet(f => f.DedicatedCircuitServiceProvider).Returns(dcsMock.Object);

            GetAzureDedicatedCircuitServiceProviderCommand cmdlet = new GetAzureDedicatedCircuitServiceProviderCommand()
            {
                CommandRuntime = mockCommandRuntime,
                ExpressRouteClient = new ExpressRouteClient(client.Object)
            };

            cmdlet.ExecuteCmdlet();

            // Assert
            IEnumerable<AzureDedicatedCircuitServiceProvider> actual =
                mockCommandRuntime.OutputPipeline[0] as IEnumerable<AzureDedicatedCircuitServiceProvider>;
            Assert.AreEqual(actual.ToArray().Count(), 2);
        }
    }
}