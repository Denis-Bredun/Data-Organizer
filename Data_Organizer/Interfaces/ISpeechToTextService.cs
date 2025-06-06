﻿using System.Globalization;

namespace Data_Organizer.Interfaces
{
    public interface ISpeechToTextService
    {
        Task<bool> RequestPermissionsAsync();
        Task<string> ListenAsync(CultureInfo culture,
            IProgress<string> recognitionResult,
            CancellationToken cancellationToken);
    }
}
