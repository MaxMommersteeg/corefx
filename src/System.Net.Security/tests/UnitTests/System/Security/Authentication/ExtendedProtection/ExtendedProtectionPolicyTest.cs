﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;

using Xunit;

namespace System.Net.Security.Tests
{
    public class ExtendedProtectionPolicyTest
    {
        [Fact]
        public void Constructor_PolicyEnforcement_NeverParam()
        {
            AssertExtensions.Throws<ArgumentException>("policyEnforcement", () => new ExtendedProtectionPolicy(PolicyEnforcement.Never, ProtectionScenario.TransportSelected, null));
        }

        [Fact]
        public void Constructor_ServiceNameCollection_ZeroElementsParam()
        {
            var paramValue = new ServiceNameCollection(new List<string>());
            AssertExtensions.Throws<ArgumentException>("customServiceNames", () => new ExtendedProtectionPolicy(PolicyEnforcement.Always, ProtectionScenario.TransportSelected, paramValue));
        }

        [Fact]
        public void Constructor_PolicyEnforcementChannelBinding_NeverParam()
        {
            var customChannelBinding = new MockCustomChannelBinding();
            AssertExtensions.Throws<ArgumentException>("policyEnforcement", () => new ExtendedProtectionPolicy(PolicyEnforcement.Never, customChannelBinding));
        }

        [Fact]
        public void Constructor_ChannelBinding_NullParam()
        {
            AssertExtensions.Throws<ArgumentNullException>("customChannelBinding", () => new ExtendedProtectionPolicy(PolicyEnforcement.Always, null));
        }

        [Fact]
        public void Constructor_CollectionParam()
        {
            var paramValue = new List<string> { "Test1", "Test2" };
            var expectedServiceNameCollection = new ServiceNameCollection(paramValue);
            var extendedProtectionPolicy = new ExtendedProtectionPolicy(PolicyEnforcement.Always, ProtectionScenario.TransportSelected, paramValue);

            Assert.Equal(2, extendedProtectionPolicy.CustomServiceNames.Count);
            Assert.Equal(expectedServiceNameCollection, extendedProtectionPolicy.CustomServiceNames);
        }

        [Fact]
        public void Constructor_PolicyEnforcement_MembersAreSet()
        {
            // Arrange
            var expectedPolicyEnforcement = PolicyEnforcement.Never;
            var expectedProtectionScenario = ProtectionScenario.TransportSelected;

            // Act
            var extendedProtectionPolicy = new ExtendedProtectionPolicy(expectedPolicyEnforcement);

            // Assert
            Assert.Equal(expectedPolicyEnforcement, extendedProtectionPolicy.PolicyEnforcement);
            Assert.Equal(expectedProtectionScenario, extendedProtectionPolicy.ProtectionScenario);
        }

        [Fact]
        public void ExtendedProtectionPolicy_OSSupportsExtendedProtection()
        {
            Assert.True(ExtendedProtectionPolicy.OSSupportsExtendedProtection);
        }

        [Fact]
        public void ExtendedProtectionPolicy_Properties()
        {
            // Arrange
            var policyEnforcementParam = PolicyEnforcement.Always;
            var protectionScenarioParam = ProtectionScenario.TransportSelected;
            var customChannelBindingParam = new MockCustomChannelBinding();

            // Act
            var extendedProtectionPolicy = new ExtendedProtectionPolicy(policyEnforcementParam, customChannelBindingParam);

            // Assert
            Assert.Null(extendedProtectionPolicy.CustomServiceNames);
            Assert.Equal(policyEnforcementParam, extendedProtectionPolicy.PolicyEnforcement);
            Assert.Equal(protectionScenarioParam, extendedProtectionPolicy.ProtectionScenario);
            Assert.Equal(customChannelBindingParam, extendedProtectionPolicy.CustomChannelBinding);
        }

        [Fact]
        public void ExtendedProtectionPolicy_ToString()
        {
            // Arrange
            var serviceName1 = "Test1";
            var serviceName2 = "Test2";
            var serviceNameCollectionParam = new ServiceNameCollection(new List<string> { serviceName1, serviceName2 });
            var policyEnforcementParam = PolicyEnforcement.Always;
            var protectionScenarioParam = ProtectionScenario.TransportSelected;
            var extendedProtectionPolicy = new ExtendedProtectionPolicy(policyEnforcementParam, protectionScenarioParam, serviceNameCollectionParam);
            var expectedString = $"ProtectionScenario={protectionScenarioParam}; PolicyEnforcement={policyEnforcementParam}; CustomChannelBinding=<null>; ServiceNames={serviceName1}, {serviceName2}";

            // Act
            var result = extendedProtectionPolicy.ToString();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedString, result);
        }

        [Fact]
        public void ExtendedProtectionPolicy_NoCustomServiceNamesAndNoCustomChannelBinding_ToString()
        {
            // Arrange
            var policyEnforcementParam = PolicyEnforcement.Always;
            var extendedProtectionPolicy = new ExtendedProtectionPolicy(policyEnforcementParam);
            var expectedString = $"ProtectionScenario={extendedProtectionPolicy.ProtectionScenario}; PolicyEnforcement={policyEnforcementParam}; CustomChannelBinding=<null>; ServiceNames=<null>";

            // Act
            var result = extendedProtectionPolicy.ToString();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedString, result);
        }

        [Fact]
        public void ExtendedProtectionPolicy_NoCustomServiceNames_ToString()
        {
            // Arrange
            var policyEnforcementParam = PolicyEnforcement.Always;
            var channelBinding = new MockCustomChannelBinding();
            var extendedProtectionPolicy = new ExtendedProtectionPolicy(policyEnforcementParam, channelBinding);
            var expectedChannelBindingString = channelBinding.ToString();
            var expectedString = $"ProtectionScenario={extendedProtectionPolicy.ProtectionScenario}; PolicyEnforcement={policyEnforcementParam}; CustomChannelBinding={expectedChannelBindingString}; ServiceNames=<null>";

            // Act
            var result = extendedProtectionPolicy.ToString();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedString, result);
        }

        public class MockCustomChannelBinding : ChannelBinding
        {
            protected override bool ReleaseHandle()
            {
                throw new NotImplementedException();
            }

            public override int Size { get; }
        }
    }
}
