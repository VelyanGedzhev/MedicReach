namespace MedicReach.Data
{
    public class DataConstants
    {
        public class MedicalCenter
        {
            public const int NameMinLength = 5;
            public const int NameMaxLength = 30;
            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 400;
            public const string DefaultImageUrl = "https://ehrintelligence.com/images/site/article_headers/_normal/rural_hospital_access.jpg";
        }

        public class Address
        {
            public const int NameMaxLength = 30;
            public const int CityMaxLength = 40;
        }

        public class Physician
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 30;
            public const string DefaultMaleImageUrl = "https://i.pinimg.com/736x/cc/9b/3b/cc9b3b4f000047d3830eb98c9c630ccc.jpg";
            public const string DefaultFemaleImageUrl = "https://png.pngtree.com/png-vector/20190811/ourlarge/pngtree-doctor-superwoman-png-image_1690213.jpg";
            public const string GenderMale = "Male";
            public const int ExaminationPriceMinValie = 0;
            public const int ExaminationPriceMaxValie = 600;
        }

        public class Speciality
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 20;

        }

        public class County
        {
            public const int NameMinLength = 5;
            public const int NameMaxLength = 70;
            public const int Alpha3CodeMinLength = 3;
            public const int Alpha3CodeMaxLength = 3;
        }
    }
}
