using Google.Cloud.Firestore;

namespace Data_Organizer_Server.Models
{
    [FirestoreData]
    public class DeviceInfoModel
    {
        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string Model { get; set; }

        [FirestoreProperty]
        public string Manufacturer { get; set; }

        [FirestoreProperty]
        public string Platform { get; set; }

        [FirestoreProperty]
        public string Idiom { get; set; }

        [FirestoreProperty]
        public string DeviceType { get; set; }

        [FirestoreProperty]
        public string Version { get; set; }

        [FirestoreProperty]
        public string DeviceInfoCombined => $"{Name}_{Model}_{Manufacturer}_{Platform}_{Idiom}_{DeviceType}";
    }
}
