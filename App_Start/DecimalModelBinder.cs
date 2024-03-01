using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GenerateurDFUSafir.App_Start
{
    public abstract class DecimalModelBinder<T> : DefaultModelBinder
    {
        protected abstract Func<string, IFormatProvider, T> ConvertFunc { get; }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == null) return base.BindModel(controllerContext, bindingContext);
            if (string.IsNullOrWhiteSpace (valueProviderResult.AttemptedValue))
            {
                return ConvertFunc.Invoke("0", CultureInfo.CurrentUICulture);
            }
            try
            {
                return ConvertFunc.Invoke(valueProviderResult.AttemptedValue, CultureInfo.CurrentUICulture);
            }
            catch (FormatException)
            {
                // If format error then fallback to InvariantCulture instead of current UI culture
                return ConvertFunc.Invoke(valueProviderResult.AttemptedValue, CultureInfo.InvariantCulture);
            }
        }


    }
    public class DoubleModelBinder : DecimalModelBinder<double>
    {
        protected override Func<string, IFormatProvider, double> ConvertFunc => Convert.ToDouble;
    }
}