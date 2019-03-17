using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace App5.Models
{
    [Table("Products")]
    class Product : INotifyPropertyChanged
    {
        private string productName;
            private string productPrice;
        public Product(string productName = "", string productBarcode = "", string productPrice = "")
        {
            ProductName = productName;
            ProductBarcode = productBarcode;
            ProductPrice = productPrice;
        }
        public int Id
        {
            get;
            set;
        }

        public string ProductName
        {
            get
            {
                return productName;
            }
            set
            {
                productName = value;
                NotifyChange();
            }
        }

        public string ProductBarcode
        {
            get;
            set;
        }

        public string ProductPrice
        {
            get
            {
                return productPrice;
            }
            set
            {
                productPrice = value;
                NotifyChange();
            }
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyChange([CallerMemberName] string memberName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }
    }
}
