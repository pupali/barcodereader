using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Validation;
using System.Diagnostics;

namespace App5.Models
{
    class FileImportContext : ValidatableModelBase
    {
        public FileImportContext()
        {
            IsSet = false;
            EqualsError = "";
            Validator = (model) =>
            {
                var context = model as FileImportContext;
                context.Errors.Clear();
                Debug.WriteLine("Price Index = " + context.PriceIdx);
                //if (int)
                //{

                //}
                if (context.PriceIdx >= 10 || context.PriceIdx == -1)
                {
                    context.Properties[nameof(context.PriceIdx)].Errors.Add("Price Index Must be Integer and Less Than 10");
                    context.IsSet = false;
                }
                if (context.BarcodeIdx >= 10 || context.BarcodeIdx == -1)
                {
                    context.Properties[nameof(context.BarcodeIdx)].Errors.Add("Barcode Index Must be Integer and Less Than 10");
                    context.IsSet = false;
                }
                if (context.ProductNameIdx >= 10 || context.ProductNameIdx == -1)
                {
                    context.Properties[nameof(context.ProductNameIdx)].Errors.Add("Product Name Index Must be Integer and Less Than 10");
                    context.IsSet = false;
                }
                if (context.Properties[nameof(ProductNameIdx)].IsValid && context.Properties[nameof(PriceIdx)].IsValid && context.Properties[nameof(BarcodeIdx)].IsValid)
                {
                    if (context.PriceIdx == context.ProductNameIdx || context.PriceIdx == context.BarcodeIdx || context.BarcodeIdx == context.ProductNameIdx )
                    {
                        context.Properties["EqualsError"].Errors.Add("Indices Can't Be Equal");
                        context.IsSet = false;
                    } else context.IsSet = true;
                }
            };
        }

        public bool IsSet
        {
            get;
            set;
        }
       
        public int PriceIdx
        {
            get { return Read<int>(); }
            set { Write(value); }
        }

        public int BarcodeIdx
        {
            get { return Read<int>(); }
            set { Write(value); }
        }

        public int ProductNameIdx
        {
            get { return Read<int>(); }
            set { Write(value); }
        }

        public string EqualsError
        {
            get { return Read<string>(); }
            set { Write(value); }
        }
    }
}
