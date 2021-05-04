using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unlock_282
{
    public interface ResolveCaptcha
    {
        Task<string> GetPhone();
        Task<string> GetCode();
    }
}
