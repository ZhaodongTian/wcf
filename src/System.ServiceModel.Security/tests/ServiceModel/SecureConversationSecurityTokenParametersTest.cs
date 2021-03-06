﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using System.ServiceModel.Security.Tokens;
using Infrastructure.Common;
using Xunit;

public static class SecureConversationSecurityTokenParametersTest
{
    [WcfTheory]
    [InlineData(false)]
    [InlineData(true)]
    public static void RequireCancellation_Property_Is_Settable(bool value)
    {
        SecureConversationSecurityTokenParameters scstp = new SecureConversationSecurityTokenParameters();
        scstp.RequireCancellation = value;
        Assert.Equal(value, scstp.RequireCancellation);
    }

    [WcfFact]
    public static void RequireCancellation_DefaultValueIsSameAsNetFramework()
    {
        SecureConversationSecurityTokenParameters scstp = new SecureConversationSecurityTokenParameters();
        Assert.True(scstp.RequireCancellation);
    }
}
