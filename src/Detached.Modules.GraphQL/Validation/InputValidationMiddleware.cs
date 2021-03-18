using HotChocolate;
using HotChocolate.Resolvers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Detached.Modules.GraphQL.Validation
{
    public class InputValidationMiddleware
    {
        private readonly FieldDelegate _next;

        public InputValidationMiddleware(FieldDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(IMiddlewareContext context)
        {
            if (context.FieldSelection.Arguments.Count > 0)
            {
                foreach (var argument in context.FieldSelection.Arguments)
                {
                    var value = context.ArgumentValue<object>(argument.Name.Value);
                    Validate(value, context);
                }
            }

            await _next(context);
        }

        public void Validate(object instance, IMiddlewareContext context)
        {
            ValidationContext validationContext = new ValidationContext(instance);

            if (instance != null)
            {
                foreach (PropertyInfo propInfo in instance.GetType().GetRuntimeProperties())
                {
                    foreach (ValidationAttribute validationAttr in propInfo.GetCustomAttributes().OfType<ValidationAttribute>())
                    {
                        validationContext.DisplayName = propInfo.Name;

                        object value = propInfo.GetValue(instance);
                        ValidationResult result = validationAttr.GetValidationResult(value, validationContext);
                        if (result != null)
                        {
                            Dictionary<string, object> extensions = new Dictionary<string, object>();

                            foreach (PropertyInfo argPropInfo in validationAttr.GetType().GetTypeInfo().DeclaredProperties)
                            {
                                extensions[argPropInfo.Name] = argPropInfo.GetValue(validationAttr);
                            }

                            extensions.Add("key", "ValidationError");
                            extensions.Add("field", propInfo.Name);
                            extensions.Add("validator", validationAttr.GetType().Name);

                            context.ReportError(
                                new Error(result.ErrorMessage, "ValidationError", context.Path, extensions: extensions)
                            );
                        }
                    }
                }
            }
        }
    }
}