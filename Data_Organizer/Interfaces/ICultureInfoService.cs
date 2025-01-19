using Data_Organizer.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Organizer.Interfaces
{
    public interface ICultureInfoService
    {
        CultureInfo GetCultureInfoFromLanguage(LanguageModel language);
    }
}
