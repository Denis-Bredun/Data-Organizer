﻿using Data_Organizer.MVVM.Models;
using Data_Organizer.MVVM.Models.Enums;

namespace Data_Organizer.Interfaces
{
    public interface IFileServiceDecorator
    {
        Task<string> ImportTextAsync();
        Task ExportTextAsync(string text, TextFileFormat textFileFormat);
        Task<string> ImportAudiofileAsync(LanguageModel selectedLanguage);
    }
}
