using System.ComponentModel.DataAnnotations;


// Модель работы с базой даных
namespace TestTaskAwwcor
{
    public class Ad
    {

        public string Id { get; set; } = "";

        [Required(ErrorMessage = "Введите название")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "В названии должо быть от 3 до 200 символов")]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Укажите стоимость")]
        [Range(1, 1000000, ErrorMessage = "Укажите стоимость от 1 до 1000000")]
        public float Price { get; set; }

        [Required(ErrorMessage = "Введите описание")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "В описании должно быть от 10 до 1000 символов")]
        public string Description { get; set; }
        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
}
