using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using System.Threading.Tasks;

namespace Shared.Api.Configuration.Binders
{
    internal class CurrentCultureDecimalBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            decimal.TryParse(value.FirstValue, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal decimalValue);

            bindingContext.Result = ModelBindingResult.Success(decimalValue);
            return Task.CompletedTask;
        }
    }
}