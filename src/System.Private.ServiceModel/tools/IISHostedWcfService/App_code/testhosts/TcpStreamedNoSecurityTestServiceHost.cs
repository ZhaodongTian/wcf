﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NET
using CoreWCF;
using CoreWCF.Channels;
#else
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
#endif

namespace WcfService
{
    [TestServiceDefinition(Schema = ServiceSchema.NETTCP, BasePath = "TcpStreamedNoSecurity.svc")]
    public class TcpStreamedNoSecurityTestServiceHost : TestServiceHostBase<IWcfService>
    {
        protected override string Address { get { return "tcp-streamed-nosecurity"; } }

        protected override Binding GetBinding()
        {
            return new NetTcpBinding(SecurityMode.None)
            {
                TransferMode = TransferMode.Streamed
            };
        }

        public TcpStreamedNoSecurityTestServiceHost(params Uri[] baseAddresses)
            : base(typeof(WcfService), baseAddresses)
        {
        }
    }
}
