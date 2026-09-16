using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace iParking.API.Filters
{
    /// <summary>
    /// Filtro para validación de modelos.
    /// Centraliza la validación de DTOs aplicando DRY y Clean Code.
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                context.Result = new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Errores de validación",
                    errors = errors
                });
            }
        }
    }
}
