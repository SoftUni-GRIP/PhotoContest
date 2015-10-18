namespace PhotoContest.Web.Infrastructure.MetaDataProvider.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;

    public class LabelConventionFilter : IModelMetadataFilter
    {
        public void TransformMetadata(ModelMetadata metadata,
            IEnumerable<Attribute> attributes)
        {
            if (!string.IsNullOrEmpty(metadata.PropertyName) &&
                string.IsNullOrEmpty(metadata.DisplayName))
            {
                metadata.DisplayName = GetStringWithSpaces(metadata.PropertyName);
            }
        }

        private string GetStringWithSpaces(string input)
        {
            return Regex.Replace(
               input,
               "(?<!^)" +
               "(" +
               "  [A-Z][a-z] |" +
               "  (?<=[a-z])[A-Z] |" +
               "  (?<![A-Z])[A-Z]$" +
               ")",
               " $1",
               RegexOptions.IgnorePatternWhitespace);
        }
    }
}