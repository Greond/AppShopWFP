using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShopWFP.Models
{
    public class PropertyView
    {
        public PropertyView(string propertyName, object propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }
        public string PropertyName { get; set; }
        public object PropertyValue { get; set; }
    }
}
