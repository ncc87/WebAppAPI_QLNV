using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace WebAppAPI_QLNV.Data
{
    [Table("NhanVien")]
    public class NhanVien
    {
        [Key]
        public string MaNV { get; set; }

        [Required]
        [MaxLength(100)]
        public string HoTen { get; set; }

        [Range(0, int.MaxValue)]
        public int NamSinh { get; set; }

        public bool GioiTinh { get; set; }
    }
}
