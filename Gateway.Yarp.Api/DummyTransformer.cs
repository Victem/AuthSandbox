using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Yarp.ReverseProxy.Transforms.Builder;

namespace Gateway.Yarp.Api
{
    public class DummyTransformer : ITransformProvider
    {
        public void Apply(TransformBuilderContext context)
        {
            throw new NotImplementedException();
        }

        public void ValidateCluster(TransformClusterValidationContext context)
        {
            throw new NotImplementedException();
        }

        public void ValidateRoute(TransformRouteValidationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
