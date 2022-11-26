using Microsoft.EntityFrameworkCore;
using WebAppAPI_QLNV.Models;

namespace WebAppAPI_QLNV.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options): base(options) { }


        #region DbSet
        public DbSet<NhanVien> NhanViens { get; set; }
        #endregion
    }
}
