using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PhotoContest.Web.Infrastructure.MetaDataProvider
{
    public interface IModelMetadataFilter
    {
        void TransformMetadata(ModelMetadata metadata,
            IEnumerable<Attribute> attributes);
    }
}