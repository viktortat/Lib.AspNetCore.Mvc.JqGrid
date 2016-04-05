﻿using Microsoft.AspNet.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.AspNetCore.Mvc.JqGrid.Core.Request.ModelBinders
{
    /// <summary>
    /// A base IModelBinder which binds JqGridCellUpdateReques.
    /// </summary>
    public abstract class JqGridCellUpdateRequestModelBinder : IModelBinder
    {
        #region Properties
        /// <summary>
        /// Gets the dictionary of supported cells.
        /// </summary>
        protected abstract IDictionary<string, Type> SupportedCells { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Attempts to bind a model.
        /// </summary>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>The result of the model binding process.</returns>
        public Task<ModelBindingResult> BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            string key = (bindingContext.IsTopLevelObject) ? (bindingContext.BinderModelName ?? String.Empty) : bindingContext.ModelName;

            Task<ModelBindingResult> jqGridCellUpdateRequestModelBindingResult;
            if (bindingContext.ModelType == typeof(JqGridCellUpdateRequest))
            {
                JqGridCellUpdateRequest model = new JqGridCellUpdateRequest
                {
                    Id = bindingContext.ValueProvider.GetValue("id").ConvertTo<string>()
                };

                foreach (KeyValuePair<string, Type> supportedCell in SupportedCells)
                {
                    if (BindCellProperties(model, bindingContext, supportedCell.Key, supportedCell.Value))
                    {
                        break;
                    }
                }

                jqGridCellUpdateRequestModelBindingResult = ModelBindingResult.SuccessAsync(key, model);
            }
            else
            {
                jqGridCellUpdateRequestModelBindingResult = ModelBindingResult.FailedAsync(key);
            }

            return jqGridCellUpdateRequestModelBindingResult;
        }

        private bool BindCellProperties(JqGridCellUpdateRequest model, ModelBindingContext bindingContext, string cellName, Type cellType)
        {
            bool cellBinded = false;

            ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(cellName);
            if (valueProviderResult != ValueProviderResult.None)
            {
                model.CellName = cellName;
                model.CellValue = valueProviderResult.ConvertTo(cellType);
            }

            return cellBinded;
        }
        #endregion
    }
}