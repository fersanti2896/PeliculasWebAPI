using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace SPeliculasAPI.Helpers {
    public class TypeBinder<T> : IModelBinder {
        public Task BindModelAsync(ModelBindingContext bindingContext) {
            var nameProp = bindingContext.ModelName;
            var provValues = bindingContext.ValueProvider.GetValue(nameProp);

            if (provValues == ValueProviderResult.None) { return Task.CompletedTask; }

            try {
                var valueDes = JsonConvert.DeserializeObject<T>(provValues.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(valueDes);
            }
            catch (Exception) {
                bindingContext.ModelState.TryAddModelError(nameProp, $"Valor invalido para tipo List");
            }

            return Task.CompletedTask;
        }
    }
}
