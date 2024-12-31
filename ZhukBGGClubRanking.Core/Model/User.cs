using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Xml.Serialization;
using ZhukBGGClubRanking.Core.Model;

namespace ZhukBGGClubRanking.Core
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле имя должно быть заполнено")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина имени должна быть в диапазоне от {2}-{1} символов")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Поле пароль должно быть заполнено")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Длина пароля должна быть в диапазоне от {2}-{1} символов")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Поле email должно быть заполнено")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Длина email должна быть в диапазоне от {2}-{1} символов")]
        public string EMail { get; set; } = "";

        [Required(ErrorMessage = "Поле полное имя должно быть заполнено")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Длина полного имени должна быть в диапазоне от {2}-{1} символов")]
        public string FullName { get; set; } = "";

        [Required(ErrorMessage = "Поле активность должно быть заполнено")]
        public bool IsActive { get; set; } = true;

        public DateTime CreateTime { get; set; }
        public int CreateUserId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина роли должна быть в диапазоне от {2}-{1} символов")]
        public string Role { get; set; }

        //private static string NOT_EMPTY = "Поле {0} не может быть пустым";
        //private static string NOT_FITTING_SIZE = "Поле {0} должно быть от {1} до {2} символов";

        public static string GetMD5Hash(string value)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        //public void TrimAndNamePasswordToLower()
        //{
        //    TrimFields();
        //    Name = Name.ToLower();
        //    Password = Name.ToLower();
        //}

        public void TrimFields()
        {
            Name = Name.Trim();
            Password = Password.Trim();
            EMail = EMail.Trim();
            FullName = FullName.Trim();
            Role = Role.Trim();
        }

        public void HashPassword()
        {
            Password = GetMD5Hash(Password);
        }

        public bool ValidateBeforeCreateOrChange(out string errorMessage)
        {
            errorMessage = "";
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this);
            if (!Validator.TryValidateObject(this, context, results, true))
            {
                foreach (var error in results)
                {
                    errorMessage += error + ", ";
                }
                if (errorMessage.Length > 3)
                    errorMessage = errorMessage.Substring(0, errorMessage.Length - 2);
                return false;
            }

            return true;
        }
    }
}