using DevExtreme.AspNet.Data;

namespace Arslan.Vms.OrderService.v1
{

	//[ModelBinder(BinderType = typeof(DataSourceLoadOptionsBinder))]
	public class DataSourceLoadOptions : DataSourceLoadOptionsBase
    {
    }

    //public class DataSourceLoadOptionsBinder : IModelBinder
    //{

    //    public Task BindModelAsync(ModelBindingContext bindingContext)
    //    {
    //        var loadOptions = new DataSourceLoadOptions();
    //        DataSourceLoadOptionsParser.Parse(loadOptions, key => bindingContext.ValueProvider.GetValue(key).FirstOrDefault());
    //        bindingContext.Result = ModelBindingResult.Success(loadOptions);
    //        return Task.CompletedTask;
    //    }

    //}

}

