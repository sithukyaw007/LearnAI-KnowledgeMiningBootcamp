// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json;

namespace Microsoft.CognitiveSearchBot
{
    public class Entity
    {
        public string GroupName { get; set; }
        public double Score { get; set; }

        public object Value { get; set; }
    }    
}
