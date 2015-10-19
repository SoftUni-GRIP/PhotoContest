namespace PhotoContest.Web.ModelBinders
{
    using System;
    using System.Web.Mvc;
    using Infrastructure.Utils;
    using PhotoContest.Models.Contracts;

    public class EntityModelBinderProviderById : IModelBinderProvider
    {
        public IModelBinder GetBinder(Type modelType)
        {
            if (!typeof(IEntity).IsAssignableFrom(modelType))
            {
                return null;
            }

            Type modelBinderType = typeof(EntityModelBinder<>).MakeGenericType(modelType);
            var modelBinder = ObjectFactory.GetInstance(modelBinderType);
            return (IModelBinder)modelBinder;
        }
    }

}