namespace PhotoContest.Web.Infrastructure.MetaDataProvider
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IModelMetadataFilter
    {
        void TransformMetadata(ModelMetadata metadata,
            IEnumerable<Attribute> attributes);
    }
}