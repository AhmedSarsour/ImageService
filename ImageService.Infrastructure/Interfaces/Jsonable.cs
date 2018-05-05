using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure.Interfaces
{
    /// <summary>
    /// An interface of object we want to make to json and convert from json.
    /// </summary>
    public interface Jsonable
    {
        string ToJSON();
        void FromJson(string str);
    }
}
